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
    [Header("Mode Follow Variables")]
    public ModeNPCHenry mode;
    public NavMeshAgent obNv;
    public Transform trPlayer;
    public Vector3 offset, diferenciaVector, posInicial,posfuera;
    public float distancia;
    public Animator obAnim;
    public float contador, temporizador;
    public bool tiempoesperando;
    public int contadorMaximo;
    public bool puedeSeguir;
    public bool finalBueno;
    public bool finalMalo;
    public float speedAtcual,speedMaxima;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] bool didDialogueStart;
    [SerializeField] bool isRange;
    [SerializeField] int index;
    NPC_Dialogue respuestaDada;
    HouseDialogue cabana;
    [SerializeField, TextArea(4, 6)] string[] linesA;
    [SerializeField, TextArea(4, 6)] string[] linesC;
    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string[] linesAFinal;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal;
    GameObject marker;

    [Header("Anim References")]
    public int numeroAnim;
    public Image blackScreen;

    // Start is called before the first frame update
    public void MedirDistancia()
    {
        diferenciaVector = trPlayer.position  - transform.position;
    }
    void Following()
    {

        MedirDistancia();

        
      

        
        if (diferenciaVector.sqrMagnitude < (distancia * 2f))
        {

            mode = ModeNPCHenry.Iddle;
            Debug.Log("Llego");


        }
        else
        {
            Debug.Log("No llego");
            obNv.SetDestination(trPlayer.position);
        }

        

    }

    void StartDialogue()
    {
        didDialogueStart = true;

        puedeSeguir = true;

        contador = 0f;

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.canMove = false;


        UIManager.instance.fadeBlack = true;

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.instance.obMap.SetActive(false);
        UIManager.instance.obMapMark.SetActive(false);

        if (finalBueno)
        {
            lines = linesAFinal;
            UIManager.instance.GanarPuntos(true, UIManager.instance.puntos);
            cabana.didDialogueStart = true;
            
        }
        else
        {
            UIManager.instance.GanarPuntos(false, UIManager.instance.puntos);
        }

        if (finalMalo)
        {
            lines = linesCFinal;
            UIManager.instance.GanarPuntos(true, UIManager.instance.puntos);
            cabana.didDialogueStart = true;
        }

        StartCoroutine(WriteDialogue());

    }

    IEnumerator WriteDialogue()
    {

        if (index == 0)
        {

            while (respuestaDada.obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, trPlayer.transform.position.y, trPlayer.transform.position.z);

                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (respuestaDada.speedZoom / 2f) * Time.deltaTime);

                respuestaDada.obCameras.orthographicSize -= respuestaDada.speedZoom * Time.deltaTime;

                yield return null;

            }
        }

        dialogueText.text = string.Empty;

        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(respuestaDada.speedText);
        }
        UIManager.instance.icono.gameObject.SetActive(true);
    }

    IEnumerator CloseDialogue()
    {
        index = 0;

        dialogueText.text = string.Empty;

        UIManager.instance.fadeFrom = true;

        while (respuestaDada.obCameras.orthographicSize < 7.5f)
        {
            respuestaDada.obCameras.orthographicSize += respuestaDada.speedZoom * Time.deltaTime;
            yield return null;

        }

       
        

        while (blackScreen.color.a < 1f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, 1.75f * Time.deltaTime));
            yield return null;
        }



        if (puedeSeguir)
        {
            transform.position = posfuera;
            obAnim.gameObject.SetActive(false);
            
        }

        if (finalBueno)
        {
            transform.position = posInicial;
            respuestaDada.gameObject.transform.position = posInicial;

            respuestaDada.gameObject.SetActive(false);
            didDialogueStart = true;
        }

        if (finalMalo)
        {
            transform.position = posfuera;
            respuestaDada.gameObject.transform.position = posfuera;

            respuestaDada.gameObject.SetActive(false);
            didDialogueStart = true;
        }


        yield return new WaitForSeconds(0.5f);

        while (blackScreen.color.a > 0f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, 1.75f * Time.deltaTime));
            yield return null;
        }

        Inventory.instance.panelItem.SetActive(true);
        UIManager.instance.obMap.SetActive(true);
        UIManager.instance.obMapMark.SetActive(true);

        MainCharacter.sharedInstance.canMove = true;


        cabana.falloTiempo = true;
        cabana.didDialogueStart = false;

        puedeSeguir = false;


        if (!finalBueno)
        {
            didDialogueStart = false;
            
        }

        obAnim.gameObject.SetActive(true);
        gameObject.SetActive(false);

        


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

    IEnumerator FadeinOut()
    {
        while (blackScreen.color.a < 1f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, 1.75f * Time.deltaTime));
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
       
        while (blackScreen.color.a > 0f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, 1.75f * Time.deltaTime));
            yield return null;
        }


    }
    void Start()
    {
        respuestaDada = GameObject.Find("NPC_Level_Leahn").GetComponent<NPC_Dialogue>();
        cabana = GameObject.Find("CabanaHermanos").GetComponent<HouseDialogue>();
        //posInicial = transform.position;
        obNv.speed = speedMaxima;
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        numeroAnim = 0;

    }



    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }
    // Update is called once per frame
    void Update()
    {

      

        if (!finalMalo)
        {
            if (isRange && respuestaDada.isRange)
            {

                finalBueno = true;
                contador = 0;


            }

            if (respuestaDada.isRange)
            {
                contador = 0;
            }

            if (mode == ModeNPCHenry.Follow)
            {

                Following();

                if (tiempoesperando && !finalBueno)
                {
                    contador += temporizador * Time.deltaTime;

                    if (contador >= contadorMaximo)
                    {
                        tiempoesperando = false;
                    }
                }

            }


            if (isRange && contador >= contadorMaximo && Inventory.instance.moverInv && !finalBueno)
            {
                if (!didDialogueStart)
                {
                    StartDialogue();
                }

            }

            if (isRange && Input.GetButtonDown("Interactuar") && puedeSeguir && Inventory.instance.moverInv && !finalBueno)
            {
                if (!didDialogueStart)
                {

                }

                else if (dialogueText.text == lines[index])
                {
                    NextDialogue();
                }

                UIManager.instance.icono.gameObject.SetActive(false);
            }


            if (isRange && Inventory.instance.moverInv && finalBueno)
            {
                if (!didDialogueStart)
                {
                    StartDialogue();
                }

            }

            if (isRange && Input.GetButtonDown("Interactuar") && puedeSeguir && Inventory.instance.moverInv && finalBueno)
            {
                if (!didDialogueStart)
                {

                }

                else if (dialogueText.text == lines[index])
                {
                    NextDialogue();
                }

                UIManager.instance.icono.gameObject.SetActive(false);
            }
        }
        else
        {
            if (isRange && respuestaDada.isRange && Inventory.instance.moverInv && finalMalo)
            {
                if (!didDialogueStart)
                {
                    StartDialogue();
                }

            }

            if (isRange && respuestaDada.isRange && Input.GetButtonDown("Interactuar") && puedeSeguir && Inventory.instance.moverInv && finalMalo)
            {
                if (!didDialogueStart)
                {

                }

                else if (dialogueText.text == lines[index])
                {
                    NextDialogue();
                }

                UIManager.instance.icono.gameObject.SetActive(false);
            }
        }
       

    }

    public void Aparecer()
    {
        mode = ModeNPCHenry.Final;
        obNv.speed = 0;
        transform.position = posInicial;
        finalMalo = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mode == ModeNPCHenry.Follow)
        {
            obNv.speed = 0f;

            isRange = true;
            
        }

        if (mode == ModeNPCHenry.Final)
        {
            obNv.speed = 0f;

            isRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mode == ModeNPCHenry.Follow)
        {
            obNv.speed = speedMaxima;

            isRange = false;
        }

        if (mode == ModeNPCHenry.Final)
        {
            obNv.speed = 0f;

            isRange = false;

        }
    }

    private void LateUpdate()
    {
        obAnim.SetInteger("Estado", numeroAnim);


    }
}
