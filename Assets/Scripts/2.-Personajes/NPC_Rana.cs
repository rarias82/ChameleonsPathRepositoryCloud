using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCRana
{
    Iddle, Walk, Follow, House, Final, Limites, Carrera, FinalB
}
public class NPC_Rana : MonoBehaviour
{
    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;
    

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] linesA0;
    [SerializeField, TextArea(4, 6)] string[] linesB0;
    [SerializeField, TextArea(4, 6)] string[] linesC0;

    [SerializeField, TextArea(4, 6)] string[] linesEA0;
    [SerializeField, TextArea(4, 6)] string[] linesEA1;
    [SerializeField, TextArea(4, 6)] string[] linesEA2;
    [SerializeField, TextArea(4, 6)] string[] linesEA3;
    [SerializeField, TextArea(4, 6)] string[] linesEA4;

    [SerializeField, TextArea(4, 6)] string[] linesEB0;
    [SerializeField, TextArea(4, 6)] string[] linesEB1;
    [SerializeField, TextArea(4, 6)] string[] linesEB2;
    [SerializeField, TextArea(4, 6)] string[] linesEB3;
    [SerializeField, TextArea(4, 6)] string[] linesEB4;

    [SerializeField, TextArea(4, 6)] string[] linesEC0;
    [SerializeField, TextArea(4, 6)] string[] linesEC1;
    [SerializeField, TextArea(4, 6)] string[] linesEC2;
    [SerializeField, TextArea(4, 6)] string[] linesEC3;
    [SerializeField, TextArea(4, 6)] string[] linesEC4;

    [SerializeField, TextArea(4, 6)] string[] linesFA;
    [SerializeField, TextArea(4, 6)] string[] linesFB;
    [SerializeField, TextArea(4, 6)] string[] linesFC;


    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string consejoFinal;
    bool escribiendo;
    public int index;
    public bool didDialogueStart;
    [SerializeField] int dialogoAnterior;
    [SerializeField] int dialogoSiguiente;
    
    public GameObject detector;
    NPC_Dialogue mapeo;
    [SerializeField] HouseDialogue hd;
   
    public float numeroAnimVelocity;
    public Transform posOriginal;
    int random000 = 0;
    int random001 = 0;

    [Header("Options References")]
    public GameObject Options;
    [SerializeField] GameObject[] listOptions;
    [SerializeField] GameObject selector;
    [SerializeField] sbyte id_selector;
    [SerializeField] string[] optionLines;
    public bool pregunta;
    [SerializeField] sbyte nextDialogue;

    [Header("Move Variables")]
    public sbyte numeroCamino;
    public sbyte numeroCaminoCarrera;
    public NavMeshAgent obNMA;
    [SerializeField] Vector3[] trCaminos;
    [SerializeField] Vector3[] trCaminosCarrera;
    Vector3 diferenciaVector;
    Vector3 diferenciaVectorCarrera;
    public float speedNPC;
    public float distancia;
    public float incrementadorVelocidad;

    [Header("Mode HUD Variables")]
    public ModeNPCRana mode;
    [SerializeField] Vector3 offset;
    [SerializeField] string nameNPC;
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;


    [Header("Anim")]
    public Animator obAnim;
    public int numeroAnim;
    

    [Header("Music References")]
    public AudioClip cancion;
    [SerializeField] AudioClip[] voces;
    [SerializeField] AudioClip[] voces1;
    int voz000, voz001;

    
    

    [Header("Finales")]
    public bool finalA;
    public bool finalB;
    public bool finalB2;
    public Item[] objetosTodo;

    [Header("Obstaculos / Carrera")]
    public GameObject carrera, obstaculos;
    bool noAbrir;

    [Header("Capas Outline")]
    public GameObject capaObj;
    public LayerMask capa;
    public LayerMask capa0;

    [Header("HacerceInvisible")]
    public GameObject malla;
    public CapsuleCollider choqueTrigger;
    public GameObject choqueMasa;
    public GameObject minimMapa;






    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
    }
    private void OnEnable()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        obNMA = GetComponent<NavMeshAgent>();
        speedNPC = obNMA.speed;   
        numeroAnim = 30;

        



      

    }

    void Start()
    {
  


        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();

        mapeo = FindObjectOfType<NPC_Dialogue>();

        Options = GameObject.Find("DialogueOptions").gameObject;

        selector = GameObject.Find("Select").gameObject;

        listOptions[0] = GameObject.Find("Panel0").gameObject;
        listOptions[1] = GameObject.Find("Panel1").gameObject;
        listOptions[2] = GameObject.Find("Panel2").gameObject;

        Item[] objetosTodo = FindObjectsOfType<Item>();

        foreach (Item objeto in objetosTodo)
        {
            objeto.gameObject.SetActive(false);
        }


        carrera = GameObject.Find("LineaCarrera");
        obstaculos = GameObject.Find("Obstaculos");

        carrera.SetActive(false);

        obstaculos.SetActive(false);


    }
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
    void VocesRandom1()
    {
        voz000 = voz001;

        voz001 = Random.Range(0, voces1.Length);


        while (voz001 == voz000)
        {
            voz001 = Random.Range(0, voces1.Length);
        }

        AudioManager.Instance.PlaySound(voces1[voz001]);

    }
    public void Interactuar()
    {

        if (isRange && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv && !noAbrir /*&& !UIManager.InstanceGUI.isGameOver*/)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1) && UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 0)
            {
                NextDialogue();

                UIManager.InstanceGUI.icono.gameObject.SetActive(false);
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);

            }


        }

        if (mapeo._map.Jugador.SaltarEscena.WasPressedThisFrame() && escribiendo)
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

    public void InteractuarFinal()
    {

        if (isRange && mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv )
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();

                UIManager.InstanceGUI.icono.gameObject.SetActive(false);
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);

            }


        }

        if (mapeo._map.Jugador.SaltarEscena.WasPressedThisFrame() && escribiendo)
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
    public void StartDialogue()
    {


        UIManager.InstanceGUI.QuitarHUDInGame();

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));


        if (pregunta)
        {
            switch (nextDialogue)
        {

            case 0:


                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEA0;
                }
                if (random001 == 1)
                {
                    lines = linesEA1;
                }
                if (random001 == 2)
                {
                    lines = linesEA2;
                }
                if (random001 == 3)
                {
                    lines = linesEA3;
                }
                if (random001 == 4)
                {
                    lines = linesEA4;
                }

                break;

            case 1:
                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEB0;
                }
                if (random001 == 1)
                {
                    lines = linesEB1;
                }
                if (random001 == 2)
                {
                    lines = linesEB2;
                }
                if (random001 == 3)
                {
                    lines = linesEB3;
                }
                if (random001 == 4)
                {
                    lines = linesEB4;
                }
                break;

            case 2:
                random000 = random001;

                random001 = Random.Range(0, 5);


                while (random001 == random000)
                {
                    random001 = Random.Range(0, 5);
                }

                if (random001 == 0)
                {
                    lines = linesEC0;
                }
                if (random001 == 1)
                {
                    lines = linesEC1;
                }
                if (random001 == 2)
                {
                    lines = linesEC2;
                }
                if (random001 == 3)
                {
                    lines = linesEC3;
                }
                if (random001 == 4)
                {
                    lines = linesEC4;
                }
                break;

            default:
                break;
        }
        }
        

        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        MainCharacter.sharedInstance.animIntervalo = 0.0f;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.InstanceGUI.fadeBlack = true;
        UIManager.InstanceGUI.fadeBlackN = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);


        DialogoFinal();

        marker.SetActive(false);

        StartCoroutine(WriteDialogue());

    }
    public IEnumerator WriteDialogue()
    {

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);
        marker.SetActive(false);

        if (index == 0)
        {
            posOriginal = FollowCameras.instance.transform;

            if (lines[index].Trim().StartsWith("P"))
            {
                FollowCameras.instance.velocidadRotacion = -75.0f;
                FollowCameras.instance.mode = Modo.Mundo;
            }

            if (lines[index].Trim().StartsWith("R"))
            {
                FollowCameras.instance.velocidadRotacion = -75.0f;
                FollowCameras.instance.mode = Modo.Odnum;
            }

            marker.SetActive(false);

            UIManager.InstanceGUI.BurbujaDialogo(0);

            

            while (obCameras.orthographicSize > 3.5f)
            {

                
                Vector3 direction = transform.position - new Vector3(trPlayer.transform.position.x, 6.079084f, trPlayer.transform.position.z);
                
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, direction, (speedZoom) * Time.deltaTime);
           
                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }

            detector.SetActive(true);
            yield return new WaitForSeconds(1f);
            FollowCameras.instance.detenerGiro = true;
            




		}
        
        if (index > 0 )
        {
            GiroDeCamara();
        }


     



        while (FollowCameras.instance.mode == Modo.Mundo)
        {
            yield return null;
        }

		while(FollowCameras.instance.mode == Modo.Odnum)
        {
            yield return null;
		}

      
            
        

        dialogueText.text = string.Empty;
        

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);

        IconDialogo(lines[index]);


		if (!pregunta)
		{
            if (numeroAnim < 100)
            {
                AnimToVar(index);

				if (index == 0)
				{
                    UIManager.InstanceGUI.BurbujaDialogo(15);
				}
                if (index == 1)
                {
                    MainCharacter.sharedInstance.eAnim = 20;
                    UIManager.InstanceGUI.BurbujaDialogo(4);
                }
                if (index == 2)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(5);
                }
                if (index == 3)
                {
                    MainCharacter.sharedInstance.eAnim = 21;
                    UIManager.InstanceGUI.BurbujaDialogo(2);
                }
                if (index == 4)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }
                if (index == 5)
                {
                    MainCharacter.sharedInstance.eAnim = 22;
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
                if (index == 6)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
            }

           

		}
		else
		{
            if (numeroAnim >= 100 && numeroAnim < 200)
            {
                AnimToVar(index + 100);

                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 2;
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                }
                if (index == 1)
                {
                    
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }
                if (index == 2)
                {
                    MainCharacter.sharedInstance.eAnim = 10;
                    UIManager.InstanceGUI.BurbujaDialogo(5);
                }
                if (index == 3)
                {
                    
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                }

                if (index == 4)
                {
                    MainCharacter.sharedInstance.eAnim = 2;
                    UIManager.InstanceGUI.BurbujaDialogo(8);
                }

                if (index == 5)
                {
                    
                    UIManager.InstanceGUI.BurbujaDialogo(1);
                }

                if (index == 6)
                {
                    MainCharacter.sharedInstance.eAnim = 10;
                    UIManager.InstanceGUI.BurbujaDialogo(0);
                }

                if (index == 7)
                {
                    
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                }

                if (index == 8)
                {
                    MainCharacter.sharedInstance.eAnim = 45;
                    UIManager.InstanceGUI.BurbujaDialogo(12);
                }

                if (index == 9)
                {
                    
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }

            }
            if (numeroAnim >= 200 && numeroAnim < 300)
            {

                AnimToVar(index + 200);


				if (id_selector == 1)
				{
                    if (index == 0)
                    {
                        MainCharacter.sharedInstance.eAnim = 2;
                        UIManager.InstanceGUI.BurbujaDialogo(13);
                    }
                    if (index == 1)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(0);
                    }
                    if (index == 2)
                    {
                        MainCharacter.sharedInstance.eAnim = 20;
                        UIManager.InstanceGUI.BurbujaDialogo(4);
                    }

                    if (index == 3)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(2);
                    }

                    if (index == 4)
                    {
                        MainCharacter.sharedInstance.eAnim = 3;
                        UIManager.InstanceGUI.BurbujaDialogo(7);
                    }

                    if (index == 5)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(7);
                    }

                    if (index == 6)
                    {
                        MainCharacter.sharedInstance.eAnim = 2;
                        UIManager.InstanceGUI.BurbujaDialogo(8);
                    }

                    if (index == 7)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(4);
                    }

                    if (index == 8)
                    {
                        MainCharacter.sharedInstance.eAnim = 10;
                        UIManager.InstanceGUI.BurbujaDialogo(5);
                    }
                }
				if (id_selector == 2)
				{
                    if (index == 0)
                    {
                        MainCharacter.sharedInstance.eAnim = 2;
                        UIManager.InstanceGUI.BurbujaDialogo(2);
                    }
                    if (index == 1)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(5);
                    }
                    if (index == 2)
                    {
                        MainCharacter.sharedInstance.eAnim = 20;
                        UIManager.InstanceGUI.BurbujaDialogo(6);
                    }

                    if (index == 3)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(2);
                    }

                    if (index == 4)
                    {
                        MainCharacter.sharedInstance.eAnim = 3;
                        UIManager.InstanceGUI.BurbujaDialogo(1);
                    }

                    if (index == 5)
                    {

                        UIManager.InstanceGUI.BurbujaDialogo(4);
                    }

                    if (index == 6)
                    {
                        MainCharacter.sharedInstance.eAnim = 2;
                        UIManager.InstanceGUI.BurbujaDialogo(5);
                    }

                  
                }    

                //}






            }
			if (lines == linesEA0 || lines == linesEA1)
			{
                if (index == 0)
                {
                    numeroAnim = 0;
                    UIManager.InstanceGUI.BurbujaDialogo(5);
                }
 
            }
            if (lines == linesEA4)
            {
                if (index == 0)
                {
                    numeroAnim = 6;
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                }

            }
            if (lines == linesEA2 || lines == linesEA3)
            {
                if (index == 0)
                {
                    numeroAnim = 6;
                    UIManager.InstanceGUI.BurbujaDialogo(4);
                }

                if (index == 1)
                {
                    MainCharacter.sharedInstance.eAnim = 3;
                    UIManager.InstanceGUI.BurbujaDialogo(8);
                }

                if (index == 2)
                {
                    numeroAnim = 101;
                    UIManager.InstanceGUI.BurbujaDialogo(13);
                }

                if (index == 3)
                {
                    MainCharacter.sharedInstance.eAnim = 2;
                    UIManager.InstanceGUI.BurbujaDialogo(6);
                }

            }

            if (lines == linesFA)
            {
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 22;
                    UIManager.InstanceGUI.BurbujaDialogo(5);
               
                }

                if (index == 1)
                {
                    UIManager.InstanceGUI.BurbujaDialogo(2);
                    numeroAnim = 6;
                }

                if (index == 2)
                {
                    MainCharacter.sharedInstance.eAnim = 45;
                    UIManager.InstanceGUI.BurbujaDialogo(8);
                }

                if (index == 3)
                {
                    numeroAnim = 201;
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                }

                if (index == 4)
                {
                    MainCharacter.sharedInstance.eAnim = 2;
                    UIManager.InstanceGUI.BurbujaDialogo(7);
                }

                if (index == 5)
                {
                    numeroAnim = 203;
                    UIManager.InstanceGUI.BurbujaDialogo(8);
                }

            }
            if (lines == linesFB)
            {
                if (index == 0)
                {
                    MainCharacter.sharedInstance.eAnim = 22;
                    UIManager.InstanceGUI.BurbujaDialogo(15);

                }

                if (index == 1)
                {
                    numeroAnim = 6;
                    UIManager.InstanceGUI.BurbujaDialogo(3);
                    
                }


            }

        }








        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;

            
            yield return new WaitForSeconds(speedText);

            escribiendo = true;
        }


        if (dialogueText.text == lines[index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }
        



    }

    public void GiroDeCamara()
    {
        if (lines[index].Trim().StartsWith("R"))
        {
            //MainCharacter.sharedInstance.cara.SetActive(false);

            FollowCameras.instance.velocidadRotacion = -75.0f;
            FollowCameras.instance.mode = Modo.Odnum;

        }

        if (lines[index].Trim().StartsWith("P"))
        {
            //MainCharacter.sharedInstance.cara.SetActive(true);

            FollowCameras.instance.velocidadRotacion = -75.0f;
            FollowCameras.instance.mode = Modo.Mundo;

        }
    }
    void AnimToVar(int indice)
    {
        numeroAnim = indice;

    }
    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position);
            MainCharacter.sharedInstance.VozLogan();
            UIManager.InstanceGUI.NombreDialogo("P");



            MainCharacter.sharedInstance.capaObj.layer = 17;
            capaObj.layer = 20;
        }

      
        if (lineas.Trim().StartsWith("R"))
        {
            UIManager.InstanceGUI.NombreDialogo("R");
            UIManager.InstanceGUI.PosicionarGlobo(transform.position);

            MainCharacter.sharedInstance.capaObj.layer = 20;
            capaObj.layer = 18;


            if (!pregunta)
            {
                VocesRandom();
            }
            else
            {
                VocesRandom1();
            }
            

        }


    }
    public IEnumerator CloseDialogue()
    {
        

        detector.SetActive(true);

        index = 0;

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));
      

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

        UIManager.InstanceGUI.fadeFrom = true;
        UIManager.InstanceGUI.fadeFromN = true;

        numeroAnim = 99;

        //FollowCameras.instance.mode = Modo.InGame;

        FollowCameras.instance.velocidadRotacion = -75f;
        FollowCameras.instance.mode = Modo.MundoAlreves;

        while (Camera.main.orthographicSize < 7.5f)
        {
            Camera.main.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;

            //FollowCameras.instance.transform.position = Vector3.Lerp(FollowCameras.instance.transform.position, posOriginal.transform.position, (speedZoom / 2.0f) * Time.deltaTime);

            //Vector3 currentAngle = new Vector3(
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.x, posOriginal.transform.rotation.eulerAngles.x, (speedZoom / 2.0f) * Time.deltaTime),
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.y, posOriginal.transform.rotation.eulerAngles.y, (speedZoom / 2.0f) * Time.deltaTime),
            //Mathf.Lerp(FollowCameras.instance.transform.rotation.eulerAngles.z, posOriginal.transform.rotation.eulerAngles.z, (speedZoom / 2.0f) * Time.deltaTime)
            //);

            yield return null;

        }

        while (FollowCameras.instance.mode == Modo.MundoAlreves)
        {
            yield return null;
        }


        FollowCameras.instance.mode = Modo.InGame;


        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);


        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);



        if (finalA)
        {
            //if (UIManager.InstanceGUI.isGameOver)
            //{
            //    UIManager.InstanceGUI.FinDelJuego();
            //}
            //else
            //{
                UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalARana());
            //}

            MainCharacter.sharedInstance.puedePausar = true;

        }
        else if (finalB && !finalB2)
        {
            //if (UIManager.InstanceGUI.isGameOver)
            //{
            //    UIManager.InstanceGUI.FinDelJuego();
            //}
            //else
            //{
                UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalBRana());
            //}MainCharacter.sharedInstance.puedePausar = true;

        }
        else if(finalB2)
        {

            //if (UIManager.InstanceGUI.isGameOver)
            //{
            //    UIManager.InstanceGUI.FinDelJuego();
            //}
            //elseMainCharacter.sharedInstance.puedePausar = true;
            //{
            UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.FinalARana());
            //}
            MainCharacter.sharedInstance.puedePausar = true;
        }
        else
        {
            MainCharacter.sharedInstance.puedePausar = true;
            MainCharacter.sharedInstance.canMove = true;

            yield return new WaitForSeconds(0.01f);
            didDialogueStart = false;
            marker.SetActive(true);

            if (UIManager.InstanceGUI.isGameOver)
            {
                noAbrir = true;
                MainCharacter.sharedInstance.puedePausar = false;
                MainCharacter.sharedInstance.canMove = false;
                UIManager.InstanceGUI.ConsejoFinal(consejoFinal);
                UIManager.InstanceGUI.FinDelJuego();
            }
        }

        MainCharacter.sharedInstance.eAnim = 0;


        
		if (UIManager.InstanceGUI.isGameOver)
		{
			UIManager.InstanceGUI.FinDelJuego();
		}
		else
		{
            MainCharacter.sharedInstance.puedePausar = false;
        }

		VolverColores();

    }
    public void Navegate()
    {
        mapeo._map.Jugador.Enable();

        if (mapeo._map.Jugador.BDOWN.WasPressedThisFrame() && id_selector < listOptions.Length - 1)
        {
            id_selector++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        if (mapeo._map.Jugador.BUP.WasPressedThisFrame() && id_selector > 0)
        {
            id_selector--;
            AudioManager.Instance.PlaySound(AudioManager.Instance.selectButton);
        }

        selector.transform.SetParent(listOptions[id_selector].transform);
        selector.transform.position = listOptions[id_selector].transform.position;


        selector.transform.SetSiblingIndex(0);


        if (mapeo._map.Jugador.Interactuar.WasPressedThisFrame() && UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 /*&& !UIManager.InstanceGUI.isGameOver*/)
        {


            index = 0;
            MainCharacter.sharedInstance.eAnim = 22;

            switch (id_selector)
            {
                case 0:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectBad);
                    numeroAnim = 100;
                    UIManager.InstanceGUI.GanarPuntos(false, UIManager.InstanceGUI.puntos);


                    foreach (Item objeto in objetosTodo)
                    {
                        objeto.gameObject.SetActive(true);
                    }


                    break;

                case 1:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectGood);
                    numeroAnim = 200;
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);
                    finalB = true;



                        break;

                case 2:
                    AudioManager.Instance.PlaySound(AudioManager.Instance.selectGood);
                    numeroAnim = 200;
                    UIManager.InstanceGUI.GanarPuntos(true, UIManager.InstanceGUI.puntos);
                    finalB = true;

                    break;

                default:
                    break;

                   

            }


            UIManager.InstanceGUI.BurbujaDialogo(2);


            UIManager.InstanceGUI.AnimateOptions(false);



            StartCoroutine(ChangeDialogueR());


        }


    }
    IEnumerator ChangeDialogueR()
    {
        index = 0;

        pregunta = true;

        switch (id_selector)
        {
            case 0:

                lines = linesA0;
                yield return new WaitForSeconds(UIManager.InstanceGUI.timeTransicion);
                FollowCameras.instance.mode = Modo.Mundo;
                FollowCameras.instance.velocidadRotacion = -75.0f;
                MainCharacter.sharedInstance.eAnim = 22;

                break;

            case 1:

                lines = linesB0;
                break;

            case 2:

                lines = linesC0;
                break;


            default:
                break;
        }

        nextDialogue = id_selector;
        yield return null;

        StartCoroutine(WriteDialogue());




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

            if (!pregunta)
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
        for (int i = 0; i < listOptions.Length; i++)
        {
            listOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = optionLines[i];
        }


        index = 0;
        mapeo._map.Disable();
        yield return new WaitForSeconds(1f);
        UIManager.InstanceGUI.AnimateOptions(true);
        
        yield return new WaitForSeconds(1f);
        


        //Options.SetActive(true);

       
        yield return new WaitForSeconds(0.5f);
        mapeo._map.Enable();
    }

    void VolverColores()
    {

        capaObj.layer = 18;
        MainCharacter.sharedInstance.capaObj.layer = 17;
    }
    void TurnToLogan()
    {

        Vector3 direction = trPlayer.transform.position - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);

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
    void Carrera()
    {
        diferenciaVectorCarrera = trCaminosCarrera[numeroCaminoCarrera] - transform.position;

        if (diferenciaVectorCarrera.sqrMagnitude < (0.5f * 2f))
        {
            numeroCaminoCarrera++;
           
            
            obNMA.speed += incrementadorVelocidad;
            if (obNMA.speed > 6.4)
            {
                obNMA.speed = 6.4f;
            }
            
            if (numeroCaminoCarrera >= trCaminosCarrera.Length)
            {
                didDialogueStart = false;
				mode = ModeNPCRana.FinalB;

				numeroAnim = 99;
                numeroCaminoCarrera = 0;
                //gameObject.SetActive(false);
            }

        }

        if (mode != ModeNPCRana.FinalB)
        {
            obNMA.SetDestination(trCaminosCarrera[numeroCaminoCarrera]);
        }
        



    }
    void DialogoFinal()
    {
        if (finalA)
        {
            lines = linesFA;
        }

        if (finalB)
        {
            lines = linesFB;
            finalB2 = true;
        }

    }

    public void DesaparecerRana(bool verdad)
    {

        if (verdad)
        {
            malla.SetActive(false);
            choqueTrigger.enabled = false;
            choqueMasa.SetActive(false);
            minimMapa.SetActive(false);
        }
        else
        {
            malla.SetActive(true);
            choqueTrigger.enabled = true;
            choqueMasa.SetActive(true);
            minimMapa.SetActive(true);

        }

   
    }
    private void Update()
    {

        if (mode == ModeNPCRana.Walk)
        {
            Walking();
        }
        if (mode == ModeNPCRana.Iddle)
        {
            if (UIManager.InstanceGUI.obAnimOptionsGame.GetInteger("Show") == 1 && (!mapeo.didDialogueStart || !mapeo.gameObject.activeInHierarchy) && (!hd.didDialogueStart || hd.didDialogueStart))
            {
                Navegate();
            }
			if (!UIManager.InstanceGUI.obCartelPausa.activeInHierarchy)
			{
                Interactuar();
            }
            
            TurnToLogan();
            //AnimacionTexto();
        }

        if (mode == ModeNPCRana.FinalB)
        {
			if (!UIManager.InstanceGUI.obCartelPausa.activeInHierarchy)
			{
                //Interactuar();
                TurnToLogan();
                Debug.Log("John");
                InteractuarFinal();
            }
            
        }

        if (mode == ModeNPCRana.Carrera)
        {
            Carrera();
        }
        //if (mapeo._map.Jugador.Interactuar.WasPressedThisFrame())
        //{
        //    UIManager.InstanceGUI.Temblor();
        //}

        RotateSon();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Walk)
        {

            mode = ModeNPCRana.Iddle;

            obNMA.speed = 0.0f;
            isRange = !isRange;
            marker.SetActive(true);
            numeroAnim = 99;

            MainCharacter.sharedInstance.puedePausar = false;



        }

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.FinalB)
        {


            MainCharacter.sharedInstance.puedePausar = false;
            obNMA.speed = 0.0f;
            isRange = true;
            marker.SetActive(true);
            numeroAnim = 99;


            


        }

        //if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Iddle)
        //{

        //    mode = ModeNPCRana.Iddle;

        //    obNMA.speed = 0.0f;
        //    isRange = !isRange;
        //    marker.SetActive(true);
        //    numeroAnim = 99;
        //    didDialogueStart = false;

        //}



    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.Iddle && !finalB)
        {

            mode = ModeNPCRana.Walk;

            obNMA.speed = speedNPC;
            isRange = !isRange;
            marker.SetActive(false);
            numeroAnim = 30;
            MainCharacter.sharedInstance.puedePausar = true;


        }

        if (other.gameObject.CompareTag("P1") && mode == ModeNPCRana.FinalB)
        {

            MainCharacter.sharedInstance.puedePausar = true;

            obNMA.speed = 0.0f;
            isRange = false;
            marker.SetActive(false);
            numeroAnim = 99;
           

        }


    }
    private void LateUpdate()
    {
        obAnim.SetInteger("EstadoAnimo", numeroAnim);


    }




}
