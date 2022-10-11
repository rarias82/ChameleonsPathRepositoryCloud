using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCHenry
{
    Iddle, Follow, House, Final
}

public class NPC_Henry : MonoBehaviour
{

    public static NPC_Henry instance;

    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA0;
    [SerializeField, TextArea(4, 6)] string[] linesA1;
    [SerializeField, TextArea(4, 6)] string[] linesA2;
    [SerializeField, TextArea(4, 6)] string[] linesA3;
    [SerializeField, TextArea(4, 6)] string[] linesA4;
    [SerializeField, TextArea(4, 6)] string[] linesA5;
    [SerializeField, TextArea(4, 6)] string[] lines;
    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;




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
        obAnim = transform.Find("HenryAnim").gameObject.GetComponent<Animator>();
        numeroAnim = 30;
        mode = ModeNPCHenry.Follow;
        

    }

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }

    public void Interactuar()
    {
        if (isRange && Input.GetButtonDown("Interactuar") && Inventory.instance.moverInv)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();
            }

            UIManager.instance.icono.gameObject.SetActive(false);

        }

        RotateSon();
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
            UIManager.instance.PosicionarGlobo(transform.position);

        }


    }

    void DialogoRandom()
    {
        dialogoAnterior = dialogoSiguiente;

        dialogoSiguiente = Random.Range(0, 5);

        while (dialogoSiguiente == dialogoAnterior)
        {
            dialogoSiguiente = Random.Range(0, 5);
        }

        switch (dialogoSiguiente)
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

    void FollowPlayer()
    {
        diferenciaVector = trPlayer.position - transform.position;


        if (diferenciaVector.sqrMagnitude < (distancia * 2f))
        {


            lineColor = Color.red;


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

        UIManager.instance.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.instance.obMap.SetActive(false);
        UIManager.instance.obMapMark.SetActive(false);


        DialogoRandom();

        StartCoroutine(WriteDialogue());

    }

    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - trPlayer.transform.position;
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom / 2f) * Time.deltaTime);

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }

        }

        if (!lines[index].Trim().StartsWith("P"))
        {
            AnimToVar(index + 1);
        }
       
        dialogueText.text = string.Empty;

        UIManager.instance.ballonDialogue.gameObject.SetActive(true);

        IconDialogo(lines[index]);

        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
        }

        UIManager.instance.icono.gameObject.SetActive(true);

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

        numeroAnim = 0;

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
        

       

        

    }

    private void OnTriggerEnter(Collider other)
    {   
            if (other.gameObject.CompareTag("P1") && mode == ModeNPCHenry.Follow)
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
