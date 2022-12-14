using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Navegar : MonoBehaviour
{
    public Mapa _map;
    public sbyte id_selector;
    public Transform boton;
    //public Transform efectoParticulas;
    public Transform boton1;
    public GameObject[] listOptions;
    public float valor;
    public bool iniciar = false;
    public bool puedeSeleccionar = false;
    public GameObject botones;
    public AudioClip sonido,seleccionar;
    public Animator logan, opciones;

    [SerializeField] float distancia;
    public bool puedemoverse;

    // Start is called before the first frame update
    void OnEnable()
    {
        _map = new Mapa();
        _map.Jugador.Enable();

       

    }

    private void Start()
    {
		//efectoParticulas = GameObject.FindGameObjectWithTag("vfx_stars").GetComponent<Transform>();
		boton = GameObject.FindGameObjectWithTag("Btn1").GetComponent<Transform>();
		boton1 = GameObject.FindGameObjectWithTag("Btn2").GetComponent<Transform>();

		GameObject obListAux = GameObject.Find("Botones").gameObject;

		boton.gameObject.SetActive(false);
		boton1.gameObject.SetActive(false);
		//efectoParticulas.gameObject.SetActive(false);

        int hijos = obListAux.transform.childCount;

        listOptions = new GameObject[hijos];

        for (int i = 0; i < listOptions.Length; i++)
        {
            listOptions[i] = obListAux.transform.GetChild(i).gameObject;


        }

        logan = GameObject.Find("Personaje").GetComponent<Animator>();
        opciones = GameObject.Find("Botones").GetComponent<Animator>();


        if (!iniciar)
        {

            StartCoroutine(Comenzar());

        }
    }

    private void OnDisable()
    {
        _map.Jugador.Disable();
    }

   

    // Update is called once per frame
    void Update()
    {

        if (puedeSeleccionar)
        {
            Navegar1();

           
        }
        

       


        
    }

    public IEnumerator Comenzar()
    {
        iniciar = true;

        botones.SetActive(true);
        

        

        yield return new WaitForSeconds(5.75f);

        opciones.SetBool("Aparecer", true);

        yield return new WaitForSeconds(4f);



		boton.gameObject.SetActive(true);
		boton1.gameObject.SetActive(true);

		//efectoParticulas.gameObject.SetActive(true);

        puedeSeleccionar = true;
        _map.Opciones.Enable();
    }

    void AparecerCOntroles()
    {


        puedemoverse = !puedemoverse;

        if (!puedemoverse)
        {
            UIManager.InstanceGUI.ObPausas.SetActive(false);
        }
        else
        {
            UIManager.InstanceGUI.ObPausas.SetActive(true);
        }


    }

    void Navegar1()
    {
        if (!puedemoverse)
        {
            if (_map.Jugador.BDOWN.WasPressedThisFrame() && id_selector < listOptions.Length - 1)
            {
                id_selector++;
                AudioManager.Instance.PlaySound(seleccionar);

            }

            if (_map.Jugador.BUP.WasPressedThisFrame() && id_selector > 0)
            {
                id_selector--;
                AudioManager.Instance.PlaySound(seleccionar);
            }

        }

        //efectoParticulas.transform.SetParent(listOptions[id_selector].transform);
        //efectoParticulas.transform.position = listOptions[id_selector].transform.position - new Vector3(0.0f, 0.0f, 0.0f);


        //efectoParticulas.transform.SetSiblingIndex(0);
        boton.transform.SetParent(listOptions[id_selector].transform);
        boton.transform.position = listOptions[id_selector].transform.position - new Vector3(distancia, 0.0f, 0.0f);
        boton.transform.SetSiblingIndex(0);


        boton1.transform.SetParent(listOptions[id_selector].transform);
		boton1.transform.position = listOptions[id_selector].transform.position - new Vector3(-distancia, 0.0f, 0.0f);
		boton1.transform.SetSiblingIndex(0);

        

        if (_map.Jugador.Interactuar.WasPressedThisFrame())
        {


            switch (id_selector)
            {
                case 0:
                    UIManager.InstanceGUI.lienzoControlesMenu.transform.SetParent(null);
                    
                    UIManager.InstanceGUI.LoadNextScene();
                    AudioManager.Instance.PlaySound(sonido);
                    _map.Opciones.Disable();
                    puedeSeleccionar = false;
                   
                    
                    logan.SetBool("Aparecer", true);
                    _map.Jugador.Disable();
                    break;

                case 1:

                    AparecerCOntroles();
                    AudioManager.Instance.PlaySound(sonido);


                    break;

                case 2:

                    UIManager.InstanceGUI.ExitPlayGame();

                    break;


                default:
                    break;
            }




        }

    }
}
