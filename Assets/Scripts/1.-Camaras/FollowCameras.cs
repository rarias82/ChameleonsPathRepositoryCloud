using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum Modo
{
    InGame, Stop, Mundo, Odnum, MundoAlreves, Principio, Relleno
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
    public float maxDistance;
    public float currentDistance;
    public float limiteParaDetenerCamra;
    public LayerMask whatDetect;
    public GameObject rayosLaser;
    public float radiosShepre;
    RaycastHit rh;
    Ray ray;
    public bool detenerGiro;

    [Header("Principio Variables")]
    [SerializeField] Transform[] puntosASeguir;
    [SerializeField] float transitionSpeed;
    public int indexSeguir = 0;

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
		trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;
        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);
        confetis.Stop();
        confetisB.Stop();
        confetisM.Stop();
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
        //UIManager.InstanceGUI.BurbujaDialogo(9);

        transform.RotateAround(trPlayer.position, Vector3.up, velocidadRotacion * Time.deltaTime );

        ray = new Ray(rayosLaser.transform.position, rayosLaser.transform.forward);

		if (Physics.SphereCast(ray,radiosShepre, out rh,maxDistance,whatDetect, QueryTriggerInteraction.Ignore))
		{
			if (rh.transform.gameObject.CompareTag("Face") && detenerGiro)
			{
                pararGiro = true;
                detenerGiro = false;
                mode = Modo.Stop;
            }
		}
    }

    void Odnum()
    {

        

        transform.RotateAround(trPlayer.position, Vector3.up, -velocidadRotacion * Time.deltaTime);

        ray = new Ray(rayosLaser.transform.position, rayosLaser.transform.forward);
      
        if (Physics.SphereCast(ray, radiosShepre, out rh, maxDistance, whatDetect, QueryTriggerInteraction.Ignore))
        {
            if (rh.transform.gameObject.CompareTag("FaceAtras") && detenerGiro)
            {
                pararGiro = true;
                detenerGiro = false;
                mode = Modo.Stop;
            }
        }

	}

    void Relleno()
    {
        //UIManager.InstanceGUI.BurbujaDialogo(9);

        transform.RotateAround(trPlayer.position, Vector3.up, velocidadRotacion * Time.deltaTime);

        ray = new Ray(rayosLaser.transform.position, rayosLaser.transform.forward);

        if (Physics.SphereCast(ray, radiosShepre, out rh, maxDistance, whatDetect, QueryTriggerInteraction.Ignore))
        {
            if (rh.transform.gameObject.CompareTag("Face") && detenerGiro)
            {
                pararGiro = true;
                detenerGiro = false;
                mode = Modo.Stop;
            }
        }
    }

    void MundoAlreves()
    {

        transform.RotateAround(trPlayer.position, Vector3.up, -velocidadRotacion * Time.deltaTime);

        if (!pararGiro)
        {
            velocidadRotacion = 0;
            mode = Modo.InGame;
        }
    }

    void StopMove()
    {
        velocidadRotacion = 0;
        pararGiro = false;
        detenerGiro = true;
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
				UIManager.InstanceGUI.ShowHUDInGame();
				//Vuelva a activar

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
        if (mode == Modo.Principio)
        {
            Interpolar();
        }

        if (mode == Modo.InGame)
        {
            InGameCameras();
 
        }

        if (mode == Modo.Stop)
        {
            StopMove();
        }

        if (mode == Modo.Mundo)
        {
            Mundo();
        }

        if (mode == Modo.Odnum)
        {
            Odnum();
        }

        if (mode == Modo.MundoAlreves)
        {
            MundoAlreves();
        }

        if (mode == Modo.Relleno)
        {
            Relleno();
        }




        MyCameras.orthographicSize = Mathf.Clamp(MyCameras.orthographicSize, 3.5f, 7.5f); // Límites

    }

	private void OnDrawGizmosSelected()
	{

        Gizmos.color = Color.green;
        Debug.DrawLine(ray.origin, ray.direction * currentDistance);
        Gizmos.DrawWireSphere(ray.direction * currentDistance,radiosShepre);
	}
}
