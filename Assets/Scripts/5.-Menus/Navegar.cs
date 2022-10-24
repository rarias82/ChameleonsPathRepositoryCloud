using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Navegar : MonoBehaviour
{
    public Mapa _map;
    public sbyte id_selector;
    public Transform boton;
    public GameObject[] listOptions;
    public float valor;
    public bool iniciar = false;
    public bool puedeSeleccionar = false;
    public GameObject botones;
    public AudioClip sonido;
    public Animator logan, opciones;

    [SerializeField] float distancia;

    // Start is called before the first frame update
    void OnEnable()
    {
        _map = new Mapa();
        _map.Jugador.Enable();

        GameObject obListAux = GameObject.Find("Botones").gameObject;

        

        int hijos = obListAux.transform.childCount;

        listOptions = new GameObject[hijos];

        for (int i = 0; i < listOptions.Length; i++)
        {
            listOptions[i] = obListAux.transform.GetChild(i).gameObject;

            
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
        

        if (_map.Jugador.Interactuar.WasPressedThisFrame() && !iniciar)
        {

            StartCoroutine(Comenzar());

        }


        valor =_map.Opciones.Navegar.ReadValue<Vector2>().y;
    }

    IEnumerator Comenzar()
    {
        iniciar = true;

        botones.SetActive(true);
        AudioManager.Instance.PlaySound(sonido);

        logan.SetBool("Aparecer",true);

        yield return new WaitForSeconds(1f);

        opciones.SetBool("Aparecer", true);

        yield return new WaitForSeconds(2f);

        _map.Opciones.Enable();

        boton.gameObject.SetActive(true);

        puedeSeleccionar = true;
    }

    void Navegar1()
    {
        if (valor == -1 && id_selector < listOptions.Length - 1)
        {
            id_selector++;
        }

        if (valor == 1 && id_selector > 0)
        {
            id_selector--;
        }

        boton.transform.SetParent(listOptions[id_selector].transform);
        boton.transform.position = listOptions[id_selector].transform.position - new Vector3(distancia, 0.0f, 0.0f);


        boton.transform.SetSiblingIndex(0);

        if (_map.Jugador.Interactuar.WasPressedThisFrame())
        {


            switch (id_selector)
            {
                case 0:

                    UIManager.InstanceGUI.LoadNextScene();
                    AudioManager.Instance.PlaySound(sonido);

                    break;

                case 1:

                    UIManager.InstanceGUI.ExitPlayGame();


                    break;


                default:
                    break;
            }




        }

    }
}
