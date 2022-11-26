using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHit : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] Transform rayo; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(rayo.transform.position, rayo.transform.TransformDirection(direction * range));
        Debug.DrawRay(rayo.transform.position, rayo.transform.TransformDirection(direction * range));

      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FollowCameras.instance.mode == Modo.Mundo)
        {
            if (other.gameObject.CompareTag("AnguloIndicado"))
            {


                FollowCameras.instance.pararGiro = true;



            }
        }

        if (FollowCameras.instance.mode == Modo.MundoAlreves)
        {
            if (other.gameObject.CompareTag("AnguloIndicadoS"))
            {


                FollowCameras.instance.pararGiro = true;



            }
        }




    }
}
