using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [TextArea(4, 6)] public string[] linesNextA1;
    [TextArea(4, 6)] public string[] linesNextA2;
    [TextArea(4, 6)] public string[] linesNextA3;
    [TextArea(4, 6)] public string[] linesNextA4;


    [TextArea(4, 6)] public string[] linesListNextA;
  
    [TextArea(4, 6)]public string[] linesNextB;
    [TextArea(4, 6)] public string[] linesNextB1;
    [TextArea(4, 6)] public string[] linesNextB2;
    [TextArea(4, 6)] public string[] linesNextB3;
    [TextArea(4, 6)] public string[] linesNextB4;
    [TextArea(4, 6)] public string[] linesNextC0;
    [TextArea(4, 6)] public string[] linesNextC01;
    [TextArea(4, 6)] public string[] linesNextC02;
    [TextArea(4, 6)] public string[] linesNextC03;
    [TextArea(4, 6)] public string[] linesNextC04;

    [TextArea(4, 6)] public string[] linesNextC2;

    [TextArea(4, 6)] public string[] linesFinalC;
    [TextArea(4, 6)] public string[] linesFinalC1;
    [TextArea(4, 6)] public string[] linesFinalC2;
    [TextArea(4, 6)] public string[] linesFinalC3;
    int random00;
    int random01;

    bool fillDialogueLines;




    private void OnEnable()
    {
        
    }

    private void Start()
    {
        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();
    }
    public void StarRoute(sbyte numeroRoute)
    {
        dObject.index = 0;

        dObject.nextRoute = true;

        UIManager.InstanceGUI.AnimateOptions(false);

        switch (numeroRoute)
        {
            case 0:
                linesNext = linesA;
                
                break;

            case 1:

               
				random00 = random01;

				random01 = Random.Range(0, 4);


				while (random01 == random00)
				{
					random01 = Random.Range(0, 4);
				}



				if (random01 == 0)
				{
					linesNext = linesB;
				}
				if (random01 == 1)
				{
					linesNext = linesB1;
				}
				if (random01 == 2)
				{
					linesNext = linesB2;
				}
				if (random01 == 3)
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
        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);




      


        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

        if (dObject.index == 0)
        {
            if (dObject.id_selector == 1 || dObject.id_selector == 2)
            {
                yield return new WaitForSeconds(UIManager.InstanceGUI.timeTransicion);
                Debug.Log("Mas facil");
                FollowCameras.instance.mode = Modo.Mundo;
                FollowCameras.instance.velocidadRotacion = -75.0f;


                MainCharacter.sharedInstance.eAnim = 22;
            }



        }

        if (dObject.index > 0)
        {
            GiroDeCamara();
        }

        
		while (FollowCameras.instance.mode == Modo.Mundo)
		{
			UIManager.InstanceGUI.BurbujaDialogo(9);
			yield return null;
		}

		while (FollowCameras.instance.mode == Modo.Odnum)
		{
			UIManager.InstanceGUI.BurbujaDialogo(9);
			yield return null;
		}



        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

     
        
        IconDialogoS(linesNext[dObject.index]);

        if (dObject.index == 0)
        {
            if (dObject.nextDialogueToTalk == 0)
            {
                UIManager.InstanceGUI.BurbujaDialogo(0);
            }

            if (dObject.nextDialogueToTalk == 1)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
            }

            if (dObject.nextDialogueToTalk == 2)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
            }

        }

        if (dObject.index == 1)
        {
            if (linesNext == linesA)
            {
                dObject.numeroAnim = 10;

                UIManager.InstanceGUI.BurbujaDialogo(5);
            }

            if (linesNext == linesB || linesNext == linesB1 || linesNext == linesB2 || linesNext == linesB3)
            {



                dObject.numeroAnim = 11;

                UIManager.InstanceGUI.BurbujaDialogo(6);
            }

            if (linesNext == linesC)
            {

                dObject.numeroAnim = 12;

                UIManager.InstanceGUI.BurbujaDialogo(6);
            }


            if (dObject.nextDialogueToTalk == 0)
            {
                UIManager.InstanceGUI.BurbujaDialogo(5);
            }
            if (dObject.nextDialogueToTalk == 1)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
            }

            if (dObject.nextDialogueToTalk == 2)
            {
                UIManager.InstanceGUI.BurbujaDialogo(2);
            }


        }

        if (dObject.index == 2)
        {
            if (dObject.nextDialogueToTalk == 0)
            {
                UIManager.InstanceGUI.BurbujaDialogo(0);
            }
            MainCharacter.sharedInstance.eAnim = 23;

        }

        if (dObject.index == 3)
        {
            if (dObject.nextDialogueToTalk == 0)
            {
                UIManager.InstanceGUI.BurbujaDialogo(5);
            }
            if (linesNext == linesA)
            {
                dObject.numeroAnim = 15;

                UIManager.InstanceGUI.BurbujaDialogo(7);
            }


        }

        if (dObject.index == 4)
        {

            if (linesNext == linesA)
            {
                dObject.numeroAnim = 16;
            }


        }

        foreach (char letter in linesNext[dObject.index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dObject.speedText);
        }

        if (dialogueText.text == linesNext[dObject.index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }
        



    }
    public void GiroDeCamara()
    {

        if (linesNext[dObject.index].Trim().StartsWith("P"))
        {
            //MainCharacter.sharedInstance.cara.SetActive(true);

            FollowCameras.instance.velocidadRotacion = -75.0f;
            FollowCameras.instance.mode = Modo.Mundo;

        }

        if (linesNext[dObject.index].Trim().StartsWith("L"))
        {
            //MainCharacter.sharedInstance.cara.SetActive(false);

            FollowCameras.instance.velocidadRotacion = -75.0f;
            FollowCameras.instance.mode = Modo.Odnum;

        }

       
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

    void IconDialogoS(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(dObject.trPlayer.position);
            MainCharacter.sharedInstance.VozLogan();
            UIManager.InstanceGUI.NombreDialogo("P");

            dObject.capaObj.layer = 20;
            MainCharacter.sharedInstance.capaObj.layer = 17;
        }

        if (lineas.Trim().StartsWith("L"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);
            dObject.VocesRandom();
            UIManager.InstanceGUI.NombreDialogo("L");


            dObject.capaObj.layer = 16;
            MainCharacter.sharedInstance.capaObj.layer = 20;
        }


    }

    private void Update()
    {

        if (dObject._map.Jugador.Interactuar.WasPressedThisFrame() && fillDialogueLines)
        {

            if (dialogueText.text == linesNext[dObject.index].Substring(1))
            {
                NextDialogue();
            }

            UIManager.InstanceGUI.icono.gameObject.SetActive(false);
        }

    }
}
