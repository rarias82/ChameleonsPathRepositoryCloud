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
    public int index;
    public bool didDialogueStart;
    public bool talkToLeahn;
    public bool nombreIncorrecto;
    //public bool toNextRoute;

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
    [SerializeField, TextArea(4, 6)] string[] linesNextACorrecta;

    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesNextCCorrecta;

    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta;


    [SerializeField, TextArea(4, 6)] string[] linesAFinal;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal;

    [Header("Character Variables")]
    public GameObject obHenry;
    public Image blackScreen;
    public bool optionCBuscarHermano;
    public bool falloTiempo;


    void Awake()
    {
        marker = transform.Find("MarkerCasa").gameObject;
        marker.SetActive(false);
        respuestaDada = GameObject.Find("NPC_Level_Leahn").GetComponent<NPC_Dialogue>();
        henry = FindObjectOfType<NPC_Henry>(); 
        trPlayer = GameObject.Find("Player").GetComponent<Transform>();
        obHenry.SetActive(false);
        
    }

    void StartDialogue()
    {

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;


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
            if (talkToLeahn && !nombreIncorrecto && !falloTiempo)
            {
                lines = linesA;
            }
            if (talkToLeahn && nombreIncorrecto && !falloTiempo)
            {
                lines = linesAIncorrecta;
            }

            if (talkToLeahn && falloTiempo)
            {
                lines = linesAFinal;
            }
           
        }

        if (respuestaDada.nextDialogueToTalk == 2)
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

    IEnumerator WriteDialogue()
    {
       
        if (index == 0)
        {

            while (respuestaDada.obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction =  transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);

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
                    lines = linesNextAIncorrecta;
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
                    lines = linesNextAIncorrecta;
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

                        didDialogueStart = true;
                        StartCoroutine(FadeinOut());
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

                if (!falloTiempo)
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
                    didDialogueStart = true;
                    StartCoroutine(FadeinOut());
                }
                
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
            if (talkToLeahn && !finishDialogue && !optionCBuscarHermano && !falloTiempo)
            {
                //nextRoute = true;
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

    IEnumerator FadeinOut()
    {
        while (blackScreen.color.a < 1f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 1f, 1.75f * Time.deltaTime));
            yield return null;
        }

        trPlayer.position = new Vector3(99.5f, 6.079084f, 104.8f);
        obHenry.SetActive(true);
        obHenry.transform.position = henry.posInicial;
        yield return new WaitForSeconds(0.5f);
        

        
        while (blackScreen.color.a > 0f)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, 1.75f * Time.deltaTime));
            yield return null;
        }

        obHenry.GetComponent<NPC_Henry>().mode = ModeNPCHenry.Follow;
        Inventory.instance.panelItem.SetActive(true);
        UIManager.instance.obMap.SetActive(true);
        UIManager.instance.obMapMark.SetActive(true);
        marker.SetActive(false);
        MainCharacter.sharedInstance.canMove = true;

        obHenry.GetComponent<NPC_Henry>().tiempoesperando = true;
        obHenry.GetComponent<NPC_Henry>().contador = 0;
    }



    // Update is called once per frame
    void Update()
    {

        //if (dialogueText.text == lines[index])
        //{
        //    UIManager.instance.icono.gameObject.SetActive(true);
        //}
        //else
        //{
        //    UIManager.instance.icono.gameObject.SetActive(false);
        //}

        if (Options.activeInHierarchy && talkToLeahn && !respuestaDada.didDialogueStart)
        {

            Navegate();
        }

        if (isRange && Input.GetButtonDown("Interactuar"))
        {


            if (!didDialogueStart && Inventory.instance.moverInv && !henry.finalBueno)
            {
                StartDialogue();
            }

            else if(!Options.activeInHierarchy)
            {
                if (dialogueText.text == lines[index])
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

            if (henry.tiempoesperando)
            {
                marker.SetActive(false);
            }
            else
            {
                marker.SetActive(true);
            }


            if (henry.finalBueno || henry.finalMalo )
            {
                marker.SetActive(false);

                isRange = false;
            }
                
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
