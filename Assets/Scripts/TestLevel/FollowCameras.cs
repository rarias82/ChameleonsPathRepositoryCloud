using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modo
{
    InGame, InDialogue, Stop, Test
}
public class FollowCameras : MonoBehaviour
{
    public static FollowCameras instance; // Instancia para comunicarse con otros scripts 
    public Modo mode;

    [Header("Main Variables")]
    public Transform trPlayer;
    public Vector3 offset;
    [Range(0.01f, 10f)]
    [SerializeField] float smoothness;
    [Range(0.01f, 10f)]
    [SerializeField] float sensivityMouse;
    public Camera MyCameras;

    [Header("Limit Variables")]
    public float xSmooth = 8f;
    public float zSmooth = 8f;
    public Vector3 maxXandZ;
    public Vector3 minXandZ;
    public float posX, posZ;


    [Header("Dialogue Variables")]

    [SerializeField] float axisH;
    [SerializeField] float numero;
    public bool pararGiro;
    public Transform lis;
    [SerializeField] Transform las;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {    
        mode = Modo.InGame;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x,maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);
        transform.position = new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z);
       
    }

    void GirarDialogo()
    {
        axisH -= Time.deltaTime * numero;
        Quaternion camTurnAngle = Quaternion.AngleAxis(axisH * smoothness, Vector3.up);
        offset = camTurnAngle * offset;
        transform.position = Vector3.Slerp(transform.position, trPlayer.position + offset, xSmooth * Time.deltaTime);
        transform.LookAt(trPlayer);

        if (pararGiro)
        {
            axisH = 0;
            mode = Modo.Stop;
        }

    }

    void InGameCameras()
    {
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);
        transform.position = Vector3.Slerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), xSmooth * Time.deltaTime);
        transform.LookAt(trPlayer);
    }

    void StopMove()
    {
        //posX = trPlayer.position.x;
        //posZ = trPlayer.position.z;
        //float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
        //float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);
        //transform.position = Vector3.Slerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), xSmooth * Time.deltaTime);
        //transform.LookAt(trPlayer);

    }

    void Interludir()
    {
        axisH += Time.deltaTime * numero;
        Quaternion camTurnAngle = Quaternion.AngleAxis(axisH * smoothness, Vector3.up);
        offset = camTurnAngle * offset;
        transform.position = Vector3.Slerp(transform.position, trPlayer.position + offset, xSmooth * Time.deltaTime);
        transform.LookAt(trPlayer);

    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {

     

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (mode == Modo.InGame)
        {
            InGameCameras();
 

        }

        if (mode == Modo.InDialogue)
        {
            GirarDialogo();
        }

        if (mode == Modo.Test)
        {
            Interludir();
        }


        if (mode == Modo.Stop)
        {
            StopMove();
        }

       


        MyCameras.orthographicSize = Mathf.Clamp(MyCameras.orthographicSize, 3.5f, 7.5f); // Límites

    }
}
