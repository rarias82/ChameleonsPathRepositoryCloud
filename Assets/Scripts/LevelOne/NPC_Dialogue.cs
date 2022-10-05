using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
public enum ModeNPC
{
    Help, Busy, Walking, House, Final, Follow
}
public class NPC_Dialogue : MonoBehaviour
{
    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] lines;
    public float speedText;
    public int index;
    public bool didDialogueStart;
    [SerializeField] HouseDialogue houses;
    [SerializeField] NPC_Henry henry;

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
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;

    [Header("Anim References")]
    public Animator obAnim;
    public int numeroAnim;

    
    




    public void StartDialogue()
    {
        
        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;


        MainCharacter.sharedInstance.intervalo = 0.0f;

        id_selector = 1;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.instance.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.instance.obMap.SetActive(false);
        UIManager.instance.obMapMark.SetActive(false);

        if (houses.optionCBuscarHermano)
        {
            lines = obRoute.linesNextC2;

            houses.obHenry.SetActive(true);
            //henry.Aparecer();

            houses.didDialogueStart = false;

            
        }

        StartCoroutine(WriteDialogue());

    }


    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.instance.PosicionarGlobo(trPlayer.position);
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.instance.PosicionarGlobo(transform.position);
        }

        if (lineas.Trim().StartsWith("H"))
        {

        }

        
    }

    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - trPlayer.transform.position;
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom/2f) * Time.deltaTime);

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;
                
                yield return null;

            }

            UIManager.instance.ballonDialogue.gameObject.SetActive(true);
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

        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
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

        if ((Input.GetButtonDown("Abajo") || Input.GetAxis("Vertical") == -1f) && id_selector < listOptions.Length - 1)
        {
            id_selector++;
        }

        if ((Input.GetButtonDown("Arriba") || Input.GetAxis("Vertical") == 1f) && id_selector > 0)
        {
            id_selector--;
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;
        

        selector.transform.SetSiblingIndex(0);


        if (Input.GetButtonDown("Interactuar"))
        {
            index = 0;

            switch (id_selector)
            {
                case 0:
                    obRoute.StarRoute(id_selector);
                    UIManager.instance.GanarPuntos(true, UIManager.instance.puntos);
                    

                    break;

                case 1:
                    obRoute.StarRoute(id_selector);
                    UIManager.instance.GanarPuntos(false, UIManager.instance.puntos);
                  

                    break;

                case 2:
                    obRoute.StarRoute(id_selector);
                    UIManager.instance.GanarPuntos(false, UIManager.instance.puntos);
                  
                    break;

                default:
                    break;
            }

            nextDialogueToTalk = id_selector;
        }


    }

    public IEnumerator CloseDialogue()
    {

        UIManager.instance.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.instance.fadeFrom = true;

        while (obCameras.orthographicSize < 7.5f)
        {
            obCameras.orthographicSize += speedZoom * Time.deltaTime;
            yield return null;

        }

        Inventory.instance.panelItem.SetActive(true);
        UIManager.instance.obMap.SetActive(true);
        UIManager.instance.obMapMark.SetActive(true);

        marker.SetActive(true);

        didDialogueStart = false;

        MainCharacter.sharedInstance.canMove = true;

        nextRoute = false;


        
        switch (nextDialogueToTalk)
        {
            case 0:
                lines = obRoute.linesNextA;
                changeInitialDialogue = true;
                mode = ModeNPC.Help;
                
                houses.talkToLeahn = true; 
                break;

            case 1:
                lines = obRoute.linesNextB;
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

        if (houses.optionCBuscarHermano)
        {

        }
        
        //changeInitialDialogue = true;


    }

    public IEnumerator CloseDialogueC()
    {

        UIManager.instance.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.instance.fadeFrom = true;

        while (obCameras.orthographicSize < 7.5f)
        {
            obCameras.orthographicSize += speedZoom * Time.deltaTime;
            yield return null;

        }

        Inventory.instance.panelItem.SetActive(true);
        UIManager.instance.obMap.SetActive(true);
        UIManager.instance.obMapMark.SetActive(true);

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

        if (diferenciaVector.sqrMagnitude < (henry.distancia * 2f))
        {

            
            Debug.Log("Llego");


        }
        else
        {
            Debug.Log("No llego");
            obNMA.SetDestination(trPlayer.position);
        }



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
            
            Navegate();
        }

        if (mode == ModeNPC.Help || mode == ModeNPC.Busy)
        {
            if (mode == ModeNPC.Help)
            {
                Vector3 direction = trPlayer.transform.position - transform.position;
                transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
            }

            Interactuar();
        }

        if (mode == ModeNPC.Follow)
        {
            Following();
        }


    }

    public void Interactuar()
    {
        if (isRange && Input.GetButtonDown("Interactuar") && Inventory.instance.moverInv && !henry.tiempoesperando)
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

            UIManager.instance.icono.gameObject.SetActive(false);


        }

        RotateSon();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (mode == ModeNPC.Walking || mode == ModeNPC.Busy)
        {
            if (other.gameObject.CompareTag("P1"))
            {

               
                    mode = ModeNPC.Help;
                    obNMA.speed = 0;

                    isRange = !isRange;

                if (henry.tiempoesperando)
                {
                    marker.SetActive(false);
                }
                else
                {
                    marker.SetActive(true);
                }


				numeroAnim = 0;

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

                if (lines == obRoute.linesNextA)
                {
                    
                    mode = ModeNPC.Busy;
                    obNMA.speed = 0;
                    isRange = !isRange;
                    marker.SetActive(false);

					numeroAnim = 0;

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
