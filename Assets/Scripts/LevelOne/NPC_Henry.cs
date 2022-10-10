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
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;


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

            if (dialogueText.text == lines[index].Substring(1))
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
    public void StartDialogue()
    {
        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;


        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.instance.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.instance.obMap.SetActive(false);
        UIManager.instance.obMapMark.SetActive(false);


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

        dialogueText.text = string.Empty;

        foreach (char letter in lines[index].ToCharArray())
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

    }


    private void Update()
    {
        Interactuar();
            
    }

    private void OnTriggerEnter(Collider other)
    {   
            if (other.gameObject.CompareTag("P1"))
            {  
                obNMA.speed = 0;
                isRange = !isRange;
                marker.SetActive(true);
                
                

            }
    }

    private void OnTriggerExit(Collider other)
    {
     
            if (other.gameObject.CompareTag("P1"))
            {

                    obNMA.speed = 0;
                    isRange = !isRange;
                    marker.SetActive(false);

            }
              


            
    }

      


}
