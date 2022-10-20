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
    [SerializeField, TextArea(4, 6)] string[] linesB1;
    [SerializeField, TextArea(4, 6)] string[] linesB2;
    [SerializeField, TextArea(4, 6)] string[] linesB3;

    [SerializeField, TextArea(4, 6)] string[] linesC;

    [SerializeField, TextArea(4, 6)] string[] linesNext;


    [TextArea(4, 6)]public string[] linesNextA;

    [TextArea(4, 6)] public string[] linesListNextA;
  
    [TextArea(4, 6)]public string[] linesNextB;
    [TextArea(4, 6)] public string[] linesNextB1;
    [TextArea(4, 6)] public string[] linesNextB2;
    [TextArea(4, 6)] public string[] linesNextB3;
    [TextArea(4, 6)] public string[] linesNextB4;
    [TextArea(4, 6)] public string[] linesNextC;

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
                dObject.random00 = Random.Range(0, 4);

                dObject.random01 = dObject.random00;

                while (dObject.random01 == dObject.random00)
                {
                    dObject.random00 = Random.Range(0, 4);
                }
                if (dObject.random00 == 0)
                {
                    linesNext = linesB;
                }
                if (dObject.random00 == 1)
                {
                    linesNext = linesB1;
                }
                if (dObject.random00 == 2)
                {
                    linesNext = linesB2;
                }
                if (dObject.random00 == 3)
                {
                    linesNext = linesB3;
                }
              
                
               
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

			if (linesNext == linesA /*|| linesNext == linesA1 || linesNext == linesA2 || linesNext == linesA3*/)
			{
                dObject.numeroAnim = 10;
            }

            if (linesNext == linesB || linesNext == linesB1 || linesNext == linesB2 || linesNext == linesB3)
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


        dObject.IconDialogo(linesNext[dObject.index]);

        foreach (char letter in linesNext[dObject.index].Substring(1).ToCharArray())
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

        if (Input.GetButtonDown("Interactuar") && fillDialogueLines)
        {

            if (dialogueText.text == linesNext[dObject.index].Substring(1))
            {
                NextDialogue();
            }

            UIManager.instance.icono.gameObject.SetActive(false);
        }

    }
}
