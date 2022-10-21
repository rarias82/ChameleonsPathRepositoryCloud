using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    NPC_Dialogue respuestaDada;
    NPC_Henry henry;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA;
    [SerializeField, TextArea(4, 6)] string[] linesC;
    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string[] linesExtra0;
    [SerializeField, TextArea(4, 6)] string[] linesExtra1;
    [SerializeField, TextArea(4, 6)] string[] linesExtra2;
    [SerializeField, TextArea(4, 6)] string[] linesExtra3;
    [SerializeField, TextArea(4, 6)] string[] linesExtra4;
    public int index;
    public bool didDialogueStart;
    public bool talkToLeahn;
    public bool nombreIncorrecto;
    

    [Header("Options References")]
    public GameObject Options;
    [SerializeField] TextMeshProUGUI[] listOptions;
    [SerializeField] GameObject selector;
    [SerializeField] sbyte id_selector;
    [SerializeField] string[] optionLinesA;
    [SerializeField] string[] optionLinesC;
    
    Transform trPlayer;
    public bool finishDialogue;

    [Header("Dialogue Next")]
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta1;
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta2;
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta3;
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta4;
    [SerializeField, TextArea(4, 6)] string[] linesNextAIncorrecta5;
    [SerializeField, TextArea(4, 6)] string[] linesNextACorrecta;

    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesNextCCorrecta;

    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta1;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta2;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta3;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta4;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta;


    [SerializeField, TextArea(4, 6)] string[] linesAFinal;

    [SerializeField, TextArea(4, 6)] string[] linesCFinal;
    [SerializeField] int random00;
    [SerializeField] int random01;
    int random001;
    int random011;
    int random00c1;
    int random01c1;
    int random00c2;
    int random01c2;
    [SerializeField] Vector3 masVector;


    [Header("Character Variables")]
    public GameObject obHenry;
    public Image blackScreen;
    public bool optionCBuscarHermano;
    
    public Transform casaGO;


    void Awake()
    {
       
        marker.SetActive(false);
        respuestaDada = GameObject.Find("NPC_Level_Leahn").GetComponent<NPC_Dialogue>();
        henry = FindObjectOfType<NPC_Henry>(); 
        trPlayer = GameObject.Find("Player").GetComponent<Transform>();
        //obHenry.SetActive(false);
        
    }

    void StartDialogue()
    {

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;


        finishDialogue = false;

        id_selector = 0;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.instance.fadeBlack = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.instance.obMap.SetActive(false);
        UIManager.instance.obMapMark.SetActive(false);


        if (respuestaDada.nextDialogueToTalk == 0)
        {

            if (!talkToLeahn)
            {
                DialogoRandom();
            }
            if (talkToLeahn && !nombreIncorrecto )
            {
                lines = linesA;
            }
            if (talkToLeahn && nombreIncorrecto) 
            {
                random001 = Random.Range(0, 5);

                

                while (random011 == random001)
                {
                    random001 = Random.Range(0, 5);
                }


                random011 = random001;

                switch (random001)
                {
                    case 0:
                        lines = linesAIncorrecta;
                        break;

                    case 1:
                        lines = linesAIncorrecta1;
                        break;

                    case 2:
                        lines = linesAIncorrecta2;
                        break;

                    case 3:
                        lines = linesAIncorrecta3;
                        break;

                    case 4:
                        lines = linesAIncorrecta4;
                        break;

                    default:
                        break;
                }
                

            }

           

        }
        else if (respuestaDada.nextDialogueToTalk == 2)
        {
            if (talkToLeahn && !nombreIncorrecto && !optionCBuscarHermano)
            {
                lines = linesC;
            }
            if (talkToLeahn && nombreIncorrecto && !optionCBuscarHermano)
            {
                lines = linesCIncorrecta;
            }

            if (optionCBuscarHermano)
            {
                lines = linesCFinal;
            }
        }
      
   
        StartCoroutine(WriteDialogue());

    }


    void DialogoRandom()
    {
        random00 = Random.Range(0, 5);

        

        while (random01 == random00)
        {
            random00 = Random.Range(0, 5);
        }

        random01 = random00;

        switch (random00)
        {
            case 0:
                lines = linesExtra0;
                break;

            case 1:
                lines = linesExtra1;
                break;

            case 2:
                lines = linesExtra2;
                break;

            case 3:
                lines = linesExtra3;
                break;

            case 4:
                lines = linesExtra4;
                break;

            default:
                break;
        }



    }

    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.instance.PosicionarGlobo(trPlayer.position/* + new Vector3(0f,50f,0f)*/);
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.instance.PosicionarGlobo(transform.position);
        }

        if (lineas.Trim().StartsWith("H"))
        {
            UIManager.instance.PosicionarGlobo(casaGO.position);

        }


    }


    IEnumerator WriteDialogue()
    {
       
        if (index == 0)
        {

            while (respuestaDada.obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction =  transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);

                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (respuestaDada.speedZoom) * Time.deltaTime);

                respuestaDada.obCameras.orthographicSize -= respuestaDada.speedZoom * Time.deltaTime;

                yield return null;

            }

            UIManager.instance.ballonDialogue.gameObject.SetActive(true);
        }

        dialogueText.text = string.Empty;


        IconDialogo(lines[index]);


        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(respuestaDada.speedText);
        }


        UIManager.instance.icono.gameObject.SetActive(true);

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

    IEnumerator ChangeDialogue()
    {
        index = 0;
        switch (id_selector)
        {
            case 0:

                if (respuestaDada.nextDialogueToTalk == 0)
                {

                    random00c1 = Random.Range(0, 3);

                    random01c1 = random00c1;

                    while (random01c1 == random00c1)
                    {
                        random00c1 = Random.Range(0, 3);
                    }

                    if (random00c1 == 0)
                    {
                        lines = linesNextAIncorrecta;
                    }
                    if (random00c1 == 1)
                    {
                        lines = linesNextAIncorrecta1;
                    }
                    if (random00c1 == 2)
                    {
                        lines = linesNextAIncorrecta2;
                    }
                  

                    nombreIncorrecto = true;
                }
                else if (respuestaDada.nextDialogueToTalk == 2)
                {
                    lines = linesNextCIncorrecta;
                    nombreIncorrecto = true;
                }
               

                break;

            case 1:
                if (respuestaDada.nextDialogueToTalk == 0)
                {
                    random00c2 = Random.Range(3, 6);

                    random01c2 = random00c2;

                    while (random01c2 == random00c2)
                    {
                        random00c2 = Random.Range(3, 6);
                    }

                    if (random00c2 == 3)
                    {
                        lines = linesNextAIncorrecta3;
                    }
                    if (random00c2 == 4)
                    {
                        lines = linesNextAIncorrecta4;
                    }
                    if (random00c2 == 5)
                    {
                        lines = linesNextAIncorrecta5;
                    }
                   

                    nombreIncorrecto = true;
                }
                else if (respuestaDada.nextDialogueToTalk == 2)
                {
                    lines = linesNextCIncorrecta;
                    nombreIncorrecto = true;
                }
                break;

            case 2:
                if (respuestaDada.nextDialogueToTalk == 0)
                {
                    lines = linesNextACorrecta;
                    nombreIncorrecto = false;
                }
                else if (respuestaDada.nextDialogueToTalk == 2)
                {
                    lines = linesNextCCorrecta;
                    nombreIncorrecto = false;
                }
                break;


            default:
                break;
        }

        finishDialogue = true;

        yield return null;

        StartCoroutine(WriteDialogue());

        
            
            
    }
    IEnumerator CloseDialogue()
    {


        UIManager.instance.ballonDialogue.gameObject.SetActive(false);

        index = 0;

        dialogueText.text = string.Empty;

        UIManager.instance.fadeFrom = true;

        while (respuestaDada.obCameras.orthographicSize < 7.5f)
        {
            respuestaDada.obCameras.orthographicSize += respuestaDada.speedZoom * Time.deltaTime;
            yield return null;

        }

        if (optionCBuscarHermano)
        {
            didDialogueStart = false;
            Inventory.instance.panelItem.SetActive(true);
            UIManager.instance.obMap.SetActive(true);
            UIManager.instance.obMapMark.SetActive(true);
            marker.SetActive(true);
            MainCharacter.sharedInstance.canMove = true;
        }
        else
        {
            if (respuestaDada.nextDialogueToTalk == 0)
            {
                if (nombreIncorrecto)
                {
                    didDialogueStart = false;
                    Inventory.instance.panelItem.SetActive(true);
                    UIManager.instance.obMap.SetActive(true);
                    UIManager.instance.obMapMark.SetActive(true);
                    marker.SetActive(true);
                    MainCharacter.sharedInstance.canMove = true;
                }
                else 
                {
                    if (talkToLeahn){

                        
                        StartCoroutine("IniciarTransicion");
                    }
                        


                }

            }
            if (respuestaDada.nextDialogueToTalk == 2)
            {
                if (nombreIncorrecto)
                {
                    didDialogueStart = false;
                    Inventory.instance.panelItem.SetActive(true);
                    UIManager.instance.obMap.SetActive(true);
                    UIManager.instance.obMapMark.SetActive(true);
                    marker.SetActive(true);
                    MainCharacter.sharedInstance.canMove = true;
                }
                else if (talkToLeahn)
                {
                    didDialogueStart = false;
                    Inventory.instance.panelItem.SetActive(true);
                    UIManager.instance.obMap.SetActive(true);
                    UIManager.instance.obMapMark.SetActive(true);
                    marker.SetActive(true);
                    MainCharacter.sharedInstance.canMove = true;
                    optionCBuscarHermano = true;

                }
            }

            if (!talkToLeahn)
            {

                
                    
                    Inventory.instance.panelItem.SetActive(true);
                    UIManager.instance.obMap.SetActive(true);
                    UIManager.instance.obMapMark.SetActive(true);
                    marker.SetActive(true);
                    MainCharacter.sharedInstance.canMove = true;
                    didDialogueStart = false;
                //didDialogueStart = true;
                //StartCoroutine(FadeinOut());


            }
            else
            {

            }
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
            if (talkToLeahn && !finishDialogue && !optionCBuscarHermano)
            {
               
                Options.SetActive(true);
            
                if (respuestaDada.nextDialogueToTalk == 0)
                {
                    for (int i = 0; i < listOptions.Length; i++)
                    {
                        listOptions[i].text = optionLinesA[i];
                    }
                }

                if (respuestaDada.nextDialogueToTalk == 2)
                {
                    for (int i = 0; i < listOptions.Length; i++)
                    {
                        listOptions[i].text = optionLinesC[i];
                    }
                }
            }
            else
            {


                StartCoroutine(CloseDialogue());
            }
         
        }


    }

    IEnumerator IniciarTransicion()
    {

        UIManager.instance.obAnim.SetTrigger("Start");
        
        yield return new WaitForSeconds(1f);

        obHenry.SetActive(true);
        obHenry.transform.position = new Vector3(95.0f, obHenry.transform.position.y, 110.0f);

        respuestaDada.trPlayer.transform.position = new Vector3(90.0f, respuestaDada.trPlayer.transform.position.y, 100.0f);

        yield return new WaitForSeconds(1f);

        UIManager.instance.obAnim.SetTrigger("End");

        MainCharacter.sharedInstance.canMove = true;


        
    }




    // Update is called once per frame
    void Update()
    {

     
        if (Options.activeInHierarchy && talkToLeahn && !respuestaDada.didDialogueStart)
        {

            Navegate();
        }

        if (isRange && Input.GetButtonDown("Interactuar"))
        {


            if (!didDialogueStart && Inventory.instance.moverInv)
            {
                StartDialogue();
            }

            else if(!Options.activeInHierarchy)
            {
                if (dialogueText.text == lines[index].Substring(1))
                {
                    NextDialogue();
                }

            }

            UIManager.instance.icono.gameObject.SetActive(false);


        }




        

    }

    private void OnTriggerEnter(Collider other)
    {      
            if (other.gameObject.CompareTag("P1") && !didDialogueStart)
            {   
                isRange = !isRange;
                marker.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1") /*&& !didDialogueStart*/)
        {
            isRange = !isRange;
            marker.SetActive(false);
        }
    }
}
