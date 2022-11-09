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
    public float speedNPC;
    //public float distancia;
    void Walking()
    {
        //diferenciaVector = trCaminos[numeroCamino] - transform.position;

        //if (diferenciaVector.sqrMagnitude < (0.5f * 2f))
        //{
        //    numeroCamino++;

        //    if (numeroCamino >= trCaminos.Length)
        //    {
        //        numeroCamino = 0;
        //    }

        //}

        //transform.position = Vector3.Lerp(transform.position, trCaminos[numeroCamino], speedNPC * Time.deltaTime);
        if (transform.position.z >=)
        {

        }
        transform.position += new Vector3(0,0,1).normalized * speedNPC * Time.deltaTime;


    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walking();
    }


}
