using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCHenry
{
    Iddle, Follow, House, Final, Limites
}

public class NPC_Henry : MonoBehaviour
{

    public static NPC_Henry instance;

    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;
    public bool detectarLimites;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA0;
    [SerializeField, TextArea(4, 6)] string[] linesA1;
    [SerializeField, TextArea(4, 6)] string[] linesA2;
    [SerializeField, TextArea(4, 6)] string[] linesA3;
    [SerializeField, TextArea(4, 6)] string[] linesA4;
    [SerializeField, TextArea(4, 6)] string[] linesA5;
    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string[] linesLogan;

    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;
    int random00;
    int random01;
    public GameObject detector;




    [Header("Move Variables")]
    public sbyte numeroCamino;
    NavMeshAgent obNMA;
    [SerializeField] Vector3[] trCaminos;
    Vector3 diferenciaVector;
    public float speedNPC;
    public float distancia;

    [Header("Mode HUD Variables")]
    public ModeNPCHenry mode;
    [SerializeField] Vector3 offset;
    [SerializeField] string nameNPC;
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;

    [Header("Anim")]
    public Animator obAnim;
    int numeroAnim;
    Color lineColor;
    


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;
       
        numeroAnim = 30;
        mode = ModeNPCHenry.Follow;


        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();




    }

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }

    public void Interactuar()
    {
        if ((isRange) && Input.GetButtonDown("Interactuar") && Inventory.instance.moverInv)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();
               
            }


            UIManager.InstanceGUI.icono.gameObject.SetActive(false);


        }

        if (detectarLimites && dialogueText.text == lines[index].Substring(1) && Input.GetButtonDown("Interactuar") && Inventory.instance.moverInv)
        {
            StartCoroutine(CloseDialogue());
            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        }

        RotateSon();
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
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);

        }


    }

    void DialogoRandom()
    {
        dialogoAnterior = Random.Range(0, 5);

        

        while (dialogoSiguiente == dialogoAnterior)
        {
            dialogoAnterior = Random.Range(0, 5);
        }


        dialogoSiguiente = dialogoAnterior;

        switch (dialogoAnterior)
        {
            case 0:
                lines = linesA0;
                break;

            case 1:
                lines = linesA1;
                break;

            case 2:
                lines = linesA2;
                break;

            case 3:
                lines = linesA3;
                break;

            case 4:
                lines = linesA5;
                break;

            default:
                break;
        }



    }

    IEnumerator DetectarLimites()
    {
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

        random01 = Random.Range(0, linesLogan.Length);

        while (random01 == random00)
        {
            random01 = Random.Range(0, linesLogan.Length);
        }

        lines[0] = linesLogan[random01];


        mode = ModeNPCHenry.Limites;

        UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);

        StartCoroutine(WriteDialogue());

    }

    void FollowPlayer()
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
    }
    public void StartDialogue()
    {
        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.InstanceGUI.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);


        DialogoRandom();

        FollowCameras.instance.mode = Modo.InDialogue;

        StartCoroutine(WriteDialogue());

    }

    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                Vector3 otraDirection = transform.position - trPlayer.transform.position;

                if (!detectarLimites)
                {
                    trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);
                }
                else
                {
                    trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, otraDirection, (0.5f) * Time.deltaTime);
                }
                

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }
           

        }


        if (!detectarLimites)
        {

            detector.SetActive(true);

            while (FollowCameras.instance.mode == Modo.InDialogue)
            {
                yield return null;
            }


            if (!lines[index].Trim().StartsWith("P"))
            {
                AnimToVar(index + 1);
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

                StartCoroutine(CloseDialogue());
            }

        



    }

    public IEnumerator CloseDialogue()
    {

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        FollowCameras.instance.mode = Modo.InGame;

        if (!detectarLimites)
        {
            while ((obCameras.orthographicSize < 7.5f) && offset != new Vector3(-15.00f, 12.5f, -15.00f))
            {
                obCameras.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;
                FollowCameras.instance.offset = Vector3.Slerp(FollowCameras.instance.offset, new Vector3(-15.00f, 12.5f, -15.00f), (speedZoom / 2.0f) * Time.deltaTime);

                yield return null;

            }

            marker.SetActive(true);


            numeroAnim = 0;
        }
        else
        {
            while ((obCameras.orthographicSize < 7.5f) )
            {
                obCameras.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;
              

                yield return null;

            }
        }

      
       

        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

      


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);

        didDialogueStart = false;

      

        MainCharacter.sharedInstance.canMove = true;



      

        detectarLimites = false;



    }


    void TurnToLogan()
    {

        Vector3 direction = trPlayer.transform.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);

    }

    void AnimToVar(int indice)
    {
        numeroAnim = indice;

    }
    

    private void Update()
    {


        if (mode == ModeNPCHenry.Follow)
        {
            FollowPlayer();
        }


        if (mode == ModeNPCHenry.Iddle)
        {
            Interactuar();
            TurnToLogan();

        }


        if (mode == ModeNPCHenry.Limites)
        {
            Interactuar();
            FollowPlayer();
            
           

        }



    }

    private void OnTriggerEnter(Collider other)
    {   
            if (other.gameObject.CompareTag("P1") && (mode == ModeNPCHenry.Follow || mode == ModeNPCHenry.Limites ))
            {

                mode = ModeNPCHenry.Iddle;

                obNMA.speed = 0.0f;
                isRange = !isRange;
                marker.SetActive(true);

                numeroAnim = 0;

        }
    }

    private void OnTriggerExit(Collider other)
    {
     
            if (other.gameObject.CompareTag("P1") && mode == ModeNPCHenry.Iddle)
            {

                    mode = ModeNPCHenry.Follow;

                    obNMA.speed = speedNPC;
                    isRange = !isRange;
                    marker.SetActive(false);

                    numeroAnim = 30;

            }
              


            
    }

    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);


    }




}
