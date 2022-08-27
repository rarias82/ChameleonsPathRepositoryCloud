using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject[] listOptions; // Lista de opciones
    [SerializeField] bool menuOn;
    [SerializeField] GameObject selector;
    [SerializeField] sbyte id_selector;


    void StartLevelOne()
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex + 1 ; 
        SceneManager.LoadScene(indexScene);
        
    }

    void ExitVideoGame()
    {
        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //Application.Quit;
        //#endif
    }
    void Navegate()
    {

        if (Input.GetButtonDown("Abajo")   && id_selector < listOptions.Length - 1)
        {
            id_selector++;
        }

        if (Input.GetButtonDown("Arriba") && id_selector > 0)
        {
            id_selector--;
        }

        selector.transform.position = listOptions[id_selector].transform.position;
        selector.transform.SetParent(listOptions[id_selector].transform);

        selector.transform.SetSiblingIndex(0);


        if (Input.GetButtonDown("Interactuar"))
        {
            switch (id_selector)
            {
                case 0:
                    StartLevelOne();
                    Debug.Log("You've choosen 0");
                    break;

                case 1:
                    Debug.Log("You've choosen 1");
                    break;

                case 2:

                    ExitVideoGame();
                    break;

                default:
                    break;
            }
        }

       
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuOn)
        {
            Navegate();
        }
    }
}
