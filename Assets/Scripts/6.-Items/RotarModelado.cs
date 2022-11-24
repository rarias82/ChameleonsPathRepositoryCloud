using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarModelado : MonoBehaviour
{
    public Vector3 rot_;
    public float speedRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rot_ /** speedRot*/ * Time.deltaTime);
    }
}
