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
    [SerializeField, TextArea(4, 6)] string[] lines;
    public float speedText;
    public int index;
    public bool didDialogueStart;
    [SerializeField] HouseDialogue houses;
    [SerializeField] NPC_Henry henry;
    public GameObject detector;

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

    [Header("Music References")]
    [SerializeField] AudioClip cancion;
    public Mapa _map;
 

    public void SetInputActions(Mapa map)
    {
        _map = map;

    }

    private void Awake()
    {
        instance = this;

        
    }

  
    void CambiarDialogos() {

        switch (nextDialogueToTalk)
        {
            case 0:

                random00 = Random.Range(0, 5);



                while (random01 == random00)
                {
                    random00 = Random.Range(0, 5);
                }


                random01 = random00;

                if (random00 == 0)
                {
                    lines = obRoute.linesNextA;
                }
                if (random00 == 1)
                {
                    lines = obRoute.linesNextA1;
                }
                if (random00 == 2)
                {
                    lines = obRoute.linesNextA2;
                }
                if (random00 == 3)
                {
                    lines = obRoute.linesNextA3;
                }
                if (random00 == 4)
                {
                    lines = obRoute.linesNextA4;
                }


                changeInitialDialogue = true;
                mode = ModeNPC.Help;

                houses.talkToLeahn = true;
                break;

            case 1:

                random00 = Random.Range(0, 5);

                while (random01 == random00)
                {
                    random00 = Random.Range(0, 5);
                }

                random01 = random00;

                
                if (random00 == 0)
                {
                    lines = obRoute.linesNextB;
                }
                if (random00 == 1)
                {
                    lines = obRoute.linesNextB1;
                }
                if (random00 == 2)
                {
                    lines = obRoute.linesNextB2;
                }
                if (random00 == 3)
                {
                    lines = obRoute.linesNextB3;
                }
                if (random00 == 4)
                {
                    lines = obRoute.linesNextB4;
                }


                changeInitialDialogue = false;
                numeroAnim = 0;
                break;

            case 2:
                lines = obRoute.linesNextC;
                changeInitialDialogue = true;
                //mode = ModeNPC.Help;
                houses.talkToLeahn = true;
                numeroAnim = 0;
                break;

            default:
                break;
        }






    }
    public void StartDialogue()
    {



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
 

        if (houses.optionCBuscarHermano)
        {
            lines = obRoute.linesNextC2;

            houses.obHenry.SetActive(true);
            //henry.Aparecer();

            houses.didDialogueStart = false;

            
        }

        FollowCameras.instance.mode = Modo.InDialogue;
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
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
        }

        if (lineas.Trim().StartsWith("H"))
        {

        }

        
    }
    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));

            if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
            {
                numeroAnim = 17;
            }

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z); 
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }
            detector.SetActive(true);

        }

        while (FollowCameras.instance.mode == Modo.InDialogue)
        {
            yield return null;
        }

        if (index == 1)
		{
            numeroAnim = 2;

        }

        if (index == 3 )
        {
			numeroAnim = 3;
		}

        if (index == 4)
        {
			numeroAnim = 4;
		}


        dialogueText.text = string.Empty;


        IconDialogo(lines[index]);

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
        }

        UIManager.InstanceGUI.icono.gameObject.SetActive(true);



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
                nextRoute = true;

                Options.SetActive(true);

                for (int i = 0; i < listOptions.Length; i++)
                {
                    listOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLines[i];
                }
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

    public void Navegate()
    {

        if ((_map.Opciones.Navegar.ReadValue<Vector2>().y == -1.0f) && id_selector < listOptions.Length - 1)
        {
            id_selector++;
        }

        if ((_map.Opciones.Navegar.ReadValue<Vector2>().y == 1.0f) && id_selector > 0)
        {
            id_selector--;
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (_map.Jugador.Interactuar.WasPressedThisFrame())
        {
            index = 0;

            switch (id_selector)
            {
                case 0:
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);


                    break;

                case 1:
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);


                    break;

                case 2:
                    obRoute.StarRoute(id_selector);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);

                    break;

                default:
                    break;
            }

            nextDialogueToTalk = id_selector;
        }


    }
    public IEnumerator CloseDialogue()
    {

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));


        index = 0;

        CambiarDialogos();


        if (lines == obRoute.linesNextA || lines == obRoute.linesNextA1 || lines == obRoute.linesNextA2 || lines == obRoute.linesNextA3 || lines == obRoute.linesNextA4)
        {
            numeroAnim = 16;
        }

        FollowCameras.instance.mode = Modo.InGame;


        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        while ((obCameras.orthographicSize < 7.5f) && offset != new Vector3(-15.00f, 12.5f, -15.00f))
        {
            obCameras.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;
            FollowCameras.instance.offset = Vector3.Slerp(FollowCameras.instance.offset, new Vector3(-15.00f, 12.5f, -15.00f), (speedZoom / 2.0f) * Time.deltaTime);
            yield return null;

        }


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

        marker.SetActive(true);

        

        nextRoute = false;


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);


       

        MainCharacter.sharedInstance.canMove = true;

       

        

        didDialogueStart = false;
    }
    public IEnumerator CloseDialogueC()
    {

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        while (obCameras.orthographicSize < 7.5f)
        {
            obCameras.orthographicSize += speedZoom * Time.deltaTime;
            yield return null;

        }

        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

        marker.SetActive(true);

        didDialogueStart = true;

        MainCharacter.sharedInstance.canMove = true;

        marker.SetActive(false);


        mode = ModeNPC.Follow;

		numeroAnim = 0;
        

    }
    public void MedirDistancia()
    {
        diferenciaVector = trPlayer.position - transform.position;
    }
    void Following()
    {

        MedirDistancia();

        //if (diferenciaVector.sqrMagnitude < (henry.distancia * 2f))
        //{

            
        //    Debug.Log("Llego");


        //}
        //else
        //{
        //    Debug.Log("No llego");
        //    obNMA.SetDestination(trPlayer.position);
        //}



    }
    void Start()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;
		numeroAnim = 1;
        detector.SetActive(false);




        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        //Options = GameObject.Find("DialogueOptions").gameObject;

        //selector = GameObject.Find("Select").gameObject;

        //listOptions[0] = GameObject.Find("Panel0").gameObject;
        //listOptions[1] = GameObject.Find("Panel1").gameObject;
        //listOptions[2] = GameObject.Find("Panel2").gameObject;


        //houses = FindObjectOfType<HouseDialogue>();
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

        if (Options.activeInHierarchy && !houses.didDialogueStart)
        {
            
            //Navegate();
        }

        if ((mode == ModeNPC.Help || mode == ModeNPC.Busy))
        {


            if (!changeInitialDialogue)
            {
                Vector3 direction = trPlayer.transform.position - transform.position;
                transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
            }
                
           

            InteractuarSS();
        }

        if (mode == ModeNPC.Follow)
        {
            Following();
        }

       
    }
    public void InteractuarSS()
    {
        if (isRange && _map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv)
        {
            if (!didDialogueStart)
            {
                StartDialogue();


            }



            else if (!Options.activeInHierarchy && !nextRoute)
            {
                if (dialogueText.text == lines[index].Substring(1))
                {
                    NextDialogue();

                }

            }

            UIManager.InstanceGUI.icono.gameObject.SetActive(false);


        }

        RotateSon();
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

                    marker.SetActive(true);

                    numeroAnim = 17;
                }
                else
                {

                    mode = ModeNPC.Help;
                    obNMA.speed = 0;
                    isRange = !isRange;

                    marker.SetActive(true);

                    numeroAnim = 0;
                }

				RotateSon();         

            }

            
        }

        if (mode == ModeNPC.Follow)
        {
            if (other.gameObject.CompareTag("P1"))
            {
                obNMA.speed = 0;
				numeroAnim = 0;
				isRange = true;
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (mode == ModeNPC.Help || mode == ModeNPC.Busy)
        {
            if (other.gameObject.CompareTag("P1"))
            {

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
                obNMA.speed = speedNPC;
				numeroAnim = 1;

				isRange = false;
            }

        }

    }

    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);
     
      
    }
}
