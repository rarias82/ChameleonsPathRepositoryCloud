using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum ModeNPCDuende
{
    Iddle, Hablar
}
public class NPC_Duende : MonoBehaviour
{
    [Header("Interaction Variables")]
    [SerializeField] GameObject marker;
    [SerializeField] bool isRange;
    public float speedText;

    [Header("Dialogue Variables")]
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] string[] lines;
    [SerializeField, TextArea(4, 6)] string[] linesPrincipal;
    [SerializeField, TextArea(4, 6)] string[] linesGanar;
    [SerializeField, TextArea(4, 6)] string[] linesPerder;
    [SerializeField, TextArea(4, 6)] string[] linesPerderMedio;
    [SerializeField, TextArea(4, 6)] string consejoFinal;
    public int index;
    public bool didDialogueStart;
    public GameObject detector;
    bool escribiendo;

    [Header("Mode HUD Variables")]
    public ModeNPCDuende mode;
    [SerializeField] Vector3 offset;
    [SerializeField] string nameNPC;
    protected Transform trPlayer;

    [Header("Camera References")]
    public Camera obCameras;
    public float speedZoom;

    [Header("Capas Outline")]
    public GameObject capaObj;
    public LayerMask capa;
    public LayerMask capa0;


    [Header("Anim")]
    public Animator obAnim;
    public int numeroAnim;

    [Header("Music References")]
    public AudioClip cancion;
    [SerializeField] AudioClip[] voces;
    int voz000, voz001;
    void RotateSon()
    {
        Quaternion rotation = Quaternion.Euler(offset);
        marker.transform.rotation = rotation;
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
    private void OnEnable()
    {
        marker = transform.Find("Marker").gameObject;
        marker.SetActive(false);
        obCameras = Camera.main;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;

        
    }



    void Start()
    {
        dialogueText = GameObject.Find("Text (TMP)N").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (mode == ModeNPCDuende.Iddle)
        {
			if (!UIManager.InstanceGUI.obCartelPausa.activeInHierarchy)
			{
                Interactuar();
                TurnToLogan();
            }
			
            

        }

        RotateSon();
    }

    public void IconDialogo(string lineas)
    {

        if (lineas.Trim().StartsWith("P"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(trPlayer.position);
            MainCharacter.sharedInstance.VozLogan();
            UIManager.InstanceGUI.NombreDialogo("P");

            capaObj.layer = 20;
            MainCharacter.sharedInstance.capaObj.layer = 17;

        }

        if (lineas.Trim().StartsWith("E"))
        {
            UIManager.InstanceGUI.PosicionarGlobo(transform.position + new Vector3(0, 0.75f, 0));
            VocesRandom();
            UIManager.InstanceGUI.NombreDialogo("E");

            capaObj.layer = 22;
            MainCharacter.sharedInstance.capaObj.layer = 20;



        }


    }

    void Interactuar()
	{
        if ((isRange) && MainCharacter.sharedInstance._map.Jugador.Interactuar.WasPressedThisFrame() && Inventory.instance.moverInv  && !UIManager.InstanceGUI.isGameOver)
        {
            if (!didDialogueStart )
            {
               
                StartDialogue();
            }
            else if (dialogueText.text == lines[index].Substring(1))
            {
                NextDialogue();
                UIManager.InstanceGUI.icono.gameObject.SetActive(false);
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);
            }

          
        } //Dialogo comun

        if (MainCharacter.sharedInstance._map.Jugador.SaltarEscena.WasPressedThisFrame() && escribiendo)
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
    void TurnToLogan()
    {

		Vector3 direction = trPlayer.transform.position - new Vector3(transform.position.x, 6.079084f, transform.position.z);
		transform.forward = Vector3.Lerp(transform.forward, direction, (speedZoom / 2.5f) * Time.deltaTime);

	}

    public void StartDialogue()
    {
        MainCharacter.sharedInstance.vectorForAnim = Vector3.zero;
        MainCharacter.sharedInstance.animIntervalo = 0.0f;

        MainCharacter.sharedInstance.intervalo = 0.0f;

        MainCharacter.sharedInstance.canMove = false;

        didDialogueStart = true;

        UIManager.InstanceGUI.fadeBlack = true;
        UIManager.InstanceGUI.fadeBlackN = true;

        marker.SetActive(false);

        index = 0;

        Inventory.instance.panelItem.SetActive(false);
        UIManager.InstanceGUI.obMap.SetActive(false);
        UIManager.InstanceGUI.obMapMark.SetActive(false);

        marker.SetActive(false);

		if (UIManager.InstanceGUI.puntosCalificacion <= 50)
		{
            lines = linesPerder;
            
        }
		else
		{
            lines = linesGanar;
        }

        

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

            StartCoroutine(CloseDialogue());
        }

    }
    public void GiroDeCamara()
    {
        if (lines[index].Trim().StartsWith("E"))
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

    public IEnumerator WriteDialogue()
    {
        marker.SetActive(false);
        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);


        if (index == 0)
        {

            marker.SetActive(false);

            AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(cancion));

            GiroDeCamara();

          

            while (obCameras.orthographicSize > 3.5f)
            {
                Vector3 directionLogan = transform.position - new Vector3(trPlayer.transform.position.x, 5.01f, trPlayer.transform.position.z);
  
                trPlayer.transform.forward = Vector3.Lerp(trPlayer.transform.forward, directionLogan, (speedZoom) * Time.deltaTime);

                obCameras.orthographicSize -= speedZoom * Time.deltaTime;

                yield return null;

            }


            detector.SetActive(true);
            yield return new WaitForSeconds(1f);
            FollowCameras.instance.detenerGiro = true;


        }
		else
		{
            GiroDeCamara();
        }


        while (FollowCameras.instance.mode == Modo.Mundo)
            {
                yield return null;
            }

        while (FollowCameras.instance.mode == Modo.Odnum)
            {
                yield return null;
            }



        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(true);


        IconDialogo(lines[index]);

        if (index == 0)
        {
            UIManager.InstanceGUI.BurbujaDialogo(4);
            
            MainCharacter.sharedInstance.eAnim = 20;
        }

        if (index == 1)
		{
            UIManager.InstanceGUI.BurbujaDialogo(1);
            numeroAnim = 1;
		}

        if (index == 2)
        {
            UIManager.InstanceGUI.BurbujaDialogo(8);

            MainCharacter.sharedInstance.eAnim = 21;
        }

        if (index == 3)
        {
            UIManager.InstanceGUI.BurbujaDialogo(15);
            numeroAnim = 2;
        }

        if (index == 4)
        {

			if (lines == linesGanar)
			{
                numeroAnim = 200;
                UIManager.InstanceGUI.BurbujaDialogo(5);
            }

            if (lines == linesPerder)
            {
                
                numeroAnim = 100;
                UIManager.InstanceGUI.BurbujaDialogo(6);
            }
           
        }


        foreach (char letter in lines[index].Substring(1).ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speedText);
            escribiendo = true;
        }
        escribiendo = false;

        if (dialogueText.text == lines[index].Substring(1))
        {
            UIManager.InstanceGUI.icono.gameObject.SetActive(true);
        }
    }

    public IEnumerator CloseDialogue()
    {
        detector.SetActive(true);
        index = 0;

        MainCharacter.sharedInstance.eAnim = 0;

        AudioManager.Instance.StartCoroutine(AudioManager.Instance.ChangeMusic(AudioManager.Instance.cancionNivel1));

        UIManager.InstanceGUI.ballonDialogue.gameObject.SetActive(false);

        dialogueText.text = string.Empty;
        UIManager.InstanceGUI.EmptyNames();



        UIManager.InstanceGUI.fadeFrom = true;
        UIManager.InstanceGUI.fadeFromN = true;


        FollowCameras.instance.velocidadRotacion = -75f;
        FollowCameras.instance.mode = Modo.MundoAlreves;

        while (Camera.main.orthographicSize < 7.5f)
        {
            Camera.main.orthographicSize += (speedZoom / 2.0f) * Time.deltaTime;


            yield return null;

        }

        while (FollowCameras.instance.mode == Modo.MundoAlreves)
        {
            yield return null;
        }


        FollowCameras.instance.mode = Modo.InGame;

        numeroAnim = 0;

        Inventory.instance.panelItem.SetActive(true);
        UIManager.InstanceGUI.obMap.SetActive(true);
        UIManager.InstanceGUI.obMapMark.SetActive(true);

        FollowCameras.instance.pararGiro = false;
        detector.SetActive(false);



		//marker.SetActive(true);

		if (lines == linesPerder)
		{
            UIManager.InstanceGUI.isGameOver = true;
        }

        
        VolverColores();

		if (lines == linesPerder)
		{
            if (UIManager.InstanceGUI.isGameOver)
            {
                UIManager.InstanceGUI.FinDelJuego();
                MainCharacter.sharedInstance.canMove = false;
                MainCharacter.sharedInstance.puedePausar = false;
                UIManager.InstanceGUI.ConsejoFinal(consejoFinal);
            }
        }

        if (lines == linesGanar)
        {
            UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

            yield return new WaitForSeconds(1f);
            
            MainCharacter.sharedInstance.canMove = true;
            MainCharacter.sharedInstance.puedePausar = true;
            gameObject.SetActive(false);

        }

        //didDialogueStart = false;

    }

    void VolverColores()
    {

        capaObj.layer = 22;
        MainCharacter.sharedInstance.capaObj.layer = 17;
    }

	private void LateUpdate()
	{
        obAnim.SetInteger("Anim", numeroAnim);
    }
	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("P1"))
        {


            MainCharacter.sharedInstance.puedePausar = false;
            isRange = !isRange;
            marker.SetActive(true);

        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("P1"))
        {

            MainCharacter.sharedInstance.puedePausar = true;
            isRange = !isRange;
            marker.SetActive(false);
        }




    }
}
