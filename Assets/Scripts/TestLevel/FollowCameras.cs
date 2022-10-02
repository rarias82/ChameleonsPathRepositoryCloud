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

    // Update is called once per frame
    void LateUpdate()
    {
        if (mode == Modo.InGame)
        {

            posX = trPlayer.position.x;
            posZ = trPlayer.position.z;
            float X = Mathf.Clamp(posX, minXandZ.x, maxXandZ.x);
            float Z = Mathf.Clamp(posZ, minXandZ.z, maxXandZ.z);

            
            transform.position = Vector3.Lerp(transform.position, new Vector3(X + offset.x, trPlayer.position.y + offset.y, Z + offset.z), xSmooth * Time.deltaTime);
            



            

        }

      

        if (mode == Modo.InDialogue)
        {
            
        }
       

        MyCameras.orthographicSize = Mathf.Clamp(MyCameras.orthographicSize, 3.5f, 7.5f); // Límites

    }
}
