using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pez : MonoBehaviour
{
    // Start is called before the first frame update
    //[Header("Move Variables")]
    //public sbyte numeroCamino;

    //[SerializeField] Vector3[] trCaminos;
    //Vector3 diferenciaVector;
    public Transform model;
    public float speedNPC;
    bool adelante = true;
    //public float distancia;
    void Walking()
    {
        
        if (transform.position.z >= 84.0f)
        {
            adelante = false;
        }

        if (transform.position.z <= 16.0f)
        {
            adelante = true;
        }


        if (adelante)
        {
            transform.position += Vector3.forward.normalized * speedNPC * Time.deltaTime;
            //model.rotation = Quaternion.Euler(-89.98f, 0f, 0f);
            model.localScale = new Vector3(model.localScale.x, 100.0f, model.localScale.z);
        }
        else
        {
            transform.position += Vector3.back.normalized * speedNPC * Time.deltaTime;
            //model.rotation = Quaternion.Euler(0f, -180f, 0f);
            model.localScale = new Vector3(model.localScale.x, -100.0f, model.localScale.z);
        }
        


    }
    void Start()
    {
        model = transform.Find("Fish_Armature").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Walking();
    }


}
