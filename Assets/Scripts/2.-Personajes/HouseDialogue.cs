using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    bool escribiendo;
    [SerializeField, TextArea(4, 6)] string consejoFinalA;
    [SerializeField, TextArea(4, 6)] string consejoFinalC;


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

    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta0;
    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta1;
    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta2;
    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta3;
    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta4;
    [SerializeField, TextArea(4, 6)] string[] linesNextCIncorrecta5;
    [SerializeField, TextArea(4, 6)] string[] linesNextCCorrecta;

    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta1;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta2;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta3;
    [SerializeField, TextArea(4, 6)] string[] linesAIncorrecta4;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta1;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta2;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta3;
    [SerializeField, TextArea(4, 6)] string[] linesCIncorrecta4;


    [SerializeField, TextArea(4, 6)] string[] linesAFinal;

    [SerializeField, TextArea(4, 6)] string[] linesCFinal;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal1;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal2;
    [SerializeField, TextArea(4, 6)] string[] linesCFinal3;
    [SerializeField] int random00;
    [SerializeField] int random01;
    int random001;
    int random011;
    int random00c1;
    int random01c1;
    int random00c2;
    int random01c2;

    int random0cf;
    int random1cf;

    int rrc0;
    int rrc1;

    int respuestaC0;
    int respuestaC1;

    [SerializeField] Vector3 masVector;


    [Header("Character Variables")]
    public GameObject obHenry;
    public Image blackScreen;
    public bool optionCBuscarHermano;

    [Header("Final References")]
    public bool henryFinalA;
    public Transform casaGO;

    [Header("Music References")]
    public AudioClip cancion;
    [SerializeField] AudioClip[] voces;
    bool noAbrir = false;

   
    int voz000, voz001;

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


    private void OnEnable()
    {
        marker.SetActive(false);
        respuestaDada = GameObject.Find("NPC_Level_Leahn").GetComponent<NPC_Dialogue>();
        
        trPlayer = GameObject.Find("Player").GetComponent<Transform>();
        //obHenry.SetActive(false);


       
    }

    private void Start()
    {
        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        Options = GameObject.Find("DialogueOptions").gameObject;

        selector = GameObject.Find("Select").gameObject;

        listOptions[0] = GameObject.Find("Panel0").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        listOptions[1] = GameObject.Find("Panel1").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        listOptions[2] = GameObject.Find("Panel2").gameObject.GetComponentInChildren<TextMeshProUGUI>();

    }
    void StartDialogue()
    {
        //UIManager.InstanceGUI.BurbujaDialogo(0);

        MainCharacter.sharedInstance.animIntervalo = 0.0f;

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;


        finishDialogue = false;

        id_selector = 0;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.InstanceGUI.fadeBlack = true;
        UIManager.InstanceGUI.fadeBlackN = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);


        if (respuestaDada.nextDialogueToTalk == 0)
        {

            if (!talkToLeahn)
            {
                DialogoRandom();
                UIManager.InstanceGUI.BurbujaDialogo(4);
            }
            if (talkToLeahn && !nombreIncorrecto )
            {
                lines = linesA;

                UIManager.InstanceGUI.BurbujaDialogo(0);
            }
            if (talkToLeahn && nombreIncorrecto) 
            {
                UIManager.InstanceGUI.BurbujaDialogo(0);

                random001 = random011;

                random011 = Random.Range(0, 5);

                

                while (random011 == random001)
                {
                    random011 = Random.Range(0, 5);
                }


                

                switch (random011)
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
            UIManager.InstanceGUI.BurbujaDialogo(0);

            if (talkToLeahn && !nombreIncorrecto && !optionCBuscarHermano)
            {
                lines = linesC;
            }
            if (talkToLeahn && nombreIncorrecto && !optionCBuscarHermano)
            {
                respuestaC0 = respuestaC1;

                respuestaC1 = Random.Range(0, 5);

                while (respuestaC1 == respuestaC0)
                {
                    respuestaC1 = Random.Range(0, 5);
                }

                if (respuestaC1 == 0)
                {
                    lines = linesCIncorrecta;
                }
                if (respuestaC1 == 1)
                {
                    lines = linesCIncorrecta1;
                }
                if (respuestaC1 == 2)
                {
                    lines = linesCIncorrecta2;
                }
                if (respuestaC1 == 3)
                {
                    lines = linesCIncorrecta3;
                }
                if (respuestaC1 == 4)
                {
                    lines = linesCIncorrecta4;
                }
                



            }

            if (optionCBuscarHermano)
            {
                random0cf = random1cf;

                random1cf = Random.Range(0, 4);

                while (random1cf == random0cf)
                {
                    random1cf = Random.Range(0, 4);
                }

                switch (random1cf)
                {
                    case 0:
                        lines = linesCFinal;
                        break;

                    case 1:
                        lines = linesCFinal1;
                        break;

                    case 2:
                        lines = linesCFinal2;
                        break;

                    case 3:
                        lines = linesCFinal3;
                        break;
                }



                
            }
        }
      
   
        StartCoroutine(WriteDialogue());

    }
    void DialogoRandom()
    {
        random00 = random01;

        random01 = Random.Range(0, 5);

        while (random01 == random00)
        {
            random01 = Random.Range(0, 5);
        }

       

        switch (random01)
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
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position/* + new Vector3(0f,50f,0f)*/);
            UIManager.InstanceGUI.BurbujaDialogo(7);
            MainCharacter.sharedInstance.VozLogan();
            UIManager.InstanceGUI.NombreDialogo("P");


            MainCharacter.sharedInstance.capaObj.layer = 17;
            respuestaDada.henry.capaObj.layer = 20;
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
            UIManager.InstanceGUI.NombreDialogo("L");


            respuestaDada.henry.capaObj.layer = 20;
            MainCharacter.sharedInstance.capaObj.layer = 20;
            respuestaDada.capaObj.layer = 16;
        }

        if (lineas.Trim().StartsWith("H"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(casaGO.position);
            UIManager.InstanceGUI.BurbujaDialogo(8);
            VocesRandom();
            UIManager.InstanceGUI.NombreDialogo("H");

            respuestaDada.henry.capaObj.layer = 19;
            MainCharacter.sharedInstance.capaObj.layer = 20;
        }


    }
    IEnumerator WriteDialogue()
    {
       
        if (index == 0)
        {
            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));


            while (respuestaDada.obCameras.orthographicSize > 3.5f)
            {
                Vector3 direction =  transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);

                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (respuestaDada.speedZoom) * Time.deltaTime);

                respuestaDada.obCameras.orthographicSize -= respuestaDada.speedZoom * Time.deltaTime;

                yield return null;

            }

            UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);
        }

        

        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

       

        IconDialogo(lines[index]);

		if (!respuestaDada.habloconLeahn)
		{
            if (index == 0)
            {
                MainCharacter.sharedInstance.eAnim = 1;

            }


            if (index == 1)
            {

                MainCharacter.sharedInstance.eAnim = 2;


            }

            if (index == 2)
            {
                
                MainCharacter.sharedInstance.eAnim = 3;

            }
		}
		else
		{
			if ((lines == linesA) || (lines == linesAIncorrecta) || (lines == linesAIncorrecta1) || (lines == linesAIncorrecta2) || (lines == linesAIncorrecta3) || (lines == linesAIncorrecta4))
			{
				if (index == 0)
				{
                    UIManager.InstanceGUI.BurbujaDialogo(12);
                    MainCharacter.sharedInstance.eAnim = 1;
                }
                if (index == 1)
                    
                {
                    MainCharacter.sharedInstance.eAnim = 10;
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
                if (index == 2)
                {
                    MainCharacter.sharedInstance.eAnim = 2;
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                }
                
			}

            if (lines == linesC)
            {
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 1;
                    UIManager.InstanceGUI.BurbujaDialogo(1);
                }
                if (index == 1)
                {
                    MainCharacter.sharedInstance.eAnim = 10;
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
                if (index == 2)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }

                if (index == 4)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
            }

			if ((lines == linesCIncorrecta) || (lines == linesCIncorrecta1) || (lines == linesCIncorrecta2) || (lines == linesCIncorrecta3) || (lines == linesCIncorrecta4) )
			{
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 1;
                    UIManager.InstanceGUI.BurbujaDialogo(8);
                }
                if (index == 1)
                {
                    MainCharacter.sharedInstance.eAnim = 10;
                    UIManager.InstanceGUI.BurbujaDialogo(12);
                }
            }


			if ((lines == linesNextAIncorrecta) || (lines == linesNextAIncorrecta1) || (lines == linesNextAIncorrecta2) || (lines == linesNextAIncorrecta3) || (lines == linesNextAIncorrecta4) || (lines == linesNextAIncorrecta5))
			{
                UIManager.InstanceGUI.BurbujaDialogo(8);
                MainCharacter.sharedInstance.eAnim = 2;
            }

            if ((lines == linesNextCIncorrecta0) || (lines == linesNextCIncorrecta1) || (lines == linesNextCIncorrecta2) || (lines == linesNextCIncorrecta3) || (lines == linesNextCIncorrecta4) || (lines == linesNextCIncorrecta5))
            {
                UIManager.InstanceGUI.BurbujaDialogo(13);
                MainCharacter.sharedInstance.eAnim = 2;
            }

			if (lines == linesNextACorrecta)
			{
				if (index == 0)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(4);
                }

                if (index == 1)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                    MainCharacter.sharedInstance.eAnim = 10;
                }

                if (index == 2)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(4);
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                }

            }

			if (lines == linesNextCCorrecta)
			{
                if (index == 0)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                    MainCharacter.sharedInstance.eAnim = 22;
                }
            }

            if (lines == linesCFinal || lines == linesCFinal1 || lines == linesCFinal2 || lines == linesCFinal3)
			{
				if (index == 0)
				{
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                    MainCharacter.sharedInstance.eAnim = 1;
                }

                if (index == 1)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(12);
                    MainCharacter.sharedInstance.eAnim = 2;
                }


                if (index == 2)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                    MainCharacter.sharedInstance.eAnim = 3;
                }

            }


			if (respuestaDada.id_selector == 1)
			{
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 3;

                }


                if (index == 1)
                {

                    MainCharacter.sharedInstance.eAnim = 2;


                }

                if (index == 2)
                {

                    MainCharacter.sharedInstance.eAnim = 1;

                }
            }
			if (lines == linesExtra0 || lines == linesExtra1 || lines == linesExtra2 || lines == linesExtra3 || lines == linesExtra4)
			{
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 3;

                }


                if (index == 1)
                {

                    MainCharacter.sharedInstance.eAnim = 2;


                }

                if (index == 2)
                {

                    MainCharacter.sharedInstance.eAnim = 1;

                }
            }

			
        }
       


        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(respuestaDada.speedText);
            escribiendo = true;
        }


        if (dialogueText.text == lines[index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }

    }
    public void Navegate()
    {

        if (respuestaDada._map.Jugador.BDOWN.WasPressedThisFrame() && id_selector < listOptions.Length - 1)
        {
            id_selector++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        if (respuestaDada._map.Jugador.BUP.WasPressedThisFrame() && id_selector > 0)
        {
            id_selector--;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (respuestaDada._map.Jugador.Interactuar.WasPressedThisFrame() && !UIManager.InstanceGUI.isGameOver && UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1)
        {

            

            switch (id_selector)
            {
                case 0:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);
                    index = 0;

                    break;

                case 1:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);
                    index = 0;

                    break;

                case 2:
                  
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);
                    AudioManager.Instance.PlaySoundBien();
                    index = 0;

                    break;

                default:
                    break;
            }

            UIManager.InstanceGUI.AnimateOptions(false);

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
                    random00c1 = random01c1;

                    random01c1 = Random.Range(0, 3);

                   

                    while (random01c1 == random00c1)
                    {
                        random01c1 = Random.Range(0, 3);
                    }


                    

                    if (random01c1 == 0)
                    {
                        lines = linesNextAIncorrecta;
                    }
                    if (random01c1 == 1)
                    {
                        lines = linesNextAIncorrecta1;
                    }
                    if (random01c1 == 2)
                    {
                        lines = linesNextAIncorrecta2;
                    }
                  

                    nombreIncorrecto = true;
                }
                else if (respuestaDada.nextDialogueToTalk == 2)
                {

                    rrc0 = rrc1;
                    rrc1 = Random.Range(0, 6);



                    while (rrc1 == rrc0)
                    {
                        rrc1 = Random.Range(0, 6);
                    }

                    if (rrc1 == 0)
                    {
                        lines = linesNextCIncorrecta0;
                    }
                    if (rrc1 == 1)
                    {
                        lines = linesNextCIncorrecta1;
                    }
                    if (rrc1 == 2)
                    {
                        lines = linesNextCIncorrecta2;
                    }
                    if (rrc1 == 3)
                    {
                        lines = linesNextCIncorrecta3;
                    }
                    if (rrc1 == 4)
                    {
                        lines = linesNextCIncorrecta4;
                    }
                    if (rrc1 == 5)
                    {
                        lines = linesNextCIncorrecta5;
                    }



                    nombreIncorrecto = true;
                }
               

                break;

            case 1:
                if (respuestaDada.nextDialogueToTalk == 0)
                {
                   
                    
                    random00c2= random01c2;
                        
                    random01c2 = Random.Range(3, 6);

                   

                    while (random01c2 == random00c2)
                    {
                        random01c2 = Random.Range(3, 6);
                    }

                       

                    if (random01c2 == 3)
                    {
                        lines = linesNextAIncorrecta3;
                    }
                    if (random01c2 == 4)
                    {
                        lines = linesNextAIncorrecta4;
                    }
                    if (random01c2 == 5)
                    {
                        lines = linesNextAIncorrecta5;
                    }
                   

                    nombreIncorrecto = true;
                }
                else if (respuestaDada.nextDialogueToTalk == 2)
                {
                    rrc0 = rrc1;
                    rrc1 = Random.Range(0, 6);



                    while (rrc1 == rrc0)
                    {
                        rrc1 = Random.Range(0, 6);
                    }

                    if (rrc1 == 0)
                    {
                        lines = linesNextCIncorrecta0;
                    }
                    if (rrc1 == 1)
                    {
                        lines = linesNextCIncorrecta1;
                    }
                    if (rrc1 == 2)
                    {
                        lines = linesNextCIncorrecta2;
                    }
                    if (rrc1 == 3)
                    {
                        lines = linesNextCIncorrecta3;
                    }
                    if (rrc1 == 4)
                    {
                        lines = linesNextCIncorrecta4;
                    }
                    if (rrc1 == 5)
                    {
                        lines = linesNextCIncorrecta5;
                    }
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

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        index = 0;

        MainCharacter.sharedInstance.eAnim = 0;

        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

        UIManager.InstanceGUI.fadeFromN = true;
        UIManager.InstanceGUI.fadeFrom = true;

        while (respuestaDada.obCameras.orthographicSize < 7.5f)
        {
            respuestaDada.obCameras.orthographicSize += respuestaDada.speedZoom * Time.deltaTime;
            yield return null;

        }

       
            yield return new WaitForSeconds(1.25f);

            if (optionCBuscarHermano)
            {
                didDialogueStart = false;
                Inventory.instance.panelItem.SetActive(true);
                UIManager.InstanceGUI.obMap.SetActive(true);
                UIManager.InstanceGUI.obMapMark.SetActive(true);
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
                        UIManager.InstanceGUI.obMap.SetActive(true);
                        UIManager.InstanceGUI.obMapMark.SetActive(true);
                        marker.SetActive(true);
                        MainCharacter.sharedInstance.canMove = true;

                    if (UIManager.InstanceGUI.isGameOver)
                    {
                        UIManager.InstanceGUI.FinDelJuego();
                        UIManager.InstanceGUI.ConsejoFinal(consejoFinalA);
                        MainCharacter.sharedInstance.canMove = false;
                        MainCharacter.sharedInstance.puedePausar = false;

                        noAbrir = true;
                    }

                    }
                    else
                    {
                        if (talkToLeahn)
                        {


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
                        UIManager.InstanceGUI.obMap.SetActive(true);
                        UIManager.InstanceGUI.obMapMark.SetActive(true);
                        marker.SetActive(true);
                        MainCharacter.sharedInstance.canMove = true;

                    if (UIManager.InstanceGUI.isGameOver)
                    {
                        UIManager.InstanceGUI.FinDelJuego();
                        UIManager.InstanceGUI.ConsejoFinal(consejoFinalC);
                        MainCharacter.sharedInstance.canMove = false;
                        MainCharacter.sharedInstance.puedePausar = false;
                        noAbrir = true;

					}
					else
					{

					}
                    }
                    else if (talkToLeahn)
                    {
                        didDialogueStart = false;
                        Inventory.instance.panelItem.SetActive(true);
                        UIManager.InstanceGUI.obMap.SetActive(true);
                        UIManager.InstanceGUI.obMapMark.SetActive(true);
                        marker.SetActive(true);
                        MainCharacter.sharedInstance.canMove = true;
                        optionCBuscarHermano = true;

                    }
                }

                if (!talkToLeahn)
                {



                    Inventory.instance.panelItem.SetActive(true);
                    UIManager.InstanceGUI.obMap.SetActive(true);
                    UIManager.InstanceGUI.obMapMark.SetActive(true);
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




		if (UIManager.InstanceGUI.isGameOver)
		{

		}
		else
		{
            MainCharacter.sharedInstance.puedePausar = true;
        }

        

        //respuestaDada.capaObj.layer = 20;
        MainCharacter.sharedInstance.capaObj.layer = 17;

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

                StartCoroutine(Esperar());
                index = 0;

            }
            else
            {


                StartCoroutine(CloseDialogue());
            }
         
        }


    }
    IEnumerator Esperar()
    {

        FollowCameras.instance.OnConfetis(0);

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



        respuestaDada._map.Disable();
        yield return new WaitForSeconds(1f);
        UIManager.InstanceGUI.AnimateOptions(true);
       
        yield return new WaitForSeconds(1f);



        //Options.SetActive(true);

    


        yield return new WaitForSeconds(0.5f);
        respuestaDada._map.Enable();
    }
    IEnumerator IniciarTransicion()
    {

        UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");
        
        yield return new WaitForSeconds(1f);

        obHenry.SetActive(true);
        obHenry.transform.position = new Vector3(95.0f, obHenry.transform.position.y, 110.0f);

        respuestaDada.trPlayer.transform.position = new Vector3(90.0f, respuestaDada.trPlayer.transform.position.y, 100.0f);

        yield return new WaitForSeconds(1f);

       

        MainCharacter.sharedInstance.canMove = true;
        henryFinalA = true;

        //condicion
        if (respuestaDada.rana.gameObject.activeInHierarchy)
        {
            respuestaDada.rana.DesaparecerRana(true);
        }
        

        UIManager.InstanceGUI.ShowHUDInGame();

        
    }

 
    void Update()
    {
		if (!UIManager.InstanceGUI.obCartelPausa.activeInHierarchy)
		{
            if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 && talkToLeahn && !respuestaDada.didDialogueStart && (!respuestaDada.rana.didDialogueStart || !respuestaDada.rana.gameObject.activeInHierarchy))
            {

                Navegate();
            }

            if (isRange && respuestaDada._map.Jugador.Interactuar.WasPressedThisFrame() && !noAbrir)
            {


                if (!didDialogueStart && Inventory.instance.moverInv)
                {
                    StartDialogue();
                }

                else if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 0)
                {
                    if (dialogueText.text == lines[index].Substring(1))
                    {
                        NextDialogue();

                        UIManager.InstanceGUI.icono.gameObject.SetActive(false);
                        AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);
                    }

                }




            }

            if (respuestaDada._map.Jugador.SaltarEscena.WasPressedThisFrame() && escribiendo)
            {

                if (dialogueText.text == lines[index].Substring(1))
                {

                }
                else
                {
                    escribiendo = false;
                    StopAllCoroutines();
                    dialogueText.text = lines[index].Substring(1);
                    UIManager.InstanceGUI.icono.gameObject.SetActive(true);
                }


            }
        }
     
        







    }
    private void OnTriggerEnter(Collider other)
    {      
            if (other.gameObject.CompareTag("P1") && !didDialogueStart)
            {   
                isRange = !isRange;
                marker.SetActive(true);

            MainCharacter.sharedInstance.puedePausar = false;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1") /*&& !didDialogueStart*/)
        {
            isRange = !isRange;
            marker.SetActive(false);

            MainCharacter.sharedInstance.puedePausar = true;
        }
    }
}
