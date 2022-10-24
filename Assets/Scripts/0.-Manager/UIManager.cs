using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager InstanceGUI;
    //{
    //    get;
    //    private set;
    //}

    [Header("Dialogue Variables")]
    public Image blackScreen;
    public bool fadeBlack;
    public bool fadeFrom;
    public float fadeSpeed;
    public Image icono;
    public Image ballonDialogue;


    [Header("Bondad Variables")]
    public Image barraBondad;
    public float currenHealth;
    public float maxHealth; 
    public float healthToModify;
    public float velocidadBarra;
    public Color32 colorBarra;

    [Header("Circle Variables")]
    public Image circulo;
    public RectTransform rtCircle;
    public Vector3 rtCircleV;
    public Vector3 destiny;
    public float puntos;
    public float puntosCalificacion;

    [Header("Mapa")]
    public GameObject obMap;
    public GameObject obMapMark;


    [Header("Fade")]
    public Animator obAnim;
    public float timeTransicion;

    [Header("Canvas")]
    [SerializeField] GameObject[] HUDLienzos;



    void ForceScales()
    {

        Screen.SetResolution(1280, 720, true);

    }
    public void ExitPlayGame()
    {
       

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        
    }
    public void DialogueFadeIn()
    {
        if (fadeBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0.75f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0.75f)
            {
                fadeBlack = false;

            }
        }

        if (fadeFrom)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFrom = false;

            }
        }

    }
    IEnumerator UpdateUI(bool add, float cantidad)
    {
        if (add)
        {
            currenHealth += cantidad;

            if (currenHealth > maxHealth)
            {
                currenHealth = maxHealth;
            }
        }
        else
        {
            currenHealth -= cantidad;

            if (currenHealth < 0)
            {
                currenHealth = 0;
            }
        }
             
        healthToModify = (currenHealth / maxHealth);


        switch (currenHealth)
        {
            case 1:
                barraBondad.color = new Color32(190, 75, 50, 255);
                break;
            case 2:
                barraBondad.color = new Color32(190, 100, 50,255);
                break;
            case 3:
                barraBondad.color = new Color32(190, 125, 50,255);
                break;
            case 4:
                barraBondad.color = new Color32(190, 150, 50,255);
                break;
            case 5:
                barraBondad.color = new Color32(190, 175, 50,255);
                break;
            case 6:
                barraBondad.color = new Color32(175, 190, 50,255);
                break;
            case 7:
                barraBondad.color = new Color32(150, 190, 50,255);
                break;
            case 8:
                barraBondad.color = new Color32(125, 190, 50,255);
                break;


        }
        while ((barraBondad.fillAmount < healthToModify) || (barraBondad.fillAmount > healthToModify))
        {
            barraBondad.fillAmount = Mathf.MoveTowards(barraBondad.fillAmount, healthToModify, velocidadBarra * Time.deltaTime);

            yield return null;
        }

    }
    public void GanarPuntos(bool add, float puntos)
    {
       
            if (add)
            {

            if (puntosCalificacion < 300)
            {
                rtCircleV = (rtCircle.position + (Vector3.right * puntos));

                puntosCalificacion += puntos;

                StartCoroutine(MoverBarra(add));
            }
            }
            else
            {
        

            if (puntosCalificacion > -100)
            {
                rtCircleV = (rtCircle.position + (Vector3.left * puntos));


                puntosCalificacion -= puntos;

                StartCoroutine(MoverBarra(add));

            }

        }
        

    }
    IEnumerator MoverBarra(bool subir)
    {
        if (subir)
        {
            while (rtCircle.position.x < rtCircleV.x)
            {
                rtCircle.Translate(Vector3.right * Time.deltaTime * velocidadBarra);
                yield return null;
            }
        }
        else
        {
            while (rtCircle.position.x > rtCircleV.x)
            {
                rtCircle.Translate(Vector3.left * Time.deltaTime * velocidadBarra);
                yield return null;
            }
        }
    }
    public void PosicionarGlobo(Vector3 posicionar)
    {
        Vector3 posDisplay = FollowCameras.instance.MyCameras.WorldToScreenPoint(posicionar);
        ballonDialogue.rectTransform.position = posDisplay + destiny;
    }
    public IEnumerator Fundido()
    {

        obAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        obAnim.SetTrigger("End");

    }

    public void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(SceneLoading(nextScene));
    }

    public IEnumerator SceneLoading(int sceneIndex)
    {
        obAnim.SetTrigger("StartTransition");
        yield return new  WaitForSeconds(timeTransicion);
        HUDLienzos[0].SetActive(false);
        HUDLienzos[1].SetActive(true);
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
        
        SceneManager.LoadScene(sceneIndex);
    }

    private void Awake()
    {
        if (InstanceGUI == null /*&& Instance != this*/)
        {
            InstanceGUI = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {

            Destroy(gameObject);
        }
    }
    void Start()
    {
        
        icono.gameObject.SetActive(false);
        puntosCalificacion = puntos;
        ballonDialogue.gameObject.SetActive(false);
        obAnim = GameObject.Find("Cortina").GetComponent<Animator>();
        HUDLienzos[0].SetActive(true);
        HUDLienzos[1].SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        DialogueFadeIn();
    }
}