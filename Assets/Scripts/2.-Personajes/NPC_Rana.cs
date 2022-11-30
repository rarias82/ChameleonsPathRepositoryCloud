using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCRana
{
    Iddle, Walk, Follow, House, Final, Limites, Carrera, FinalB
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

    [SerializeField, TextArea(4, 6)] string[] linesEA0;
    [SerializeField, TextArea(4, 6)] string[] linesEA1;
    [SerializeField, TextArea(4, 6)] string[] linesEA2;
    [SerializeField, TextArea(4, 6)] string[] linesEA3;
    [SerializeField, TextArea(4, 6)] string[] linesEA4;

    [SerializeField, TextArea(4, 6)] string[] linesEB0;
    [SerializeField, TextArea(4, 6)] string[] linesEB1;
    [SerializeField, TextArea(4, 6)] string[] linesEB2;
    [SerializeField, TextArea(4, 6)] string[] linesEB3;
    [SerializeField, TextArea(4, 6)] string[] linesEB4;

    [SerializeField, TextArea(4, 6)] string[] linesEC0;
    [SerializeField, TextArea(4, 6)] string[] linesEC1;
    [SerializeField, TextArea(4, 6)] string[] linesEC2;
    [SerializeField, TextArea(4, 6)] string[] linesEC3;
    [SerializeField, TextArea(4, 6)] string[] linesEC4;

    [SerializeField, TextArea(4, 6)] string[] linesFA;
    [SerializeField, TextArea(4, 6)] string[] linesFB;
    [SerializeField, TextArea(4, 6)] string[] linesFC;


    [SerializeField, TextArea(4, 6)] string[] lines;
    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;
    public Mapa _map;
    public GameObject detector;
    NPC_Dialogue mapeo;
    [SerializeField] HouseDialogue hd;
    public bool iniciarAnim;
    public bool terminarAnim;
    public bool animOn;
    public float numeroAnimVelocity;
    public Transform posOriginal;
    int random000 = 0;
    int random001 = 0;

    [Header("Options References")]
    public GameObject Options;
    [SerializeField] GameObject[] listOptions;
    [SerializeField] GameObject selector;
    [SerializeField] sbyte id_selector;
    [SerializeField] string[] optionLines;
    public bool pregunta;
    [SerializeField] sbyte nextDialogue;

    [Header("Move Variables")]
    public sbyte numeroCamino;
    public sbyte numeroCaminoCarrera;
    public NavMeshAgent obNMA;
    [SerializeField] Vector3[] trCaminos;
    [SerializeField] Vector3[] trCaminosCarrera;
    Vector3 diferenciaVector;
    Vector3 diferenciaVectorCarrera;
    public float speedNPC;
    public float distancia;
    public float incrementadorVelocidad;

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
    public int numeroAnim;
    

    [Header("Music References")]
    public AudioClip cancion;
    [SerializeField] AudioClip[] voces;
    [SerializeField] AudioClip[] voces1;
    int voz000, voz001;

    [Header("Intanciar")]
    public static NPC_Rana instanciaRana;

    [Header("Finales")]
    public bool finalA;
    public bool finalB;
    public bool finalB2;
    public Item[] objetosTodo;



    private void Awake()
    {
        instanciaRana = this;     
    }

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

        //obNMA.speed = 4.0f;
        numeroAnim = 30;

        //numeroAnim = 400;

       


    }

    void Start()
    {
        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        mapeo = FindObjectOfType<NPC_Dialogue>();

        Options = GameObject.Find("DialogueOptions").gameObject;

        selector = GameObject.Find("Select").gameObject;

        listOptions[0] = GameObject.Find("Panel0").gameObject;
        listOptions[1] = GameObject.Find("Panel1").gameObject;
        listOptions[2] = GameObject.Find("Panel2").gameObject;

        Item[] objetosTodo = FindObjectsOfType<Item>();

        foreach (Item objeto in objetosTodo)
        {
            objeto.gameObject.SetActive(false);
        }
    }
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
    void VocesRandom1()
    {
        voz000 = voz001;

        voz001 = Random.Range(0, voces1.Length);


        while (voz001 == voz000)
        {
            voz001 = Random.Range(0, voces1.Length);
        }

        AudioManager.Instance.PlaySound(voces1[voz001]);

    }
    public void Interactuar()
    {

        if (isRange && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv /*&& !mapeo.didDialogueStart && !mapeo.houses.didDialogueStart && !mapeo.henry.didDialogueStart*/)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1) && UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 0)
            {
                NextDialogue();

                UIManager.InstanceGUI.icono.gameObject.SetActive(false);

            }


        }

        
        RotateSon();
    }
    public void StartDialogue()
    {
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));


        if (pregunta)
        {
            switch (nextDialogue)
        {

            case 0:


                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEA0;
                }
                if (random001 == 1)
                {
                    lines = linesEA1;
                }
                if (random001 == 2)
                {
                    lines = linesEA2;
                }
                if (random001 == 3)
                {
                    lines = linesEA3;
                }
                if (random001 == 4)
                {
                    lines = linesEA4;
                }

                break;

            case 1:
                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEB0;
                }
                if (random001 == 1)
                {
                    lines = linesEB1;
                }
                if (random001 == 2)
                {
                    lines = linesEB2;
                }
                if (random001 == 3)
                {
                    lines = linesEB3;
                }
                if (random001 == 4)
                {
                    lines = linesEB4;
                }
                break;

            case 2:
                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEC0;
                }
                if (random001 == 1)
                {
                    lines = linesEC1;
                }
                if (random001 == 2)
                {
                    lines = linesEC2;
                }
                if (random001 == 3)
                {
                    lines = linesEC3;
                }
                if (random001 == 4)
                {
                    lines = linesEC4;
                }
                break;

            default:
                break;
        }
        }
        

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        MainCharacter.sharedInstance.animIntervalo = 0.0f;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.InstanceGUI.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);


        DialogoFinal();

        StartCoroutine(WriteDialogue());

    }
    public IEnumerator WriteDialogue()
    {
        detector.SetActive(true);

        

        if (index == 0)
        {
            posOriginal = FollowCameras.instance.transform;

            FollowCameras.instance.velocidadRotacion = -75.0f;
            FollowCameras.instance.mode = Modo.Mundo;

           

            UIManager.InstanceGUI.BurbujaDialogo(0);

            

            while (obCameras.orthographicSize > 3.5f)
            {

                
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);
           
                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }

            


        }

        if (!pregunta)
        {
            if (index == 1)
            {
                MainCharacter.sharedInstance.eAnim = 20;
                UIManager.InstanceGUI.BurbujaDialogo(4);
            }

            if (index == 3)
            {
                MainCharacter.sharedInstance.eAnim = 21;
                UIManager.InstanceGUI.BurbujaDialogo(2);
            }

            if (index == 5)
            {
                MainCharacter.sharedInstance.eAnim = 22;
                UIManager.InstanceGUI.BurbujaDialogo(0);
            }

        }
        else
        {
            if (nextDialogue == 0)
            {
                if (index == 2)
                {
                    MainCharacter.sharedInstance.eAnim = 20;
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
            }

            
        }



        //if (index == 7)
        //{
        //    MainCharacter.sharedInstance.eAnim = 0;
        //}




        while (FollowCameras.instance.mode == Modo.Mundo)
        {
            yield return null;
        }

        if (numeroAnim >= 100 && numeroAnim < 200)
        {
            AnimToVar(index + 100);
        }
        if (numeroAnim >= 200 && numeroAnim < 300)
        {
            AnimToVar(index + 200);
        }


        if (numeroAnim < 100)
        {
            AnimToVar(index);
        }
        
            
        

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

        IconDialogo(lines[index]);


        if (!pregunta)
        {
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

            if (index == 1)
            {
                UIManager.InstanceGUI.BurbujaDialogo(4);
            }

            if (index == 2)
            {
                UIManager.InstanceGUI.BurbujaDialogo(7);
            }

            if (index == 2)
            {
                UIManager.InstanceGUI.BurbujaDialogo(0);
            }
        }
        else
        {
            if (nextDialogue == 0)
            {
                if (index == 3)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                }
                if (index == 4)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }
                if (index == 5)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(5);
                }
                if (index == 6)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
                if (index == 7)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                }

                if (index == 8)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }

                if (index == 8)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }

                if (index == 8)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
            }
        }

        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;

            
            yield return new WaitForSeconds(speedText);
        }





        //yield return new WaitForSeconds(5f);

        //terminarAnim = true;
        if (dialogueText.text == lines[index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }
        



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
            if (!pregunta)
            {
                VocesRandom();
            }
            else
            {
                VocesRandom1();
            }
            

        }


    }
    public IEnumerator CloseDialogue()
    {
        detector.SetActive(true);

        index = 0;

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
      

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;

        UIManager.InstanceGUI.fadeFrom = true;

        numeroAnim = 99;

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


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);



        if (finalA)
        {
            UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalARana());
        }
        else if (finalB && !finalB2)
        {
            UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalBRana());
            
        }
        else if(finalB2)
        {
            UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalARana());
        }
        else
        {
            MainCharacter.sharedInstance.canMove = true;

            yield return new WaitForSeconds(0.01f);
            didDialogueStart = false;
            marker.SetActive(true);
        }

        MainCharacter.sharedInstance.eAnim = 0;

    }
    public void Navegate()
    {

        if (mapeo._map.Jugador.BDOWN.WasPressedThisFrame() && id_selector < listOptions.Length - 1)
        {
            id_selector++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        if (mapeo._map.Jugador.BUP.WasPressedThisFrame() && id_selector > 0)
        {
            id_selector--;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (mapeo._map.Jugador.Interactuar.WasPressedThisFrame())
        {

            AudioManager.Instance.PlaySound(AudioManager.Instance.clickButton);

            MainCharacter.sharedInstance.eAnim = 22;

            switch (id_selector)
            {
                case 0:
                    numeroAnim = 100;
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);


                    foreach (Item objeto in objetosTodo)
                    {
                        objeto.gameObject.SetActive(true);
                    }


                    break;

                case 1:
                    numeroAnim = 200;
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);
                    finalB = true;



                        break;

                case 2:
                    numeroAnim = 200;
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);
                    finalB = true;

                    break;

                default:
                    break;

                   

            }


            UIManager.InstanceGUI.BurbujaDialogo(2);


            UIManager.InstanceGUI.AnimateOptions(false);



            StartCoroutine(ChangeDialogueR());


        }


    }
    IEnumerator ChangeDialogueR()
    {
        index = 0;

        pregunta = true;

        switch (id_selector)
        {
            case 0:

                lines = linesA0;

                break;

            case 1:

                lines = linesB0;
                break;

            case 2:

                lines = linesC0;
                break;


            default:
                break;
        }

        nextDialogue = id_selector;
        yield return null;

        StartCoroutine(WriteDialogue());




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

            if (!pregunta)
            {

                StartCoroutine(Esperar());


            }
            else
            {
                StartCoroutine(CloseDialogue());
            }
            //if (talkToLeahn && !finishDialogue && !optionCBuscarHermano)
            //{

            //    UIManager.InstanceGUI.AnimateOptions(true);

            //    if (respuestaDada.nextDialogueToTalk == 0)
            //    {
            //        for (int i = 0; i < listOptions.Length; i++)
            //        {
            //            listOptions[i].text = optionLinesA[i];
            //        }
            //    }

            //    if (respuestaDada.nextDialogueToTalk == 2)
            //    {
            //        for (int i = 0; i < listOptions.Length; i++)
            //        {
            //            listOptions[i].text = optionLinesC[i];
            //        }
            //    }
            //}
            //else
            //{


            //    StartCoroutine(CloseDialogue());
            //}
            
        }

    }
    IEnumerator Esperar()
    {

        FollowCameras.instance.OnConfetis(0);
        for (int i = 0; i < listOptions.Length; i++)
        {
            listOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLines[i];
        }


        index = 0;
        mapeo._map.Disable();
        yield return new WaitForSeconds(1f);
        UIManager.InstanceGUI.AnimateOptions(true);
        
        yield return new WaitForSeconds(1f);
        


        //Options.SetActive(true);

       
        yield return new WaitForSeconds(0.5f);
        mapeo._map.Enable();
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
    void Carrera()
    {
        diferenciaVectorCarrera = trCaminosCarrera[numeroCaminoCarrera] - transform.position;

        if (diferenciaVectorCarrera.sqrMagnitude < (0.5f * 2f))
        {
            numeroCaminoCarrera++;
           
            
            obNMA.speed += incrementadorVelocidad;
            if (obNMA.speed > 6.4)
            {
                obNMA.speed = 6.4f;
            }
            
            if (numeroCaminoCarrera >= trCaminosCarrera.Length)
            {
                didDialogueStart = false;
                mode = ModeNPCRana.FinalB;
                numeroAnim = 99;
                numeroCaminoCarrera = 0;
            }

        }

        if (mode != ModeNPCRana.FinalB)
        {
            obNMA.SetDestination(trCaminosCarrera[numeroCaminoCarrera]);
        }
        



    }
    void DialogoFinal()
    {
        if (finalA)
        {
            lines = linesFA;
        }

        if (finalB)
        {
            lines = linesFB;
            finalB2 = true;
        }

    }
    private void Update()
    {

        if (mode == ModeNPCRana.Walk)
        {
            Walking();
        }


        if (mode == ModeNPCRana.Iddle)
        {

            if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 && (!mapeo.didDialogueStart || !mapeo.gameObject.activeInHierarchy) && !hd.didDialogueStart)
            {
                Navegate();
            }
            Interactuar();
            TurnToLogan();
            AnimacionTexto();

        }

        if (mode == ModeNPCRana.FinalB)
        {

           
            Interactuar();
            TurnToLogan();
            

        }


        if (mode == ModeNPCRana.Carrera)
        {

            Carrera();

        }

        //if (mapeo._map.Jugador.Interactuar.WasPressedThisFrame())
        //{
        //    UIManager.InstanceGUI.Temblor();
        //}

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

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.FinalB)
        {

            

            obNMA.speed = 0.0f;
            isRange = true;
            marker.SetActive(true);
            numeroAnim = 99;
            
            

        }

        //if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Iddle)
        //{

        //    mode = ModeNPCRana.Iddle;

        //    obNMA.speed = 0.0f;
        //    isRange = !isRange;
        //    marker.SetActive(true);
        //    numeroAnim = 99;
        //    didDialogueStart = false;

        //}



    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Iddle && !finalB)
        {

            mode = ModeNPCRana.Walk;

            obNMA.speed = speedNPC;
            isRange = !isRange;
            marker.SetActive(false);
            numeroAnim = 30;

        }

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.FinalB)
        {

           

            obNMA.speed = 0.0f;
            isRange = false;
            marker.SetActive(false);
            numeroAnim = 99;
           

        }


    }
    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);


    }




}
