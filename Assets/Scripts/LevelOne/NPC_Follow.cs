using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class NPC_Follow : MonoBehaviour
{
    
    [Header("Routes Dialogue Variables")]
    public NPC_Dialogue dObject;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA;
    [SerializeField, TextArea(4, 6)] string[] linesB;
    [SerializeField, TextArea(4, 6)] string[] linesC;

    [SerializeField, TextArea(4, 6)] string[] linesNext;


    [TextArea(4, 6)]public string[] linesNextA;
    [TextArea(4, 6)]public string[] linesNextB;
    [TextArea(4, 6)]public string[] linesNextC;

    [TextArea(4, 6)] public string[] linesNextC2;

    bool fillDialogueLines; 

    public void StarRoute(sbyte numeroRoute)
    {
        dObject.nextRoute = true;

        dObject.Options.SetActive(false);

        switch (numeroRoute)
        {
            case 0:
                linesNext = linesA;
                
                break;

            case 1:
                linesNext = linesB;
               
                break;

            case 2:
                linesNext = linesC;
               
                break;

                
            default:
                break;
        }

        fillDialogueLines = true;

        StartCoroutine(ContinueWriteDialogue());



    }
    public IEnumerator ContinueWriteDialogue()
    {

        if (dObject.index == 1)
        {

			if (linesNext == linesA)
			{
                dObject.numeroAnim = 10;
            }

            if (linesNext == linesB)
            {

                dObject.numeroAnim = 11;
            }

            if (linesNext == linesC)
            {

                dObject.numeroAnim = 12;
            }
           
        }

        if (dObject.index == 3)
        {

            if (linesNext == linesA)
            {
                dObject.numeroAnim = 15;
            }


        }

        if (dObject.index == 4)
        {

            if (linesNext == linesA)
            {
                dObject.numeroAnim = 16;
            }


        }


        dialogueText.text = string.Empty;


        foreach (char letter in linesNext[dObject.index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dObject.speedText);
        }

        UIManager.instance.icono.gameObject.SetActive(true);



    }

    void NextDialogue()
    {
        dObject.index++;

        if ((dObject.index < linesNext.Length))
        {
            StartCoroutine(ContinueWriteDialogue());
        }
        else
        {
            fillDialogueLines = false;
            StartCoroutine(dObject.CloseDialogue());

        }


    }



    private void Update()
    {
        //if (dialogueText.text == linesNext[dObject.index])
        //{
        //    UIManager.instance.icono.gameObject.SetActive(true);
        //}
        //else
        //{
        //    UIManager.instance.icono.gameObject.SetActive(false);
        //}


        if (Input.GetButtonDown("Interactuar") && fillDialogueLines)
        {

            if (dialogueText.text == linesNext[dObject.index])
            {
                NextDialogue();
            }

            UIManager.instance.icono.gameObject.SetActive(false);
        }

    }
}
