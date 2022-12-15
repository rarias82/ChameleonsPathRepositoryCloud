using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public enum ModeNPC
{
    Help, Busy, Walking, House, Final, Follow
}
public class NPC_Dialogue : MonoBehaviour
{
    public static NPC_Dialogue instance;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [TextArea(4, 6)] public string[] lines;
    public float speedText;
    public int index;
    public bool didDialogueStart;
    public HouseDialogue houses;
    public NPC_Henry henry;
    public GameObject detector;
    public bool seguiraLogan;

    [Header("Options References")]
    public GameObject Options;
    [SerializeField] GameObject[] listOptions;
    [SerializeField] GameObject selector;
    [SerializeField] sbyte id_selector;
    [SerializeField] sbyte indexQuesion;
    [SerializeField] string[] optionLines;
    public bool nextRoute;
    public NPC_Follow obRoute;
    public sbyte nextDialogueToTalk;
    public bool changeInitialDialogue;
    public int random00;
    public int random01;
    public float valor;
    public bool habloconLeahn;

    [Header("Move Variables")]
    public sbyte numeroCamino;
    NavMeshAgent obNMA;
    [SerializeField] Vector3[] trCaminos;
    Vector3 diferenciaVector;
    public float speedNPC;

    [Header("Mode HUD Variables")]
    public ModeNPC mode;
    [SerializeField] Vector3 offset;
    [SerializeField] string nameNPC;
    GameObject marker;
    public bool isRange;
    public Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;

    [Header("Anim References")]
    public Animator obAnim;
    public int numeroAnim;
    public float distancia;
    Color lineColor;
    bool detectarLimites;
   
    [SerializeField, TextArea(4, 6)] string[] linesLogan0;
    [SerializeField, TextArea(4, 6)] string[] linesLogan1;
    [SerializeField, TextArea(4, 6)] string[] linesLogan2;
    [SerializeField, TextArea(4, 6)] string[] linesLogan3;

    [Header("Music References")]
    [SerializeField] AudioClip cancion;
    public Mapa _map;
    bool subir;
    bool desactivaralgo;
    float biblio;
    public bool logan;
    bool mirar;
    public NPC_Rana rana;
    [SerializeField] AudioClip[] voces;
    int voz000, voz001;
    [SerializeField] NPC_Henry enrique;
    public Vector3 posOriginal;
    

