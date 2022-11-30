using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

public class UIManager : MonoBehaviour
{
    public static UIManager InstanceGUI;
   
    [Header("Dialogue Variables")]
    public Image blackScreen;
    public bool fadeBlack;
    public bool fadeFrom;
    public float fadeSpeed;
    public Image icono;
    public Image ballonDialogue;
    public float valor; 


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

    public Vector3 originalPoseCircle;

    [Header("Mapa")]
    public GameObject obMap;
    public GameObject obMapMark;


    [Header("Fade")]
    public Animator obAnim;
    public Animator obAnimOptionsGame;
    public int optionesActivadas;
    public float timeTransicion;

    [Header("Canvas")]
    public GameObject[] HUDLienzos;

    [Header("Temblor")]
    [SerializeField] float duracionTemblor = 1.0f;
    [SerializeField] AnimationCurve curvas;

    [Header("FPS")]
    int limiteDeFrames = 180;

    [Header("Burubuja")]
    public Animator animBurbuja;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI cuentaTexto;
    public GameObject obstaculos, carrera, manto;
    public Scene currentScene;
    public string sceneName;

    [Header("Pausas")]
    public bool encenderTecla;
    public bool isPaused;
    public GameObject ObPausas;
    public bool juegoPausas;

    public GameObject ObGameOver;
    public bool isGameOver;

    [Header("Comics")]
    public Sprite[] vinetas;
    public Image colocarImagen;

    [Header("Icrementador")]
    public int etapa;

    void ForceScalesAndLimitFPS()
    {

        Screen.SetResolution(1280, 720, true);
        Application.targetFrameRate = limiteDeFrames;

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

            FollowCameras.instance.OnConfetis(1);


            if (puntosCalificacion < 400)
            {
                rtCircleV = (rtCircle.position + (Vector3.right * puntos));

                puntosCalificacion += puntos;

                StartCoroutine(MoverBarra(add));
            }
            }
            else
            {

            FollowCameras.instance.OnConfetis(2);
            StartCoroutine(TemblorPantalla());

            if (puntosCalificacion > -200)
            {
                rtCircleV = (rtCircle.position + (Vector3.left * puntos));


                puntosCalificacion -= puntos;

                StartCoroutine(MoverBarra(add));

            }


            if (puntosCalificacion < -200)
            {
                //ObGameOver.SetActive(true);
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
     
        SceneManager.LoadScene(sceneIndex);
    }

    public void AnimateOptions(bool activar)
    {
        if (activar)
        {
            obAnimOptionsGame.SetInteger("Show",1);
            
        }
        else
        {
            obAnimOptionsGame.SetInteger("Show", 0);
            
        }


    }

    public IEnumerator FinalA()
    {

        MainCharacter.sharedInstance._map.Jugador.Disable();
        UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1f);
        NPC_Henry.instance.gameObject.SetActive(false);
        NPC_Dialogue.instance.gameObject.SetActive(false);



        yield return new WaitForSeconds(1f);


        MainCharacter.sharedInstance.canMove = true;
        MainCharacter.sharedInstance._map.Jugador.Enable();


    }

    public IEnumerator FinalARana()
    {

        MainCharacter.sharedInstance._map.Jugador.Disable();
        UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1f);
        if (FindObjectOfType<NPC_Rana>().finalB2)
        {
            carrera.SetActive(false);
            obstaculos.SetActive(false);

        }

        FindObjectOfType<NPC_Rana>().gameObject.SetActive(false);

       


        yield return new WaitForSeconds(1f);


