using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);


        transform.position = Vector3.Lerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), xSmooth * Time.deltaTime);
    }
}
