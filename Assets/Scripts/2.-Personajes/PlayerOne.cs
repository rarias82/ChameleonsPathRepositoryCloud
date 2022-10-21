using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOne : MonoBehaviour
{
    public static PlayerOne sharedInstance;

    [Header("Cameras")]
    [SerializeField] Transform playerCamera;

    [Header("Move Variables")]
    public bool canMove;
    [SerializeField] float MaxmoveSpeed;
    [SerializeField] float moveSpeed;
    public Vector3 inputPlayer;
    public Vector3 inputMove;
    [SerializeField] CharacterController cc;
    [SerializeField] float rotacionSuave;
    float velocidadRotacionSuave;

    [Header("Gravity Variables")]
    [SerializeField] float forceGravity;
    [SerializeField] float forceGravityScale;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundDistance;
    public LayerMask layerGround;
    [SerializeField] float radius;

    [Header("Anim variables")]
    ////public Animator obAnim;
    public Vector3 vectorForAnim;

    void Awake()
    {
        sharedInstance = this;
    }
    void MovePlayer()
    {

        float axH = Input.GetAxis("Horizontal");
        float axV = Input.GetAxis("Vertical");

        inputMove = new Vector3(axH, 0.0f, axV);

        vectorForAnim = inputMove;
        vectorForAnim = Vector3.ClampMagnitude(vectorForAnim, 1);

        float yStore = inputPlayer.y;
        inputPlayer.y = yStore;



        if (axH != 0.0f || axV != 0.0f)
        {
            float anguloARotar = Mathf.Atan2(inputMove.x, inputMove.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloARotar, ref velocidadRotacionSuave, rotacionSuave);
                
            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, anguloARotar, 0f) * Vector3.forward;

            float newSpeed = (Mathf.Abs(vectorForAnim.x) + Mathf.Abs(vectorForAnim.z)) * moveSpeed;

            cc.Move(moveDirection.normalized * newSpeed * Time.deltaTime);
        }

        MoveGravity();
    }
    void MoveGravity()
    {

        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * groundDistance, radius, layerGround);

        if (isGrounded && inputPlayer.y < 0.0f)
        {
            cc.slopeLimit = 45f;
            inputPlayer.y = -2.0f;
        }

        if (!isGrounded)
        {

            cc.slopeLimit = 120f;

            if ((cc.collisionFlags & CollisionFlags.Above) != 0)
            {
                inputPlayer.y = -2.0f;
            }
        }
        else
        {
            moveSpeed = MaxmoveSpeed;
            rotacionSuave = 0.25f;
        }

        inputPlayer.y += forceGravity * Time.deltaTime * forceGravityScale;

        cc.Move(inputPlayer * Time.deltaTime);
    }
    void Start()
    {
        // Incicializar las variables principales
        playerCamera = Camera.main.transform;
        moveSpeed = MaxmoveSpeed;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * groundDistance), radius);
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