        MainCharacter.sharedInstance.canMove = true;
        MainCharacter.sharedInstance._map.Jugador.Enable();


    }

    public IEnumerator FinalBRana()
    {
        FindObjectOfType<NPC_Rana>().gameObject.GetComponent<NavMeshAgent>().enabled = false;
        carrera.SetActive(true);
        obstaculos.SetActive(true);
        
        

        UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1f);
        
        
        MainCharacter.sharedInstance.transform.position = new Vector3(138.4f, 6.079084f, 132.3f);
        FindObjectOfType<NPC_Rana>().gameObject.transform.position = new Vector3(140f, 6f, 128f);
        FindObjectOfType<NPC_Rana>().numeroAnim = 99;

        yield return new WaitForSeconds(1f);

        manto.SetActive(true);


        for (int i = 1; i < 6; i++)
        {
            yield return new WaitForSeconds(1f);
            cuentaTexto.text = i.ToString("f1");

        }

        cuentaTexto.text = "Ya!!!";
        yield return new WaitForSeconds(0.5f);

        manto.SetActive(false);

        MainCharacter.sharedInstance.canMove = true;
        MainCharacter.sharedInstance._map.Jugador.Enable();

        FindObjectOfType<NPC_Rana>().gameObject.GetComponent<NavMeshAgent>().enabled = true;

      


        FindObjectOfType<NPC_Rana>().mode = ModeNPCRana.Carrera;

        FindObjectOfType<NPC_Rana>().obNMA.speed = 4.0f;
        FindObjectOfType<NPC_Rana>().numeroAnim = 400;


    }

    public void Temblor()
    {
        StartCoroutine(TemblorPantalla());
        
    }

    IEnumerator TemblorPantalla()
    {
        Vector3 posInicial = FollowCameras.instance.transform.position;
        float tiempoTranscurrido = 0.0f;
        float fuerza = 0.0f;

        while (tiempoTranscurrido < duracionTemblor)
        {
            tiempoTranscurrido += Time.deltaTime;
            fuerza = curvas.Evaluate(tiempoTranscurrido/duracionTemblor);
            FollowCameras.instance.transform.position = posInicial + (Random.insideUnitSphere * fuerza);
            yield return null;
        }

        FollowCameras.instance.transform.position = posInicial;

    }

    public void BurbujaDialogo(float indice)
    {
        animBurbuja.SetFloat("Indicador", indice);

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
   
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {









    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ComenzarEscena());
    }

    public void Pausas()
    {
        //if (encenderTecla)
        //{

        //    if (GameManager.InstancieInput.mapeoControles.Jugador.Interactuar.WasPressedThisFrame())
        //    {
        //        isPaused = !isPaused;

        //        if (isPaused)
        //        {
        //            Time.timeScale = 0.0f;
        //        }
        //        else
        //        {
        //            Time.timeScale = 1.0f;
        //        }
        //    }
            
        //    {
            
        //    }
        //}

    }

    public void ShowPausas()
    {
        //if (juegoPausas)
        //{
        //    if (isPaused)
        //    {
        //        ObPausas.SetActive(true);


        //    }
        //    else
        //    {
        //        ObPausas.SetActive(false);
        //    }
        //}


    }

    public IEnumerator Reiniciar()
    {

        //UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

        yield return new WaitForSeconds(1f);

        FindObjectOfType<NPC_Rana>().objetosTodo[0].gameObject.SetActive(false);
        FindObjectOfType<NPC_Rana>().objetosTodo[0].gameObject.SetActive(false);
        Inventory.instance.slot[0].GetComponent<Slot>().Quitar();
        FindObjectOfType<NPC_Rana>().finalA = false;

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));



        rtCircle.transform.position = originalPoseCircle;
        ObGameOver.SetActive(false);

        int nextScene = SceneManager.GetActiveScene().buildIndex;



        StartCoroutine(SceneLoading(nextScene));



    }

    IEnumerator ComenzarEscena()
    {
        Debug.Log("CargarEscena");
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        etapa++;

        if (sceneName == "MenuStart")
        {
            colocarImagen.gameObject.SetActive(false);
            HUDLienzos[0].SetActive(true);
            HUDLienzos[1].SetActive(false);

            encenderTecla = false;
            juegoPausas = false;

            ObPausas.SetActive(false);
            icono.gameObject.SetActive(false);
            puntosCalificacion = puntos;
            ballonDialogue.gameObject.SetActive(false);
            ForceScalesAndLimitFPS();

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionMenu));

        }

        if (sceneName == "Comic")
        {
            colocarImagen.gameObject.SetActive(true);
            HUDLienzos[0].SetActive(false);
            HUDLienzos[1].SetActive(false);
            StartCoroutine(Diapos());
            

        }

        if (sceneName == "LevelOne")
        {
            GameManager.InstancieInput.ActivarInput();

            HUDLienzos[0].SetActive(false);
            HUDLienzos[1].SetActive(true);
            colocarImagen.gameObject.SetActive(false);



            if (etapa == 3)
            {
                carrera = GameObject.Find("LineaCarrera");
                obstaculos = GameObject.Find("Obstaculos");
                

                
            }
            
            manto.SetActive(false);
            
            carrera.SetActive(false);
            
            obstaculos.SetActive(false);
            

            encenderTecla = true;
            juegoPausas = true;

            originalPoseCircle = rtCircle.transform.position;
            ObPausas.SetActive(false);
            icono.gameObject.SetActive(false);
            puntosCalificacion = puntos;
            ballonDialogue.gameObject.SetActive(false);
            ForceScalesAndLimitFPS();



            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));

            yield return new WaitForSeconds(2.75f);

            MainCharacter.sharedInstance._map.Jugador.Enable();

        }

      


        yield return null;



    

    }

    IEnumerator Diapos()
    {
       
        for (int i = 0; i < vinetas.Length; i++)
        {
            colocarImagen.sprite = vinetas[i];

            yield return new WaitForSeconds(5f);
        }

        UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.SceneLoading(2));

    }

    // Update is called once per frame
    void Update()
    {
        Pausas();

        ShowPausas();

        DialogueFadeIn();

      


    }


}