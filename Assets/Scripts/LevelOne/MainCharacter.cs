using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

//using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{
    public static MainCharacter sharedInstance;

    [Header("Cameras")]
    [SerializeField] Transform playerCamera;

    [Header("Move Variables")]
    //[SerializeField] PlayerInput playerInput;
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
    Animator obAnim;
    public float intervalo, animSpeed;

    [Header("Effects")]
    [SerializeField] ParticleSystem polvoTierra;
    [SerializeField] ParticleSystem.EmissionModule polvoTierraEmission;


    void Awake()
    {
        sharedInstance = this;
        obAnim = transform.GetComponentInChildren<Animator>();
        
    }


    //void OnMovimiento(InputValue valor )
    //{
    //    Vector2 inputMovimiento = valor.Get<Vector2>();
    //    inputMove = new Vector3(inputMovimiento.x, 0.0f, inputMovimiento.y);

    //}
    void MovePlayer()
    {


        float axH = Input.GetAxis("Horizontal");
        float axV = Input.GetAxis("Vertical");

        inputMove = new Vector3(axH, 0.0f, axV);

        vectorForAnim = inputMove;
        vectorForAnim = Vector3.ClampMagnitude(vectorForAnim, 1);

        float yStore = inputPlayer.y;
        inputPlayer.y = yStore;

        intervalo = (Mathf.Abs(vectorForAnim.x) + Mathf.Abs(vectorForAnim.z));

        intervalo = Mathf.Clamp(intervalo, 0f, 1f);


        if (axH != 0.0f || axV != 0.0f)
        {



            float anguloARotar = Mathf.Atan2(inputMove.x, inputMove.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;

            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloARotar, ref velocidadRotacionSuave, rotacionSuave);

            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, anguloARotar, 0f) * Vector3.forward;

            Vector3.ClampMagnitude(moveDirection, 1f);




            if (Input.GetButton("Sprint"))
            {
                moveSpeed = (MaxmoveSpeed + 2.5f);

                intervalo = 1.0f;

            }
            else
            {

                moveSpeed = MaxmoveSpeed;

            }

            cc.Move(moveDirection * (moveSpeed * (intervalo)) * Time.deltaTime);

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

        inputPlayer.y += forceGravity * Time.deltaTime * forceGravityScale;

        cc.Move(inputPlayer * Time.deltaTime);
    }
    void Start()
    {
        // Incicializar las variables principales
        playerCamera = Camera.main.transform;
        moveSpeed = MaxmoveSpeed;
        canMove = true;
        cc = GetComponent<CharacterController>();
        polvoTierraEmission = polvoTierra.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }

       


    }

    private void LateUpdate()
    {
        obAnim.SetFloat("Velocidad", intervalo);

		switch (intervalo)
		{
            case 0.0f:
                obAnim.speed = 1.0f;
                polvoTierraEmission.rateOverTime = 0.0f;
                break;

            case 0.5f:
                obAnim.speed = 2.0f;
                polvoTierraEmission.rateOverTime = 2.5f;
                break;

            case 1f:
                polvoTierraEmission.rateOverTime = 10f;
                obAnim.speed = animSpeed;
                break;

            default:
				break;
		}
            
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * groundDistance), radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
           
        }
    }

}
