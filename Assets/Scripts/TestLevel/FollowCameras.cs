using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modo
{
    InGame, InDialogue, InLimit
}
public class FollowCameras : MonoBehaviour
{
    public static FollowCameras instance; // Instancia para comunicarse con otros scripts 
    public Modo mode;

    [Header("Main Variables")]
    public Transform trPlayer;
    public Vector3 offset;
    public Vector3 initialOffset;
    public Vector3 dialogueOffset;
    [SerializeField] float posY;
    [Range(0.01f, 10f)]
    [SerializeField] float smoothness;
    [Range(0.01f, 10f)]
    [SerializeField] float sensivityMouse;
    public Camera MyCameras;

    [Header("Limit Variables")]
    public float xMargin = 1f;
    public float zMargin = 1f;
    public float xSmooth = 8f;
    public float zSmooth = 8f;
    public Vector3 maxXandZ;
    public Vector3 minXandZ;
    public float Margin;

    public float posX,posZ;
    public float TposX, TposZ;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        initialOffset = offset;
        mode = Modo.InGame;
        trPlayer = GameObject.FindGameObjectWithTag("Main Character").transform;

        posX = trPlayer.position.x;
        posZ = trPlayer.position.z;
        float X = Mathf.Clamp(posX, minXandZ.x,maxXandZ.x);
        float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);

        transform.position = new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z);
    }

    bool CheckXMargingX()
    {
        return (transform.position.x - trPlayer.position.x)< xMargin;
    }

    bool CheckXMargingZ()
    {
        return (transform.position.z - trPlayer.position.z) < zMargin;
    }

    bool CheckMarging()
    {
        return (transform.position.x > minXandZ.x && transform.position.x < maxXandZ.x);
    }
    void Tracking()
    {
        float targetX = trPlayer.position.x;
        float targetZ = trPlayer.position.z;

        
        targetX = Mathf.Clamp(targetX, minXandZ.x, maxXandZ.x);
        targetZ = Mathf.Clamp(targetZ, minXandZ.z, maxXandZ.z);
        //targetY = Mathf.Clamp(targetY, minXandY.y, maxXandY.y);

        //if (CheckXMargingX() || CheckXMargingZ())
        //{
        //    targetX = Mathf.Lerp(transform.position.x, trPlayer.position.x, xSmooth * Time.deltaTime);
        //    targetZ = Mathf.Lerp(transform.position.z, trPlayer.position.z, zSmooth * Time.deltaTime);
        //    //transform.position = Vector3.Lerp(transform.position, trPlayer.position, xSmooth * Time.deltaTime);
        //}

        //transform.position = new Vector3(targetX, transform.position.y, targetZ);

        transform.position = Vector3.Lerp(transform.position, new Vector3(posX,transform.position.y,posZ), xSmooth * Time.deltaTime);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (mode == Modo.InGame)
        {
            //Vector3 destiny = trPlayer.position + offset;

            //Vector3 newDestiny = new Vector3(destiny.x, destiny.y, destiny.z);

            posX = trPlayer.position.x;
            posZ = trPlayer.position.z;
            float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
            float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);

         
            
            transform.position = Vector3.Lerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), xSmooth * Time.deltaTime);
            

            //if (TposZ > minXandZ.z + offset.z && TposZ < maxXandZ.z)
            //{
            //    posZ = TposZ;
            //}



            

        }

        if (mode == Modo.InLimit)
        {
            //Vector3 destiny = trPlayer.position + offset;

            //Vector3 newDestiny = new Vector3(destiny.x, destiny.y, destiny.z);

            Tracking();
        }

        if (mode == Modo.InDialogue)
        {
            //Vector3 destiny = trPlayer.position + dialogueOffset;

            ////Vector3 newDestiny = new Vector3(destiny.x, destiny.y, destiny.z);

            //transform.position = Vector3.Lerp(transform.position, destiny, smoothness * Time.deltaTime);
        }
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensivityMouse, Vector3.up) * offset;

        //transform.LookAt(trPlayer);

        MyCameras.orthographicSize = Mathf.Clamp(MyCameras.orthographicSize, 3.5f, 7.5f); // Límites

    }
}
