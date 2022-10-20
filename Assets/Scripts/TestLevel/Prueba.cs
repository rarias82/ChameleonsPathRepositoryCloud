using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    [Header("Main Variables")]
    public Transform trPlayer;
    [Range(0.01f, 10f)]
    [SerializeField] float smoothness;
    [Range(0.01f, 10f)]
    [SerializeField] float sensivityMouse;
    [SerializeField] Vector3 offset;
    [SerializeField] float axisH;
    [SerializeField] float numero;
    [SerializeField] Quaternion angles;

    private void Start()
    {
        angles = Quaternion.Euler(30, 45, 0);
    }
    void GirarDialogo()
    {

        Quaternion camTurnAngle = Quaternion.AngleAxis(axisH * smoothness, Vector3.up);
        offset = camTurnAngle * offset;
        transform.position = Vector3.Slerp(transform.position, trPlayer.position + offset, smoothness * Time.deltaTime);
        transform.LookAt(trPlayer);

    }
    private void Update()
    {
        axisH -= Time.deltaTime * numero;
        Debug.Log(axisH);
    }
    void LateUpdate()
    {

        GirarDialogo();

        //transform.rotation = Quaternion.Slerp(transform.rotation, angles, smoothness * Time.deltaTime);
    }

    

       

    
}
