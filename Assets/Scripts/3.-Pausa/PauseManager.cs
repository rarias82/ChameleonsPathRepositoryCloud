using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instancePausa;
    private void Awake()
    {
        if (instancePausa == null /*&& Instance != this*/)
        {
            instancePausa = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {

            Destroy(gameObject);
        }
    }


}
