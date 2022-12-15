using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum Modo
{
    InGame, InDialogue, Stop, Test, Mundo, MundoAlreves, Principio
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

    [Header("Mundo Variables")]
    public float velocidadRotacion = 20.0f;
    public ParticleSystem confetis;
    public ParticleSystem confetisB;
    public ParticleSystem confetisM;

    [Header("Principio Variables")]
    [SerializeField] Transform[] puntosASeguir;
    [SerializeField] float transitionSpeed;
    //[SerializeField] Transform puntoActual;
    public int indexSeguir = 0;

    //public VolumeProfile vp;


    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
       

    }

    private void OnEnable()
    {
        mode = Modo.Principio;
        //mode = Modo.InGame;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);
        //transform.position = new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z);
        confetis.Stop();
        confetisB.Stop();
        confetisM.Stop();
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

    void Mundo()
    {
        
        transform.RotateAround(trPlayer.position, Vector3.up, velocidadRotacion * Time.deltaTime );
        if (pararGiro)
        {
            velocidadRotacion = 0;
            mode = Modo.Stop;
        }
    }

    void MundoAlreves()
    {

        transform.RotateAround(trPlayer.position, Vector3.up, -velocidadRotacion * Time.deltaTime);
        if (pararGiro)
        {
            velocidadRotacion = 0;
            mode = Modo.InGame;
        }
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

    void Interpolar()
    {
        if (transitionSpeed<10.0f)
        {
            transitionSpeed += Time.deltaTime;
        }

        if (transitionSpeed >= 15.0f)
        {
            transitionSpeed = 15.0f;
        }
       

        Vector3 diferenciaVector = puntosASeguir[indexSeguir].position - transform.position;

        Vector3 actualAngle = new Vector3(
            Mathf.MoveTowards(transform.rotation.eulerAngles.x, puntosASeguir[indexSeguir].rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.MoveTowards(transform.rotation.eulerAngles.y, puntosASeguir[indexSeguir].rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.MoveTowards(transform.rotation.eulerAngles.z, puntosASeguir[indexSeguir].rotation.eulerAngles.z, Time.deltaTime * transitionSpeed)
              );
            
       
            
          

        if (diferenciaVector.sqrMagnitude < (0.2f * 2f))
        {
            indexSeguir++;

            if (indexSeguir >= puntosASeguir.Length)
            {
                indexSeguir = 0;
                mode = Modo.InGame;
                MainCharacter.sharedInstance._map.Jugador.Enable();

            }

        }

        transform.position = Vector3.MoveTowards(transform.position, puntosASeguir[indexSeguir].position, Time.deltaTime * transitionSpeed);
        transform.eulerAngles = actualAngle;

    }

    public void OnConfetis(int seleccion)
    {
        switch (seleccion)
        {

            case 0:
                confetis.Play();
                break;
                
            case 1:
                confetisB.Play();
                break;

            case 2:
                confetisM.Play();
                break;
            default:
                break;
        }
       
       


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

        if (mode == Modo.Mundo)
        {
            Mundo();
        }

        if (mode == Modo.MundoAlreves)
        {
            MundoAlreves();
        }

        if (mode == Modo.Principio)
        {
            Interpolar();
        }


        MyCameras.orthographicSize = Mathf.Clamp(MyCameras.orthographicSize, 3.5f, 7.5f); // Límites

    }
}
