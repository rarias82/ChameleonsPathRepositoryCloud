using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class comic : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite[] vinetas;
    public Image colocarImagen;
    

    IEnumerator Diapos()
    {
        for (int i = 0; i < vinetas.Length; i++)
        {
            //vinetas[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < vinetas.Length; i++)
        {
            //UIManager.InstanceGUI.obAnim.SetTrigger("StartTransition");

            colocarImagen.sprite = vinetas[i]; //vinetas[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(5f);

            
        }


        yield return new WaitForSeconds(5f);

        UIManager.InstanceGUI.StartCoroutine(UIManager.InstanceGUI.SceneLoading(2));






    }
    void OnEnable()
    {
        StartCoroutine(Diapos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
