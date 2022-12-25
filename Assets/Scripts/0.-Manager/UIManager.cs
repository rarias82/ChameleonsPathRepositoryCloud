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
    [SerializeField] TextMeshProUGUI dialogos;
    [SerializeField] TextMeshProUGUI dialogosNombre;
    public Image blackScreen, blackScreenN;
    public bool fadeBlack, fadeBlackN;
    public bool fadeFrom, fadeFromN;
    public float fadeSpeed;
    public Image icono;
    public Image ballonDialogue;
    public float valor;
    

    [Header("Bondad Variables")]
    public Image barraBondad;
    public float velocidadBarra;
    
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
    public GameObject lienzoControlesMenu;
    public GameObject lienzoCanvas;

    [Header("Temblor")]
    [SerializeField] float duracionTemblor = 1.0f;
    [SerializeField] AnimationCurve curvas;

    [Header("FPS")]
    int limiteDeFrames = 250;

    [Header("Burubuja")]
    public Animator animBurbuja;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI cuentaTexto;

    public GameObject manto;
    public Scene currentScene;
    public string sceneName;

    [Header("Pausas")]
    public bool encenderTecla;
    public bool isPaused;
    public GameObject ObPausas;
    public bool juegoPausas;
    public GameObject obCartelPausa;
    public bool estaPausados;

    public sbyte id_selectorP;
    public GameObject[] listOptionsP;
    public GameObject selectorP;

    [Header("Comics")]
    public Image[] listaVinetas1;
    [TextArea(4, 6)] public string comicTextos;
    public string comicTextos2;
   
    public TextMeshProUGUI textoComics;
    public GameObject paletaComicsFinal;
    public GameObject colita, colita2;

    [Header("Icrementador")]
    public int etapa;

    [Header("Game Over1")]
    public sbyte id_selectorGO;
    public GameObject[] listOptionsGO;
    public GameObject ObGameOver;
    public bool isGameOver;
    public GameObject selectorGO;



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
    public void DialogueFadeInNames()
    {
        if (fadeBlackN)
        {
            blackScreenN.color = new Color(blackScreenN.color.r, blackScreenN.color.g, blackScreenN.color.b, Mathf.MoveTowards(blackScreenN.color.a, 0.75f, fadeSpeed * Time.deltaTime));

            if (blackScreenN.color.a == 0.75f)
            {
                fadeBlackN = false;

            }
        }

        if (fadeFromN)
        {
            blackScreenN.color = new Color(blackScreenN.color.r, blackScreenN.color.g, blackScreenN.color.b, Mathf.MoveTowards(blackScreenN.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreenN.color.a == 0f)
            {
                fadeFromN = false;

            }
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
            //else
            //{
            //    FinDelJuego();
            //}


            

        }

        if (puntosCalificacion == -200)
        {   
            
            
            isGameOver = true;
            
            
            
            
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

        yield return new WaitForSeconds(timeTransicion);

        SceneManager.LoadScene(sceneIndex);
    }

    public void AnimateOptions(bool activar)
    {
        if (activar)
        {
            obAnimOptionsGame.SetInteger("Show", 1);

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
            FindObjectOfType<NPC_Rana>().carrera.SetActive(false);
            FindObjectOfType<NPC_Rana>().obstaculos.SetActive(false);

        }

        FindObjectOfType<NPC_Rana>().gameObject.SetActive(false);




        yield return new WaitForSeconds(1f);


        MainCharacter.sharedInstance.canMove = true;
        MainCharacter.sharedInstance._map.Jugador.Enable();


    }
    public IEnumerator FinalBRana()
    {
        FindObjectOfType<NPC_Rana>().gameObject.GetComponent<NavMeshAgent>().enabled = false;
        FindObjectOfType<NPC_Rana>().carrera.SetActive(true);
        FindObjectOfType<NPC_Rana>().obstaculos.SetActive(true);



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

        while (tiempoTranscurrido < duracionTemblor && !UIManager.InstanceGUI.isGameOver)
        {
            tiempoTranscurrido += Time.deltaTime;
            fuerza = curvas.Evaluate(tiempoTranscurrido / duracionTemblor);
            FollowCameras.instance.transform.position = posInicial + (Random.insideUnitSphere * fuerza);
            yield return null;
        }

        FollowCameras.instance.transform.position = posInicial;

    }

    public void BurbujaDialogo(float indice)
    {

        if (animBurbuja.gameObject.activeSelf)
        {
            animBurbuja.SetFloat("Indicador", indice);
        }
       

    }

    public void NombreDialogo(string letraInicial)
    {

        if (letraInicial == "P")
        {
            dialogosNombre.text = "Logan";
        }
        if (letraInicial == "L")
        {
            dialogosNombre.text = "Leahn";
        }
        if (letraInicial == "H")
        {
            dialogosNombre.text = "Henry";
        }
        if (letraInicial == "R")
        {
            dialogosNombre.text = "Rana";
        }


    }

    public void EmptyNames()
    {
        dialogosNombre.text = string.Empty;
    }

    public void FinDelJuego()
    {
        dialogos.text = string.Empty;
        ObGameOver.SetActive(true);
        
    }

    public void MostrarCartelPausa()
    {
        estaPausados = !estaPausados;

        if (estaPausados)
        {
            Time.timeScale = 0.0f;
            obCartelPausa.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            obCartelPausa.SetActive(false);
        }
    }

    void NavegarGO()
    {

        //if (MainCharacter.sharedInstance._map.Jugador.BDOWN.WasPressedThisFrame() && id_selectorGO < listOptionsGO.Length - 1)
        //{
        //    id_selectorGO++;
        //    AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);

        //}

        //if (MainCharacter.sharedInstance._map.Jugador.BUP.WasPressedThisFrame() && id_selectorGO > 0)
        //{
        //    id_selectorGO--; ;
        //    AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        //}

        selectorGO.transform.SetParent(listOptionsGO[id_selectorGO].transform);
        selectorGO.transform.position = listOptionsGO[id_selectorGO].transform.position;


        selectorGO.transform.SetSiblingIndex(0);



        if (MainCharacter.sharedInstance._map.Jugador.Interactuar.WasPressedThisFrame())
        {

            switch (id_selectorGO)
            {
                case 0:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
                    MainCharacter.sharedInstance._map.Jugador.Disable();
                    StartCoroutine(Reiniciar());


                    break;

                //case 1:
                //    AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
                //    MainCharacter.sharedInstance._map.Jugador.Disable();
                //    StartCoroutine(VolverMenu());

                    //break;
            }

        }

    }

    void NavegarP()
    {

        if (MainCharacter.sharedInstance._map.Jugador.BDOWN.WasPressedThisFrame() && id_selectorP < listOptionsP.Length - 1)
        {
            id_selectorP++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);

        }

        if (MainCharacter.sharedInstance._map.Jugador.BUP.WasPressedThisFrame() && id_selectorP > 0)
        {
            id_selectorP--; ;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        selectorP.transform.SetParent(listOptionsP[id_selectorP].transform);
        selectorP.transform.position = listOptionsP[id_selectorP].transform.position;


        selectorP.transform.SetSiblingIndex(0);



        if (MainCharacter.sharedInstance._map.Jugador.Interactuar.WasPressedThisFrame())
        {

            switch (id_selectorP)
            {
                case 0:
                    obCartelPausa.SetActive(false);
                    Time.timeScale = 1.0f;
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
                    MainCharacter.sharedInstance._map.Jugador.Disable();
                    StartCoroutine(Reiniciar());


                    break;

                case 1:
                    //obCartelPausa.SetActive(false);
                    //Time.timeScale = 1.0f;
                    //AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
                    //MainCharacter.sharedInstance._map.Jugador.Disable();
                    //StartCoroutine(VolverMenu());

                    ExitPlayGame();

                    break;
            }

        }

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
        HUDLienzos[0].SetActive(false);








    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ComenzarEscena());
    }


    public IEnumerator Reiniciar()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0); fadeBlack = false; fadeFrom = false; dialogos.text = string.Empty;
 

        

        //Inventory.instance.slot[1].GetComponent<Slot>().Quitar();
        

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));



        rtCircle.transform.position = originalPoseCircle;

        ObGameOver.SetActive(false);


        int nextScene = SceneManager.GetActiveScene().buildIndex;


        yield return null;

        Time.timeScale = 1.0f;

        StartCoroutine(SceneLoading(nextScene));



    }
    public IEnumerator VolverMenu()
    {

        HUDLienzos[0].SetActive(false);
        HUDLienzos[1].SetActive(false);

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0); fadeBlack = false; fadeFrom = false; dialogos.text = string.Empty;


       

        //Inventory.instance.slot[1].GetComponent<Slot>().Quitar();
        

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionMenu));



        rtCircle.transform.position = originalPoseCircle;

        ObGameOver.SetActive(false);


        int nextScene = SceneManager.GetActiveScene().buildIndex;


        yield return null;

        Time.timeScale = 1.0f;

        StartCoroutine(SceneLoading(nextScene-2));



    }

    IEnumerator ComenzarEscena()
    {
        Debug.Log("CargarEscena");
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        etapa++;

        if (sceneName == "MenuStart")
        {


            

            lienzoControlesMenu = GameObject.FindGameObjectWithTag("Controles");
            UIManager.InstanceGUI.lienzoControlesMenu.SetActive(true);
            lienzoControlesMenu.transform.SetParent(lienzoCanvas.transform);
            lienzoControlesMenu.transform.SetAsFirstSibling();

            UIManager.InstanceGUI.paletaComicsFinal.SetActive(false);

            dialogos.text = string.Empty;
            //HUDLienzos[0].SetActive(false);
            HUDLienzos[0].SetActive(false);
            HUDLienzos[1].SetActive(false);

            obCartelPausa.SetActive(false);

            encenderTecla = false;
            juegoPausas = false;

            ObPausas.SetActive(false);
            icono.gameObject.SetActive(false);
            puntosCalificacion = puntos;
            ballonDialogue.gameObject.SetActive(false);
            ForceScalesAndLimitFPS();

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionMenu));


            isGameOver = false;
        }

        if (sceneName == "Comic")
        {
            UIManager.InstanceGUI.lienzoControlesMenu.SetActive(false);

            colita.SetActive(true);
            colita2.SetActive(true);
            UIManager.InstanceGUI.paletaComicsFinal.SetActive(false);
          
            dialogos.text = string.Empty;
            //HUDLienzos[1].SetActive(false);
            HUDLienzos[0].SetActive(false);
            HUDLienzos[1].SetActive(true);

            obCartelPausa.SetActive(false);

            LimpiarCOmic();

            isGameOver = false;
        }

        if (sceneName == "LevelOne")
        {

            QuitarHUDInGame();
            QuitarHUDInGame();


            obCartelPausa.SetActive(false);

            UIManager.InstanceGUI.paletaComicsFinal.SetActive(false);

            dialogos.text = string.Empty;
            GameManager.InstancieInput.ActivarInput();

            HUDLienzos[0].SetActive(true);
            HUDLienzos[1].SetActive(false);
            //HUDLienzos[2].SetActive(false);


            manto.SetActive(false);
            
            
            

            encenderTecla = true;
            juegoPausas = true;

            originalPoseCircle = rtCircle.transform.position;
            ObPausas.SetActive(false);
            icono.gameObject.SetActive(false);
            puntosCalificacion = puntos;
            ballonDialogue.gameObject.SetActive(false);
            ForceScalesAndLimitFPS();



            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));

            isGameOver = false;

            Inventory.instance.slot[1].GetComponent<Slot>().Quitar();

        }

      


        yield return null;



    

    }

    public void QuitarHUDInGame()
    {

        Inventory.instance.panelItem.SetActive(false);

        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);

        dialogos.text = string.Empty;
        dialogosNombre.text = string.Empty;

    }

    public void ShowHUDInGame()
    {

        Inventory.instance.panelItem.SetActive(true);

        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

    }


    public void LimpiarCOmic()
    {
        foreach (Image miniImagen in listaVinetas1)
        {
            miniImagen.color = new Color(miniImagen.color.r, miniImagen.color.g, miniImagen.color.b, 0.0f);
        }

    }
    void Update()
    {
       

        DialogueFadeIn();
        DialogueFadeInNames();

        if (ObGameOver.activeInHierarchy && isGameOver)
        {
            NavegarGO();
        }

        if (obCartelPausa.activeInHierarchy)
        {
            NavegarP();
        }

    }


}