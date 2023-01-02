using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCHenry
{
    Iddle, Follow, House, Final, Limites, FinalC
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

    [SerializeField, TextArea(4, 6)] string[] linesLogan0;
    [SerializeField, TextArea(4, 6)] string[] linesLogan1;
    [SerializeField, TextArea(4, 6)] string[] linesLogan2;
    [SerializeField, TextArea(4, 6)] string[] linesLogan3;
    [SerializeField, TextArea(4, 6)] string[] linesLogan4;

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

    bool dialogoRandomActivado = false;


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

    [Header("Capas Outline")]
    public GameObject capaObj;
    public LayerMask capa;
    public LayerMask capa0;

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
            obAnim.speed = 2.0f;
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
        if (isRange && NPC_Dialogue.instance.isRange && Inventory.instance.moverInv && !logan && !mapeo.logan && !UIManager.InstanceGUI.isGameOver)
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

        } // Dialogo Final

   

        if (detectarLimites && dialogueText.text == lines[index].Substring(1) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() &&  Inventory.instance.moverInv && !seAcabo) 
        {
            
            logan = true;
            StartCoroutine(CloseDialogue());
            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        } // Cuando Logan se aleja

        if ((isRange) && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv && !seAcabo && !UIManager.InstanceGUI.isGameOver)
        {
            if (!didDialogueStart && !logan && !detectarLimites)
            {
                dialogoRandomActivado = true;
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();

            }

            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        } //Dialogo comun

        
    }
    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position);
            MainCharacter.sharedInstance.VozLogan();
            UIManager.InstanceGUI.NombreDialogo("P");

            capaObj.layer = 20;
            MainCharacter.sharedInstance.capaObj.layer = 17;

        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(NPC_Dialogue.instance.transform.position);
            mapeo.VocesRandom();
            UIManager.InstanceGUI.NombreDialogo("L");

            capaObj.layer = 20;
            MainCharacter.sharedInstance.capaObj.layer = 20;
            mapeo.capaObj.layer = 16;






        }

        if (lineas.Trim().StartsWith("H"))
        {
            VocesRandom();
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
            UIManager.InstanceGUI.BurbujaDialogo(8);
            UIManager.InstanceGUI.NombreDialogo("H");

            capaObj.layer = 19;
            MainCharacter.sharedInstance.capaObj.layer = 20;

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
        UIManager.InstanceGUI.fadeBlackN = true;


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
            case 4:
                lines = linesLogan4;
                break;
        }

    

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
        UIManager.InstanceGUI.fadeBlackN = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);

        if (dialogoRandomActivado)
        {
            DialogoRandom();
        }
       

        


        if (seAcabo)
        {
            lines = linesFinalA;
            mapeo.QuitarMarca();

        }
        

        StartCoroutine(WriteDialogue());

    }
    public IEnumerator WriteDialogue()
    {
        marker.SetActive(false);
        if (index == 0)
        {

            if (detectarLimites)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
                MainCharacter.sharedInstance.eAnim = 2;
                MainCharacter.sharedInstance.animIntervalo = 0.0f;
                AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
            }
            else
            {
                /*detector.SetActive(true);*/

                UIManager.InstanceGUI.BurbujaDialogo(8);
                
               
                AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));

                if (lines[index].Trim().StartsWith("P"))
                {
                    FollowCameras.instance.velocidadRotacion = -75.0f;
                    FollowCameras.instance.mode = Modo.Mundo;
                }

                if (lines[index].Trim().StartsWith("L"))
                {
                    FollowCameras.instance.velocidadRotacion = -75.0f;
                    FollowCameras.instance.mode = Modo.Odnum;
                }

            }

            posOriginal = FollowCameras.instance.transform.position;

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 directionLogan = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                
                Vector3 directionL = transform.position - new Vector3(mapeo.transform.position.x, 6.079084f, mapeo.transform.position.z);

                if (seAcabo)
                {
                    mapeo.mode = ModeNPC.Final;
                    transform.forward = Vector3.Lerp(transform.forward, directionL, (speedZoom) * Time.deltaTime);
                    
                }
                else
                {
                    trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, directionLogan, (speedZoom) * Time.deltaTime);
                }
                //if (!detectarLimites)
                //{
                //    trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);
                //}
                //else
                //{
                //    trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, otraDirection, (0.5f) * Time.deltaTime);
                //}
                

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

            detector.SetActive(true);


			if (index>0)
			{
                if (lines[index].Trim().StartsWith("P"))
                {
                    FollowCameras.instance.velocidadRotacion = -75.0f;
                    FollowCameras.instance.mode = Modo.Mundo;
                }

                if (lines[index].Trim().StartsWith("H"))
                {
                    FollowCameras.instance.velocidadRotacion = -75.0f;
                    FollowCameras.instance.mode = Modo.Odnum;
                }
            }



            while (FollowCameras.instance.mode == Modo.Mundo)
            {
                yield return null;
            }

            while (FollowCameras.instance.mode == Modo.Odnum)
            {
                yield return null;
            }


            if (!lines[index].Trim().StartsWith("P"))
            {
                AnimToVar(index + 1);
            }
        }
       
        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

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
        UIManager.InstanceGUI.EmptyNames();



        UIManager.InstanceGUI.fadeFrom = true;
        UIManager.InstanceGUI.fadeFromN = true;



        if (!detectarLimites)
        {
            //FollowCameras.instance.mode = Modo.InGame;


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

            if (!seAcabo)
            {
                marker.SetActive(true);
            }

           


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

            //marker.SetActive(true);
        }




        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);

        if (seAcabo) //Final de hermanos
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

            MainCharacter.sharedInstance.puedePausar = true;

            marker.SetActive(false);

            if (mapeo.rana.gameObject.activeInHierarchy)
            {
                mapeo.rana.DesaparecerRana(false);
            }
        }
        else
        {
            


            if (detectarLimites)
            {
                marker.SetActive(false);
            }


            detectarLimites = false;

            yield return new WaitForSeconds(0.01f);
            
            

            
            logan = false;

            Debug.Log("Marca");

            marker.SetActive(true);
          
            
            didDialogueStart = false;
            MainCharacter.sharedInstance.canMove = true;
        }

        if (UIManager.InstanceGUI.isGameOver)
        {
            UIManager.InstanceGUI.FinDelJuego();
        }


        MainCharacter.sharedInstance.puedePausar = true;
        VolverColores();

    }


    void VolverColores()
    {

        capaObj.layer = 19;
        MainCharacter.sharedInstance.capaObj.layer = 17;
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
    void TurnToLeahn()
    {
       
            Vector3 directionLeahn = mapeo.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, directionLeahn, (speedZoom / 2.5f) * Time.deltaTime);

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
            


        }
        if (mode == ModeNPCHenry.FinalC)
        {
            Interactuar();
            TurnToLeahn();
            
        }

        RotateSon();

    }
    private void OnTriggerEnter(Collider other)
    {   
            if (other.gameObject.CompareTag("P1") && (mode == ModeNPCHenry.Follow || mode == ModeNPCHenry.Limites ))
            {


            MainCharacter.sharedInstance.puedePausar = false;

            mode = ModeNPCHenry.Iddle;

                obNMA.speed = 0.0f;
                isRange = !isRange;

            if (!logan)
            {
                marker.SetActive(true);
            }
                

                numeroAnim = 0;

        }


        if (other.gameObject.CompareTag("P1") && ((mode == ModeNPCHenry.Final) || (mode == ModeNPCHenry.FinalC)))
        {


            MainCharacter.sharedInstance.puedePausar = false;

            isRange = !isRange;

           


            numeroAnim = 0;

        }






        obAnim.speed = 1.0f;
    }
    private void OnTriggerExit(Collider other)
    {
     
            if (other.gameObject.CompareTag("P1") && mode == ModeNPCHenry.Iddle)
            {

                    mode = ModeNPCHenry.Follow;

            MainCharacter.sharedInstance.puedePausar = true;
            isRange = !isRange;
                    marker.SetActive(false);

                    StartCoroutine(Esperar());
            //obNMA.speed = speedNPC;


            obAnim.speed = 2.0f;
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
