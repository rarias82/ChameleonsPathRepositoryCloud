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

        if (indexVineta==3)
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



            while (UIManager.InstanceGUI.listaVinetas1[8].color.a != 0)
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





            }








            UIManager.InstanceGUI.LimpiarCOmic();

            UIManager.InstanceGUI.paletaComicsFinal.SetActive(true);


            terminar = true;

            indexVineta = 0;


            //UIManager.InstanceGUI.textoComics.text = string.Empty;

            //foreach (char letter in UIManager.InstanceGUI.comicTextos.ToCharArray())
            //{
            //    UIManager.InstanceGUI.textoComics.text += letter;
            //    yield return new WaitForSeconds(0.055f);
            //}

            //UIManager.InstanceGUI.textoComics.text = string.Empty;


            UIManager.InstanceGUI.LoadNextScene();
            _map.Jugador.Disable();
        }




        if (!terminar)
        {
            pasarDiapo = true;

            colita.SetActive(true);

            //if (indexVineta == 3 || indexVineta == 8)
            //{
            //    boton.SetActive(false);
            //    colita.SetActive(true);
            //}
            //else
            //{
            //    boton.SetActive(true);
            //    colita.SetActive(false);
            //}
        }

       
        

    }

    // Update is called once per frame
    void Update()
    {
        if (_map.Jugador.Interactuar.WasPressedThisFrame() && pasarDiapo && !terminar)
        {
            StartCoroutine(PasarVinetas());
            boton.SetActive(false);
            colita.SetActive(false);




        }   
    }
}
