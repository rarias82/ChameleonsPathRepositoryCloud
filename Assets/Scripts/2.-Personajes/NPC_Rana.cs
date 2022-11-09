using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCRana
{
    Iddle, Walk, Follow, House, Final, Limites
}
public class NPC_Rana : MonoBehaviour
{
    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;
    

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA0;
    [SerializeField, TextArea(4, 6)] string[] linesB0;
    [SerializeField, TextArea(4, 6)] string[] linesC0;
    [SerializeField, TextArea(4, 6)] string[] lines;
    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;
    public Mapa _map;
    public GameObject detector;
    NPC_Dialogue mapeo;
    public bool iniciarAnim;
    public bool terminarAnim;
    public bool animOn;
    public float numeroAnimVelocity;

    [Header("Move Variables")]
    public sbyte numeroCamino;
    NavMeshAgent obNMA;
    [SerializeField] Vector3[] trCaminos;
    Vector3 diferenciaVector;
    public float speedNPC;
    public float distancia;

    [Header("Mode HUD Variables")]
    public ModeNPCRana mode;
    [SerializeField] Vector3 offset;
    [SerializeField] string nameNPC;
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;


    [Header("Anim")]
    public Animator obAnim;
    int numeroAnim;
    

    [Header("Music References")]
    public AudioClip cancion;


    public void SetInputActions(Mapa map)
    {
        _map = map;
    }

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }

    private void OnEnable()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;
        numeroAnim = 30;

        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        mapeo = FindObjectOfType<NPC_Dialogue>();
    }

    public void Interactuar()
    {

        if (isRange && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame())
            {
                NextDialogue();

                UIManager.InstanceGUI.icono.gameObject.SetActive(false);

            }


        }


        RotateSon();
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



        FollowCameras.instance.mode = Modo.InDialogue;


        StartCoroutine(WriteDialogue());

    }

    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));
           


            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);
           
                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }

            
        }


            detector.SetActive(true);

            while (FollowCameras.instance.mode == Modo.InDialogue)
            {
                yield return null;
            }


                AnimToVar(index);
            
        

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

        IconDialogo(lines[index]);



        if ((index % 2 == 0) && (index <= 4))
        {
            animOn = true;
            iniciarAnim = true;
        }
        else
        {
            dialogueText.transform.localScale = Vector3.one;
            iniciarAnim = false;
            terminarAnim = false;
            animOn = false;
        }

        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;

            
            yield return new WaitForSeconds(speedText);
        }



        

        //yield return new WaitForSeconds(5f);

        //terminarAnim = true;

        UIManager.InstanceGUI.icono.gameObject.SetActive(true);



    }

    void AnimToVar(int indice)
    {
        numeroAnim = indice;

    }

    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position);
        }

      
        if (lineas.Trim().StartsWith("R"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);

        }


    }

    public IEnumerator CloseDialogue()
    {

        index = 0;

     
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
      

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        FollowCameras.instance.mode = Modo.InGame;

            while ((obCameras.orthographicSize < 7.5f) && offset != new Vector3(-15.00f, 12.5f, -15.00f))
            {
                obCameras.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;
                FollowCameras.instance.offset = Vector3.Slerp(FollowCameras.instance.offset, new Vector3(-15.00f, 12.5f, -15.00f), (speedZoom / 2.0f) * Time.deltaTime);

                yield return null;

            }

       


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);

        MainCharacter.sharedInstance.canMove = true;
         
        marker.SetActive(true);

        didDialogueStart = false;



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

    public void AnimacionTexto()
    {


        if (animOn)
        {
            if (iniciarAnim)
            {
                dialogueText.transform.localScale = new Vector3(Mathf.MoveTowards(dialogueText.transform.localScale.x, 0.5f, numeroAnimVelocity * Time.deltaTime), dialogueText.transform.localScale.y, dialogueText.transform.localScale.z);/*Vector3.MoveTowards(dialogueText.transform.localScale, new Vector3(0.5f, 1.0f, 1.0f), numeroAnimVelocity * Time.deltaTime);*/

                if (dialogueText.transform.localScale.x == 0.5f)
                {
                    iniciarAnim = false;
                    terminarAnim = true;

                }
            }

            if (terminarAnim)
            {
                dialogueText.transform.localScale = new Vector3(Mathf.MoveTowards(dialogueText.transform.localScale.x, 1f, numeroAnimVelocity * Time.deltaTime), dialogueText.transform.localScale.y, dialogueText.transform.localScale.z);/*Vector3.MoveTowards(dialogueText.transform.localScale, new Vector3(0.5f, 1.0f, 1.0f), numeroAnimVelocity * Time.deltaTime);*/

                if (dialogueText.transform.localScale.x == 1f)
                {
                    iniciarAnim = true;
                    terminarAnim = false;

                }
            }
        }

       


    }

    void TurnToLogan()
    {

        Vector3 direction = trPlayer.transform.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);

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

    private void Update()
    {

        if (mode == ModeNPCRana.Walk)
        {
            Walking();
        }


        if (mode == ModeNPCRana.Iddle)
        {
            Interactuar();
            TurnToLogan();
            AnimacionTexto();

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Walk)
        {

            mode = ModeNPCRana.Iddle;

            obNMA.speed = 0.0f;
            isRange = !isRange;
            marker.SetActive(true);
            numeroAnim = 99;

        }


        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Iddle)
        {

            mode = ModeNPCRana.Walk;

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
