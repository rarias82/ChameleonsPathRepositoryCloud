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
    [SerializeField, TextArea(4, 6)] string[] linesFinalA;

    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;
    int random00;
    int random01;
    public GameObject detector;
    public bool logan;
    NPC_Dialogue mapeo;

    public bool seAcabo;

    [SerializeField] HouseDialogue hd;

    public Vector3 posOriginal;


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

    [Header("Music References")]
    public AudioClip cancion;
    [SerializeField] AudioClip[] voces;
    int voz000, voz001;

    [Header("Efectos")]
    [SerializeField] ParticleSystem polvoTierra;
    

    void VocesRandom()
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

        polvoTierra.Stop();

        if (hd.optionCBuscarHermano)
        {
            mode = ModeNPCHenry.Iddle;
        }
        else
        {
            numeroAnim = 30;
            mode = ModeNPCHenry.Follow;
        }
        


        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        mapeo = FindObjectOfType<NPC_Dialogue>();




    }

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }

    public void Interactuar()
    {



        if (isRange && NPC_Dialogue.instance.isRange && Inventory.instance.moverInv && !logan && !mapeo.logan)
        {
            if (!didDialogueStart)
            {
                seAcabo = true;
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame())
            {
                NextDialogue();

                UIManager.InstanceGUI.icono.gameObject.SetActive(false);

            }


        }


        if (detectarLimites && dialogueText.text == lines[index].Substring(1) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() &&  Inventory.instance.moverInv && !seAcabo)
        {
            
            logan = true;
            StartCoroutine(CloseDialogue());
            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        }



        if ((isRange) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv && !seAcabo)
        {
            if (!didDialogueStart && !logan && !detectarLimites)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();

            }


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
            UIManager.InstanceGUI.PosicionarGlobo(NPC_Dialogue.instance.transform.position);
        }

        if (lineas.Trim().StartsWith("H"))
        {
            VocesRandom();
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
            UIManager.InstanceGUI.BurbujaDialogo(8);

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
        MainCharacter.sharedInstance.animIntervalo = 0.0f;

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

        


        if (seAcabo)
        {
            lines = linesFinalA;
        }
        

        StartCoroutine(WriteDialogue());

    }

    public IEnumerator WriteDialogue()
    {
        if (index == 0)
        {


            

            if (detectarLimites)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
                MainCharacter.sharedInstance.eAnim = 2;
                MainCharacter.sharedInstance.animIntervalo = 0.0f;
            }
            else
            {
                detector.SetActive(true);

                UIManager.InstanceGUI.BurbujaDialogo(8);

                FollowCameras.instance.velocidadRotacion = -75.0f;
                FollowCameras.instance.mode = Modo.Mundo;
                
               
                AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));
            }

            posOriginal = FollowCameras.instance.transform.position;
            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                Vector3 otraDirection = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);

                //if (seAcabo)
                //{

                //}
                //Vector3 directionL = transform.position - new Vector3(mapeo.transform.position.x, 6.079084f, mapeo.transform.position.z);
                //Vector3 otraDirectionL = transform.position - new Vector3(mapeo.transform.position.x, 6.079084f, mapeo.transform.position.z);

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

        if (lines == linesA5)
        {

            if ((index == 0) || (index == 2))
            {
                UIManager.InstanceGUI.BurbujaDialogo(1);
            }
            
        }

        if (lines == linesFinalA)
        {
            if (index % 2 == 0)
            {
                UIManager.InstanceGUI.BurbujaDialogo(7);
            }
            else
            {
                UIManager.InstanceGUI.BurbujaDialogo(3);
            }



        }


        if (!detectarLimites)
        {

            

            while (FollowCameras.instance.mode == Modo.Mundo)
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

                StartCoroutine(CloseDialogue());
            }

        



    }

    public IEnumerator CloseDialogue()
    {
        detector.SetActive(true);
        index = 0;

        MainCharacter.sharedInstance.eAnim = 0;


        if (!logan)
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
        }
        
        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        

        if (!detectarLimites)
        {
            //FollowCameras.instance.mode = Modo.InGame;


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

            marker.SetActive(true);


            numeroAnim = 0;
        }
        else
        {

            didDialogueStart = true;
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

        

      

        



      

        


        if (seAcabo)
        {
            detectarLimites = false;

            didDialogueStart = true;
            logan = false;
            UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalA());

            if (!mapeo.rana.gameObject.GetComponent<NPC_Rana>().finalA)
            {
                mapeo.rana.gameObject.SetActive(true);
            }
            else
            {
                mapeo.rana.gameObject.SetActive(false);
            }


            

        }
        else
        {
            MainCharacter.sharedInstance.canMove = true;
            if (detectarLimites)
            {

                marker.SetActive(false);



            }


            detectarLimites = false;

            yield return new WaitForSeconds(0.01f);
            
            

            didDialogueStart = false;
            logan = false;
        }

      

    }

    public void Final()
    {
       

    }
    void TurnToLogan()
    {
        if (seAcabo)
        {
            Vector3 directionLeahn = mapeo.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, directionLeahn, (speedZoom / 2.5f) * Time.deltaTime);
        }
        else
        {
            Vector3 direction = trPlayer.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);
        }
        

     

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

        if (mode == ModeNPCHenry.Final)
        {
            Interactuar();
            //FollowPlayer();


        }



    }

    private void OnTriggerEnter(Collider other)
    {   
            if (other.gameObject.CompareTag("P1") && (mode == ModeNPCHenry.Follow || mode == ModeNPCHenry.Limites ))
            {

                mode = ModeNPCHenry.Iddle;

                obNMA.speed = 0.0f;
                isRange = !isRange;

            if (!logan)
            {
                marker.SetActive(true);
            }
                

                numeroAnim = 0;

        }


        if (other.gameObject.CompareTag("P1") && (mode == ModeNPCHenry.Final))
        {

            

            
            isRange = !isRange;

           


            numeroAnim = 0;

        }
    }

    private void OnTriggerExit(Collider other)
    {
     
            if (other.gameObject.CompareTag("P1") && mode == ModeNPCHenry.Iddle)
            {

                    mode = ModeNPCHenry.Follow;

                    
                    isRange = !isRange;
                    marker.SetActive(false);

                    StartCoroutine(Esperar());
            //obNMA.speed = speedNPC;
            


        }
              


            
    }

    IEnumerator Esperar()
    {

        
        yield return new WaitForSeconds(0.25f);
        polvoTierra.Play();
        numeroAnim = 30;
        obNMA.speed = speedNPC;
    }

    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);


    }




}
