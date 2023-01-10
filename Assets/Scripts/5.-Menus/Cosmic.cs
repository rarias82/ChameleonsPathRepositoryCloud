using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmic : MonoBehaviour
{
    public Mapa _map;
    public int indexVineta;
    public bool pasarDiapo = true;
    public GameObject boton, colita;
    bool terminar;
    bool transicion;

    void OnEnable()
    {
        _map = new Mapa();
        _map.Jugador.Enable();

       

    }

    

    private void OnDisable()
    {
        _map.Jugador.Disable();
    }

    void Start()
    {
       

        boton = GameObject.Find("ApretarBOton");
        colita = GameObject.Find("ApretarCola");

        boton.SetActive(false);
        colita.SetActive(true);
        //boton.SetActive(true);

    }

    IEnumerator PasarVinetas()
    {


        pasarDiapo = false;

        if (indexVineta == 3)
        {
            while (UIManager.InstanceGUI.listaVinetas1[0].color.a != 0f)
            {
                UIManager.InstanceGUI.listaVinetas1[0].color = new Color(UIManager.InstanceGUI.listaVinetas1[0].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[0].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[0].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[0].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[1].color = new Color(UIManager.InstanceGUI.listaVinetas1[1].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[1].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[1].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[2].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[2].color = new Color(UIManager.InstanceGUI.listaVinetas1[2].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[2].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[2].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[2].color.a, 0f, (0.75f) * Time.deltaTime));


                yield return null;

            }
        }

        if (indexVineta == 8)
        {
            while (UIManager.InstanceGUI.listaVinetas1[3].color.a != 0f)
            {

                UIManager.InstanceGUI.listaVinetas1[3].color = new Color(UIManager.InstanceGUI.listaVinetas1[3].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[3].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[3].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[3].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[4].color = new Color(UIManager.InstanceGUI.listaVinetas1[4].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[4].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[4].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[4].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[5].color = new Color(UIManager.InstanceGUI.listaVinetas1[5].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[5].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[5].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[5].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[6].color = new Color(UIManager.InstanceGUI.listaVinetas1[6].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[6].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[6].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[6].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[7].color = new Color(UIManager.InstanceGUI.listaVinetas1[7].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[7].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[7].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[7].color.a, 0f, (0.75f) * Time.deltaTime));

                

                yield return null;

            }
        }

        if (indexVineta == 12)
        {
            while (UIManager.InstanceGUI.listaVinetas1[8].color.a != 0f)
            {


                UIManager.InstanceGUI.listaVinetas1[8].color = new Color(UIManager.InstanceGUI.listaVinetas1[8].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[8].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[8].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[8].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[9].color = new Color(UIManager.InstanceGUI.listaVinetas1[9].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[9].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[9].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[9].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[10].color = new Color(UIManager.InstanceGUI.listaVinetas1[10].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[10].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[10].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[10].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[11].color = new Color(UIManager.InstanceGUI.listaVinetas1[11].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[11].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[11].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[11].color.a, 0f, (0.75f) * Time.deltaTime));



                yield return null;

            }
        }

        if (indexVineta == 17)
        {
            while (UIManager.InstanceGUI.listaVinetas1[12].color.a != 0f)
            {


                UIManager.InstanceGUI.listaVinetas1[12].color = new Color(UIManager.InstanceGUI.listaVinetas1[12].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[12].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[12].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[12].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[13].color = new Color(UIManager.InstanceGUI.listaVinetas1[13].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[13].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[13].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[13].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[14].color = new Color(UIManager.InstanceGUI.listaVinetas1[14].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[14].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[14].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[14].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[15].color = new Color(UIManager.InstanceGUI.listaVinetas1[15].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[15].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[15].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[15].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[16].color = new Color(UIManager.InstanceGUI.listaVinetas1[16].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[16].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[16].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[16].color.a, 0f, (0.75f) * Time.deltaTime));



                yield return null;

            }
        }

        if (indexVineta == 21)
        {
            while (UIManager.InstanceGUI.listaVinetas1[17].color.a != 0f)
            {


                UIManager.InstanceGUI.listaVinetas1[17].color = new Color(UIManager.InstanceGUI.listaVinetas1[17].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[17].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[17].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[17].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[18].color = new Color(UIManager.InstanceGUI.listaVinetas1[18].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[18].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[18].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[18].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[19].color = new Color(UIManager.InstanceGUI.listaVinetas1[19].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[19].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[19].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[19].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[20].color = new Color(UIManager.InstanceGUI.listaVinetas1[20].color.r,
                                                                         UIManager.InstanceGUI.listaVinetas1[20].color.g,
                                                                         UIManager.InstanceGUI.listaVinetas1[20].color.b,
                                                                         Mathf.MoveTowards(
                                                                         UIManager.InstanceGUI.listaVinetas1[20].color.a, 0f, (0.75f) * Time.deltaTime));




                yield return null;

            }
        }

        if (indexVineta == 24)
        {
            while (UIManager.InstanceGUI.listaVinetas1[21].color.a != 0f)
            {
                UIManager.InstanceGUI.listaVinetas1[21].color = new Color(UIManager.InstanceGUI.listaVinetas1[21].color.r,
                                                                        UIManager.InstanceGUI.listaVinetas1[21].color.g,
                                                                        UIManager.InstanceGUI.listaVinetas1[21].color.b,
                                                                        Mathf.MoveTowards(
                                                                        UIManager.InstanceGUI.listaVinetas1[21].color.a, 0f, (0.75f) * Time.deltaTime));

                UIManager.InstanceGUI.listaVinetas1[22].color = new Color(UIManager.InstanceGUI.listaVinetas1[22].color.r,
                                                                        UIManager.InstanceGUI.listaVinetas1[22].color.g,
                                                                        UIManager.InstanceGUI.listaVinetas1[22].color.b,
                                                                        Mathf.MoveTowards(
                                                                        UIManager.InstanceGUI.listaVinetas1[22].color.a, 0f, (0.75f) * Time.deltaTime));


                UIManager.InstanceGUI.listaVinetas1[23].color = new Color(UIManager.InstanceGUI.listaVinetas1[23].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[23].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[23].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[23].color.a, 0f, (0.75f) * Time.deltaTime));

                



                yield return null;

            }
        }

        while (UIManager.InstanceGUI.listaVinetas1[indexVineta].color.a != 1f)
        {

            UIManager.InstanceGUI.listaVinetas1[indexVineta].color = new Color(UIManager.InstanceGUI.listaVinetas1[indexVineta].color.r,
                                                                          UIManager.InstanceGUI.listaVinetas1[indexVineta].color.g,
                                                                          UIManager.InstanceGUI.listaVinetas1[indexVineta].color.b,
                                                                          Mathf.MoveTowards(
                                                                          UIManager.InstanceGUI.listaVinetas1[indexVineta].color.a, 1f, (0.75f) * Time.deltaTime));
            yield return null;
        }


        indexVineta++;

        if (indexVineta > UIManager.InstanceGUI.listaVinetas1.Length - 1)
        {        
            indexVineta = 0;
            transicion = true;
            

        }


        if (!terminar)
        {
            pasarDiapo = true;

            colita.SetActive(true);

        }

       
        

    }


    void Update()
    {
        if (_map.Jugador.Interactuar.WasPressedThisFrame())
        {

            if (pasarDiapo && !terminar & !transicion)
            {
                StartCoroutine(PasarVinetas());
                boton.SetActive(false);
                colita.SetActive(false);
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);
            }
            else if (transicion)
            {
                UIManager.InstanceGUI.LoadNextScene();
				_map.Jugador.Disable();
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);
            }

            

        }

        if (_map.Jugador.SaltarEscena.WasPressedThisFrame())
        {

            if (pasarDiapo && !terminar & !transicion)
            {
                UIManager.InstanceGUI.LoadNextScene();
                _map.Jugador.Disable();
                AudioManager.Instance.PlaySound(AudioManager.Instance.pasarPagina);
            }

            

        }

    }
}
