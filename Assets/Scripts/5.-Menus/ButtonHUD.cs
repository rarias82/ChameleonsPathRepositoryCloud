using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHUD : MonoBehaviour
{
    [SerializeField] AudioClip startSound;
    public void StartGame()
    {
        AudioManager.Instance.PlaySound(startSound);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
