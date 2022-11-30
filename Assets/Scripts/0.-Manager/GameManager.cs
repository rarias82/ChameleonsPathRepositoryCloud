using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager InstancieInput;
    public Mapa mapeoControles;
    bool activado = false;
    private void Awake()
    {
        if (InstancieInput == null /*&& Instance != this*/)
        {
            InstancieInput = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {

            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        mapeoControles = new Mapa();
        

    }

    private void Start()
    {
        
      
    }

    public void ActivarInput()
    {


        activado = !activado;

        if (activado)
        {
            mapeoControles.Jugador.Enable();
        }
        else
        {
            mapeoControles.Jugador.Disable();
        }
        

    }

    


}
