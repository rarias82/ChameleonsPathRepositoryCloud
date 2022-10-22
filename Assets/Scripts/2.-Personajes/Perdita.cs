using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Perdita : MonoBehaviour
{
    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA;
    [SerializeField, TextArea(4, 6)] string[] linesB;
    [SerializeField, TextArea(4, 6)] string[] linesC;
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

    [Header("Options References")]
    public GameObject Options;
    [SerializeField] TextMeshProUGUI[] listOptions;
    [SerializeField] GameObject selector;
    public int id_selector;
    [SerializeField] string[] optionLines;



    public enum ModeNPC
    {
        Help, Concerned, Busy, Walking, House, Final, Follow
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
            Options.SetActive(true);

            for (int i = 0; i < listOptions.Length; i++)
            {
                listOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLines[i];
            }
            

        }


    }

    public IEnumerator CloseDialogue()
    {

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

    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }

    public void Navegate()
    {

        if (Input.GetButtonDown("Abajo") && id_selector < listOptions.Length - 1)
        {
            id_selector++;
        }

        if (Input.GetButtonDown("Arriba") && id_selector > 0)
        {
            id_selector--;
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (Input.GetButtonDown("Interactuar"))
        {


            switch (id_selector)
            {
                case 0:

                    UIManager.instance.GanarPuntos(false, UIManager.instance.puntos);


                    break;

                case 1:

                    UIManager.instance.GanarPuntos(false, UIManager.instance.puntos);


                    break;

                case 2:

                    UIManager.instance.GanarPuntos(true, UIManager.instance.puntos);

                    break;

                default:
                    break;
            }

            Options.SetActive(false);

			StartCoroutine(ChangeDialogue());


		}


    }
    // Start is called before the first frame update
    void Start()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;
        
    }

    IEnumerator ChangeDialogue()
    {
        index = 0;
        
        switch (id_selector)
        {
            case 0:

                lines = linesA;

                break;

            case 1:

                lines = linesB;

                break;

            case 2:
                
                lines = linesC;

                break;


            default:
                break;
        }

     

        yield return null;

        StartCoroutine(WriteDialogue());




    }

    // Update is called once per frame
    void Update()
    {
        if (mode == ModeNPC.Walking)
        {
            Walking();
        }

        if (mode == ModeNPC.Concerned)
        {

            Vector3 direction = trPlayer.transform.position - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);//Mira al jugador

            if (Options.activeInHierarchy)
            {

                Navegate();
            }



            if (isRange && Input.GetButtonDown("Interactuar") && Inventory.instance.moverInv && !Options.activeInHierarchy) // Pregunta si puede interactuar
            {
                if (!didDialogueStart)
                {
                    StartDialogue();
                }

                else if (dialogueText.text == lines[index])
                {                   
   
                    NextDialogue();

                }

                UIManager.instance.icono.gameObject.SetActive(false);


            }

            RotateSon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1") )
        {
            isRange = !isRange;

            if (mode == ModeNPC.Walking)
			{
                obNMA.speed = 0;
                mode = ModeNPC.Concerned;
                marker.SetActive(true);
            }
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            isRange = !isRange;

            if (mode == ModeNPC.Concerned)
            {
                obNMA.speed = speedNPC;
                mode = ModeNPC.Walking;
                marker.SetActive(false);
            }

        }
    }
}