    [Header("Efectos")]
    [SerializeField] ParticleSystem polvoTierra;
    bool noAbrir;
    public void SetInputActions(Mapa map)
    {
        _map = map;

    }
    public void VocesRandom()
    {
        voz000 = voz001;

        voz001 = Random.Range(0, voces.Length);


        while (voz001 == voz000)
        {
            voz001 = Random.Range(0, voces.Length);
        }

        AudioManager.Instance.PlaySound(voces[voz001]);

    }
    private void Awake()
    {
        instance = this;

        
    }
    private void OnEnable()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;
        numeroAnim = 1;
        detector.SetActive(false);
        polvoTierra.Stop();


    }
    void Start()
    {

            dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

            Options = GameObject.Find("DialogueOptions").gameObject;

            selector = GameObject.Find("Select").gameObject;

            listOptions[0] = GameObject.Find("Panel0").gameObject;
            listOptions[1] = GameObject.Find("Panel1").gameObject;
            listOptions[2] = GameObject.Find("Panel2").gameObject;
        





        //houses = FindObjectOfType<HouseDialogue>();
    }
 
    void CambiarDialogos() {

        switch (nextDialogueToTalk)
        {
            case 0:


                random00 = random01;
;
                random01 = Random.Range(0, 5);

                
                while (random01 == random00)
                {
                    random01 = Random.Range(0, 5);
                }


               

                if (random01 == 0)
                {
                    lines = obRoute.linesNextA;
                }
                if (random01 == 1)
                {
                    lines = obRoute.linesNextA1;
                }
                if (random01 == 2)
                {
                    lines = obRoute.linesNextA2;
                }
                if (random01 == 3)
                {
                    lines = obRoute.linesNextA3;
                }
                if (random01 == 4)
                {
                    lines = obRoute.linesNextA4;
                }


                changeInitialDialogue = true;
                mode = ModeNPC.Help;

                houses.talkToLeahn = true;
                break;

            case 1:

                random00 = random01;
                
                random01 = Random.Range(0, 5);


                while (random01 == random00)
                {
                    random01 = Random.Range(0, 5);
                }




                if (random01 == 0)
                {
                    lines = obRoute.linesNextB;
                }
                if (random01 == 1)
                {
                    lines = obRoute.linesNextB1;
                }
                if (random01 == 2)
                {
                    lines = obRoute.linesNextB2;
                }
                if (random01 == 3)
                {
                    lines = obRoute.linesNextB3;
                }
                if (random01 == 4)
                {
                    lines = obRoute.linesNextB4;
                }


                changeInitialDialogue = false;
                numeroAnim = 0;
                break;

            case 2:

                random00 = random01;
                
                random01 = Random.Range(0, 5);


                while (random01 == random00)
                {
                    random01 = Random.Range(0, 5);
                }




                if (random01 == 0)
                {
                    lines = obRoute.linesNextC0;
                }
                if (random01 == 1)
                {
                    lines = obRoute.linesNextC01;
                }
                if (random01 == 2)
                {
                    lines = obRoute.linesNextC02;
                }
                if (random01 == 3)
                {
                    lines = obRoute.linesNextC03;
                }
                if (random01 == 4)
                {
                    lines = obRoute.linesNextC04;
                }
        


               
                changeInitialDialogue = true;
                mode = ModeNPC.Help;
                houses.talkToLeahn = true;
                numeroAnim = 0;
               
                break;

            default:
                break;
        }






    }
    public void StartDialogue()
    {

        if (detectarLimites)
        {
            
        }
        else
        {

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));

        }

        MainCharacter.sharedInstance.animIntervalo = 0.0f;


        didDialogueStart = true;
        index = 0;

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        id_selector = 1;

        MainCharacter.sharedInstance.canMove = false;      

        UIManager.InstanceGUI.fadeBlack = true;

        marker.SetActive(false);
   

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);
 

        if (houses.optionCBuscarHermano && !seguiraLogan)
        {
             // antes estab en closedialoguec

            lines = obRoute.linesNextC2;

            houses.obHenry.SetActive(true);
            //NPC_Henry.instance.mode = ModeNPCHenry.Final;
            NPC_Henry.instance.mode = ModeNPCHenry.FinalC;
            //henry.Aparecer();

            houses.didDialogueStart = false;

   
           UIManager.InstanceGUI.BurbujaDialogo(7);

        }

        if (seguiraLogan)
        {
            random00 = random01;
            
            random01 = Random.Range(0, 4);


            while (random01 == random00)
            {
                random01 = Random.Range(0, 4);
            }

            if (random01 == 0)
            {
                lines = obRoute.linesFinalC;
            }
            if (random01 == 1)
            {
                lines = obRoute.linesFinalC1;
            }
            if (random01 == 2)
            {
                lines = obRoute.linesFinalC2;
            }
            if (random01 == 3)
            {
                lines = obRoute.linesFinalC3;
            }
            


           
        }


        index = 0;

        StartCoroutine(WriteDialogue());

    }
    public Vector3 ScreenDisplayPont(Vector3 posicionar)
    {
        Vector3 posDisplay = FollowCameras.instance.MyCameras.WorldToScreenPoint(posicionar);
        return posDisplay;



    }
    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position);
            MainCharacter.sharedInstance.VozLogan();
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
            VocesRandom();
        }

        if (lineas.Trim().StartsWith("H"))
        {

        }

        
    }
    public IEnumerator WriteDialogue()
    {

        marker.SetActive(false);

        if (index == 0)
        {
            UIManager.InstanceGUI.BurbujaDialogo(0);

            if (detectarLimites)
            {

                UIManager.InstanceGUI.BurbujaDialogo(2);
                MainCharacter.sharedInstance.eAnim = 2;
                MainCharacter.sharedInstance.animIntervalo = 0.0f;

                AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
            }
            else
            {
                //detector.SetActive(true);

                FollowCameras.instance.velocidadRotacion = -75.0f;
                FollowCameras.instance.mode = Modo.Mundo;
                       
            }
            
            if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
            {
                numeroAnim = 17;
                
            }

            if (lines == obRoute.linesNextB || lines == obRoute.linesNextB1 || lines == obRoute.linesNextB2 || lines == obRoute.linesNextB3 || lines == obRoute.linesNextB4)
            {
                MainCharacter.sharedInstance.eAnim = 21;

                if (index == 0)
                {
                    numeroAnim = 2;
                }
                if (index == 1)
                {
                    numeroAnim = 3;
                }
                if (index == 2)
                {
                    numeroAnim = 4;
                }
            }

            if (lines == obRoute.linesNextC0 || lines == obRoute.linesNextC01 || lines == obRoute.linesNextC02 || lines == obRoute.linesNextC03 || lines == obRoute.linesNextC04)
            {
                MainCharacter.sharedInstance.eAnim = 20;

                if (index == 0)
                {
                    numeroAnim = 2;
                }
                if (index == 1)
                {
                    numeroAnim = 3;
                }
                if (index == 2)
                {
                    numeroAnim = 4;
                }
            }

            if (lines == obRoute.linesNextC2)
            {
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 20;
                }

                if (index == 1)
                {
                    numeroAnim = 2;
                }

                if (index == 2)
                {
                    MainCharacter.sharedInstance.eAnim = 21;
                }

                if (index == 3)
                {
                    numeroAnim = 3;
                }
            }
            
            posOriginal = FollowCameras.instance.transform.position;

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z); 
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;
            }

            if (!detectarLimites)
            detector.SetActive(true);

        }

        while (FollowCameras.instance.mode == Modo.Mundo)
        {
            yield return null;
        }

        if (index == 0)
        {
            
            if (!habloconLeahn)
            {
                MainCharacter.sharedInstance.eAnim = 20;
            }
        }

        if (index == 2)
        {


            if (!habloconLeahn)
            {
                MainCharacter.sharedInstance.eAnim = 21;
            }
            
        }

        if (index == 1)
		{
            if (!habloconLeahn)
            {
                numeroAnim = 2;
            }
            

        }

        if (index == 3 )
        {
			numeroAnim = 3;

            if (!habloconLeahn)
            {
                UIManager.InstanceGUI.BurbujaDialogo(1);
            }
           
        }

        if (index == 4)
        {
            
            numeroAnim = 4;


            if (!habloconLeahn)
            {
                UIManager.InstanceGUI.BurbujaDialogo(4);
            }
            
            
        }

        if (habloconLeahn)
        {
            if (index == 1)
            {

                switch (nextDialogueToTalk)
                {
                    case 0:
                        UIManager.InstanceGUI.BurbujaDialogo(5);
                        break;

                    case 1:
                        UIManager.InstanceGUI.BurbujaDialogo(6);
                        break;

                    case 2:
                        UIManager.InstanceGUI.BurbujaDialogo(6);
                        break;

                }

            }
        }
       


        dialogueText.text = string.Empty;


        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

        IconDialogo(lines[index]);


        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
        }

        if (dialogueText.text == lines[index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }

    }
    public void NextDialogue()
    {
        index++;

        if ((index < lines.Length))
        {
            StartCoroutine(WriteDialogue());
        }
        else
        {


            if (!changeInitialDialogue && !houses.optionCBuscarHermano)
            {

                StartCoroutine(Esperar());
                index = 0;
            }
            else
            {
                if (!houses.optionCBuscarHermano)
                {
                    StartCoroutine(CloseDialogue());
                }
                else
                {
                    StartCoroutine(CloseDialogueC());

                }
                
            }



        }

        
    }
    IEnumerator Esperar()
    {
        FollowCameras.instance.OnConfetis(0);

        for (int i = 0; i < listOptions.Length; i++)
        {
            listOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLines[i];
        }

        _map.Disable();
        yield return new WaitForSeconds(1f);
        UIManager.InstanceGUI.AnimateOptions(true);
       
        yield return new WaitForSeconds(1f);
        nextRoute = true;
        

        //Options.SetActive(true);

 
        yield return new WaitForSeconds(0.5f);
        _map.Enable();
    }
    public void Navegate()
    {

        if (_map.Jugador.BDOWN.WasPressedThisFrame() && id_selector < listOptions.Length - 1)
        {
            id_selector++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        if (_map.Jugador.BUP.WasPressedThisFrame() && id_selector > 0)
        {
            id_selector--;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (_map.Jugador.Interactuar.WasPressedThisFrame() && UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 && !UIManager.InstanceGUI.isGameOver)
        {
            index = 0;

            habloconLeahn = true;

            

            switch (id_selector)
            {
                case 0:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectGood);
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);
                    UIManager.InstanceGUI.BurbujaDialogo(7);


                    break;

                case 1:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);
                    UIManager.InstanceGUI.BurbujaDialogo(2);

                    break;

                case 2:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);
                    UIManager.InstanceGUI.BurbujaDialogo(2);
                    break;

                default:
                    break;
            }

            MainCharacter.sharedInstance.eAnim = 22;

            nextDialogueToTalk = id_selector;
        }


    }
    public IEnumerator CloseDialogue()
    {

        detector.SetActive(true);

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));

        index = 0;

        CambiarDialogos();


        if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
        {
            numeroAnim = 16;
            
        }

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        FollowCameras.instance.velocidadRotacion = -75f;
        FollowCameras.instance.mode = Modo.MundoAlreves;

        while (Camera.main.orthographicSize < 7.5f)
        {
            Camera.main.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;

            //FollowCameras.instance.transform.position = Vector3.Lerp(FollowCameras.instance.transform.position, posOriginal.transform.position, (speedZoom / 2.0f) * Time.deltaTime);

            //Vector3 currentAngle = new Vector3(
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.x, posOriginal.transform.rotation.eulerAngles.x, (speedZoom / 2.0f) * Time.deltaTime),
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.y, posOriginal.transform.rotation.eulerAngles.y, (speedZoom / 2.0f) * Time.deltaTime),
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.z, posOriginal.transform.rotation.eulerAngles.z, (speedZoom / 2.0f) * Time.deltaTime)
            //);

            yield return null;

        }

        while (FollowCameras.instance.mode == Modo.MundoAlreves)
        {
            yield return null;
        }

        FollowCameras.instance.mode = Modo.InGame;


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);


        nextRoute = false;


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);



        
        MainCharacter.sharedInstance.eAnim = 0; 

        yield return new WaitForSeconds(0.01f);
        marker.SetActive(true);
        
        didDialogueStart = false;
        MainCharacter.sharedInstance.canMove = true;

        if (UIManager.InstanceGUI.isGameOver)
        {
            UIManager.InstanceGUI.FinDelJuego();
        }

        MainCharacter.sharedInstance.puedePausar = true;

    }
    public IEnumerator CloseDialogueC()
    {
        Debug.Log("Todo es posible");
        rana.gameObject.SetActive(false);
        seguiraLogan = true;

        detector.SetActive(true);

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));

        index = 0;

        //CambiarDialogos();

        numeroAnim = 0;
        MainCharacter.sharedInstance.eAnim = 0;

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        FollowCameras.instance.velocidadRotacion = -75f;
        FollowCameras.instance.mode = Modo.MundoAlreves;

        while (Camera.main.orthographicSize < 7.5f)
        {
            Camera.main.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;


            yield return null;

        }

        while (FollowCameras.instance.mode == Modo.MundoAlreves)
        {
            yield return null;
        }

        FollowCameras.instance.mode = Modo.InGame;


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

       


        nextRoute = false;


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);

        

        yield return new WaitForSeconds(0.01f);
        marker.SetActive(true);

        didDialogueStart = false;
        MainCharacter.sharedInstance.canMove = true;

        mode = ModeNPC.Follow;
        obNMA.speed = 0;

        //if (UIManager.InstanceGUI.isGameOver)
        //{
        //    UIManager.InstanceGUI.FinDelJuego();
        //}

        MainCharacter.sharedInstance.puedePausar = true;
    }
    public IEnumerator CloseDialogueN()
    {
        Debug.Log("closeN");
        index = 0;

        MainCharacter.sharedInstance.eAnim = 0;
        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        FollowCameras.instance.mode = Modo.InGame;

            while ((obCameras.orthographicSize < 7.5f))
            {
                obCameras.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;


                yield return null;

            }
        




        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);




        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);


            detectarLimites = false;


        yield return new WaitForSeconds(0.01f);
        didDialogueStart = false;

            logan = false;

        MainCharacter.sharedInstance.canMove = true;

        if (UIManager.InstanceGUI.isGameOver)
        {
            UIManager.InstanceGUI.FinDelJuego();
        }

        MainCharacter.sharedInstance.puedePausar = true;

    }
    public void MedirDistancia()
    {
        diferenciaVector = trPlayer.position - transform.position;
    }
    void Following()
    {

        diferenciaVector = trPlayer.position - transform.position;


        if (diferenciaVector.sqrMagnitude > (distancia * 2f) && !detectarLimites)
        {


            lineColor = Color.red;
            StartCoroutine("DetectarLimites");



        }
        else
        {
            lineColor = Color.green;

        }

        Debug.DrawRay(transform.position, diferenciaVector, lineColor);

        obNMA.SetDestination(trPlayer.position);

        if (obNMA.speed == 0)
        {
            Vector3 direction = trPlayer.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
        }

    }
    IEnumerator DetectarLimites()
    {

        logan = true;
        detectarLimites = true;
        didDialogueStart = true;

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        MainCharacter.sharedInstance.canMove = false;



        UIManager.InstanceGUI.fadeBlack = true;



        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);



        yield return null;


        random00 = random01;

        random01 = Random.Range(0, 4);


        while (random01 == random00)
        {
            random01 = Random.Range(0, 4);
        }




        switch (random01)
        {
            case 0:
                lines = linesLogan0;
                break;

            case 1:
                lines = linesLogan1;
                break;

            case 2:
                lines = linesLogan2;
                break;

            case 3:
                lines = linesLogan3;
                break;

        }

       


        //mode = ModeNPCHenry.Limites;

        UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);

        StartCoroutine(WriteDialogue());

    }
    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }
    void Walking()
    {
        diferenciaVector = trCaminos[numeroCamino] - transform.position;

        if (diferenciaVector.sqrMagnitude < (0.5f * 2f))
        {
            numeroCamino++;
            
            if (numeroCamino >= trCaminos.Length)
            {
                numeroCamino = 0;
            }
            
        }

        obNMA.SetDestination(trCaminos[numeroCamino]);



    }
    void Update()
    {
        
        if (mode == ModeNPC.Walking)
        {
            Walking();
        }

        if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 && !houses.didDialogueStart && (!rana.didDialogueStart || !rana.gameObject.activeInHierarchy))
        {

            Navegate();
        }

        if ((mode == ModeNPC.Help || mode == ModeNPC.Busy))
        {


            //if (!changeInitialDialogue)
            //{
            //    Vector3 direction = trPlayer.transform.position - transform.position;
            //    transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
            //}
            Vector3 direction = trPlayer.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);


            InteractuarSS();
        }

        if (mode == ModeNPC.Follow)
        {
            Following();
            InteractuarSS();

            if (mirar)
            {
                Vector3 direction = trPlayer.transform.position - transform.position;
                transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
            }
            else
            {

            }

        }

        if (mode == ModeNPC.Final)
        {
            Vector3 directionH = enrique.gameObject.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, directionH, (speedZoom / 2.5f) * Time.deltaTime);
        }


        RotateSon();

      
    }
    public void InteractuarSS()
    {

        if (detectarLimites && dialogueText.text == lines[index].Substring(1) && _map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv && !enrique.seAcabo /*&& noAbrir*/)
        {

            logan = true;
            StartCoroutine(CloseDialogueN());
            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        }//Cuando Logan se aleja

        if (isRange && _map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv && !houses.henryFinalA && !logan && !detectarLimites && !enrique.seAcabo && !UIManager.InstanceGUI.isGameOver)
        {

            UIManager.InstanceGUI.icono.gameObject.SetActive(false);

            if (!didDialogueStart)
            {
                StartDialogue();

            }
            else if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 0 && !nextRoute && !logan && !detectarLimites && !enrique.seAcabo /*&& !houses.didDialogueStart*/)
            {
                if (dialogueText.text == lines[index].Substring(1))
                {
                    NextDialogue();

                }

            }

            

        } // Dialogo Normal

        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (mode == ModeNPC.Walking || mode == ModeNPC.Busy)
        {
            if (other.gameObject.CompareTag("P1") )
            {

                if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
                {
                    mode = ModeNPC.Help;
                    obNMA.speed = 0;
                    isRange = !isRange;
                    if (!houses.henryFinalA)
                    {
                        marker.SetActive(true);
                    }
                    

                    numeroAnim = 17;
                    
                }
                else
                {

                    mode = ModeNPC.Help;
                    obNMA.speed = 0;
                    isRange = !isRange;
                    if (!houses.henryFinalA)
                    {
                        marker.SetActive(true);
                    }

                    numeroAnim = 0;
                    
                }

				RotateSon();         

            }

            
        }

        if (mode == ModeNPC.Follow)
        {
            if (other.gameObject.CompareTag("P1"))
            {
                mirar = true;
                obNMA.speed = 0f;
				numeroAnim = 0;
                
                isRange = true;
                marker.SetActive(true);

                obAnim.speed = 1.0f;

                MainCharacter.sharedInstance.puedePausar = false;
            }

        }

    }
    IEnumerator EsperarCorrer()
    {
        yield return new WaitForSeconds(0.25f);
        polvoTierra.Play();
        numeroAnim = 1;
        obNMA.speed = 4.2f;
    }
    private void OnTriggerExit(Collider other)
    {
        if (mode == ModeNPC.Help || mode == ModeNPC.Busy)
        {
            if (other.gameObject.CompareTag("P1"))
            {

                MainCharacter.sharedInstance.puedePausar = true;

                if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
                {
                    
                    mode = ModeNPC.Busy;
                    obNMA.speed = 0;
                    isRange = !isRange;
                    marker.SetActive(false);

					numeroAnim = 16;

				}
                else
                {
                    mode = ModeNPC.Walking;
                    obNMA.speed = speedNPC;
                    isRange = !isRange;
                    marker.SetActive(false);

					numeroAnim = 1;
				}
                

            }
        }

        if (mode == ModeNPC.Follow)
        {
            if (other.gameObject.CompareTag("P1"))
            {

                MainCharacter.sharedInstance.puedePausar = true;
                marker.SetActive(false);
                isRange = false;
                mirar = false;

                StartCoroutine(EsperarCorrer());
                obAnim.speed = 2.0f;
            }

        }

    }
    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);
     
      
    }
}
