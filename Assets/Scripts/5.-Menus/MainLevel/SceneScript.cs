using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float tiempo;
    Animator obAnim;


    private void Awake()
    {
        obAnim = GetComponent<Animator>();
            
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        int indiceNextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(SceneCargar(indiceNextScene));

    }

    public IEnumerator SceneCargar(int indScene)
    {
        obAnim.SetTrigger("Start");
        yield return new WaitForSeconds(tiempo);
        SceneManager.LoadScene(indScene);


    }
}
