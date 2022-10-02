using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinCam : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Main Variables")]
    public Transform trPlayer;
    public Vector3 offset;
   

    public float posX, posZ;
  

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, FollowCameras.instance.minXandZ.x, FollowCameras.instance.maxXandZ.x);
        float Z = Mathf.Clamp(posZ, FollowCameras.instance.minXandZ.z, FollowCameras.instance.maxXandZ.z);


        transform.position = Vector3.Lerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), FollowCameras.instance.xSmooth * Time.deltaTime);
    }
}
