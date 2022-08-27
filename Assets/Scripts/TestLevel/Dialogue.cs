using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;

    //Referencias al UI
    [Header("UI References")]
    public TMP_Text GUI_text;
    public TMP_Text[] GUI_listOptions;
    public GameObject panel;
    public sbyte optionSelected;

    [Header("Local References")]
    public bool isPlayerInRange;
    [TextArea(4, 6)]
    public string[] dialogue;
    public string[] dialogueOptions;
    public bool dialogueStart, canPressButton;
    int lineIndex;
    public float timeType;
    public GameObject dialogueMark;
    public GameObject Options;
    public string[] listNames;
    public float speedMovImage;
    [TextArea(4, 6)]
    public string[] dialogueRoutes;

    private void Awake()
    {


    }
    void StartDialogue()
    {
        PlayerOne.sharedInstance.canMove = false;
        FollowCameras.instance.mode = Modo.InDialogue;

        dialogueMark.SetActive(false);
         
        dialogueStart = true;
        
        UIManager.instance.fadeBlack = true;
        lineIndex = 0;

        StartCoroutine("ShowLine");
    }

    IEnumerator ShowLine()
    {
        if (lineIndex > 0)
        {
            char[] letra = dialogue[optionSelected].ToCharArray();
        }


        if (lineIndex == 0)
        {
            char[] letra = dialogue[lineIndex].ToCharArray();
        }

        GUI_text.text = string.Empty;
        
        if (lineIndex==0)
        {
            while (obCameras.orthographicSize > 3.5f)
            {
                obCameras.orthographicSize -= speedZoom * Time.deltaTime;
                yield return null;

            }
        }

      

        foreach (char ch in dialogue[lineIndex].Substring(1))
        {
            canPressButton = true;
            GUI_text.text += ch;
            yield return new WaitForSecondsRealtime(timeType);
        }

        if (GUI_text.text == dialogue[lineIndex].Substring(1) && lineIndex == 2)
        {
            Options.SetActive(true);

        }
    }
    void NextDialogue()
    {
        Options.SetActive(false);

        lineIndex++;

        if (lineIndex < dialogue.Length)
        {
            StartCoroutine("ShowLine");
        }
        else
        {
            StartCoroutine("Final");
        }

    }

    private void Start()
    {
        
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            optionSelected = 0;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            optionSelected = 1;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            optionSelected = 2;
        }


        if (isPlayerInRange && Input.GetButtonDown("Jump"))
        {

            if (!dialogueStart)
            {
                StartDialogue();
            }
            else if (GUI_text.text == dialogue[lineIndex].Substring(1))
            {



                switch (optionSelected)
                {
                    case 0:
                        dialogue[3] = dialogueRoutes[optionSelected].Substring(1);
                        break;

                    case 1:
                        dialogue[3] = dialogueRoutes[optionSelected].Substring(1);
                        break;

                    case 2:
                        dialogue[3] = dialogueRoutes[optionSelected].Substring(1);
                        break;

                    default:

                        break;
                }



                NextDialogue();
            }
            else if (canPressButton)
            {

                StopAllCoroutines();
                GUI_text.text = dialogue[lineIndex].Substring(1);

            }

        }

    }

    IEnumerator Final()
    {
        canPressButton = false;
        UIManager.instance.fadeFrom = true;
        GUI_text.text = string.Empty;

        while (obCameras.orthographicSize < 7.5f)
        {
            obCameras.orthographicSize += speedZoom * Time.deltaTime;
            yield return null;

        }

        yield return null;

        dialogueStart = false;
        dialogueMark.SetActive(true);
        PlayerOne.sharedInstance.canMove = true;
        FollowCameras.instance.mode = Modo.InGame;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            dialogueMark.SetActive(true);
            isPlayerInRange = true;

          
            for (int i = 0; i < GUI_listOptions.Length; i++)
            {
            GUI_listOptions[i].text = dialogueOptions[i];
            }

        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {
            dialogueMark.SetActive(false);
            isPlayerInRange = false;
        }
    }
}