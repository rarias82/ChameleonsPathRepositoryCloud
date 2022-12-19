using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir.z = MainCharacter.sharedInstance.transform.eulerAngles.y;
        transform.localEulerAngles = dir;
    }
}
