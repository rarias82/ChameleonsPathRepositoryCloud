using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMap : MonoBehaviour
{
    public Vector3 offset;
    [Range(0.01f, 10f)]
    [SerializeField] float smoothness;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, FollowCameras.instance.trPlayer.position + offset, smoothness * Time.deltaTime);
    }
}
