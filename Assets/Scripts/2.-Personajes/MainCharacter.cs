using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float intervalo, animSpeed, animIntervalo;

    [Header("Effects")]
    [SerializeField] ParticleSystem polvoTierra;
    [SerializeField] ParticleSystem.EmissionModule polvoTierraEmission;

    public Mapa _map;
    public bool botonIn;

    public NPC_Dialogue _leahn;
    public NPC_Rana _rana;


    void Awake()
    {
        sharedInstance = this;
        obAnim = transform.GetComponentInChildren<Animator>();

        _leahn = FindObjectOfType<NPC_Dialogue>();
        //_rana = FindObjectOfType<NPC_Rana>();
        _map = new Mapa();
        _map.Jugador.Enable();
        _map.Opciones.Enable();

     


    }

    private void OnEnable()
    {
        
    }

    public Vector3 ScreenDisplayPont(Vector3 posicionar)
    {
        Vector3 posDisplay = FollowCameras.instance.MyCameras.WorldToScreenPoint(posicionar);
        return posDisplay;



    }

   
    void MovePlayer()
    {


        Vector2 movementInput = _map.Jugador.Move.ReadValue<Vector2>();


        inputMove = new Vector3(movementInput.x, 0.0f, movementInput.y);

        vectorForAnim = inputMove;
        vectorForAnim = Vector3.ClampMagnitude(vectorForAnim, 1);

        float yStore = inputPlayer.y;
        inputPlayer.y = yStore;

        intervalo = (Mathf.Abs(vectorForAnim.x) + Mathf.Abs(vectorForAnim.z));

        intervalo = Mathf.Clamp(intervalo, 0f, 1f);

        animIntervalo = intervalo;

        if (movementInput.x != 0.0f || movementInput.y != 0.0f)
        {



            float anguloARotar = Mathf.Atan2(inputMove.x, inputMove.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;

            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloARotar, ref velocidadRotacionSuave, rotacionSuave);

            transform.rotation = Quaternion.Euler(0f, angulo, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, anguloARotar, 0f) * Vector3.forward;

            Vector3.ClampMagnitude(moveDirection, 1f);



            if (_map.Jugador.Sprintar.WasPressedThisFrame())
            {
                polvoTierra.Play();
            }


            if (_map.Jugador.Sprintar.IsPressed())
            {
                moveSpeed = (MaxmoveSpeed + 3.25f);

                intervalo = 1.0f;

                animIntervalo = intervalo + 0.5f;

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
            cc.slopeLimit = 0f;
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

        polvoTierra.Stop();
        _map = new Mapa();
        _leahn.SetInputActions(_map);
        //_rana.SetInputActions(_map);

        _map.Jugador.Enable();
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
        
        obAnim.SetFloat("Velocidad", animIntervalo);



        if (animIntervalo == 0.0f)
        {
            obAnim.speed = 1.0f;
        }

        if (animIntervalo > 0.0f && animIntervalo < 1f)
        {
            obAnim.speed = 2f;
        }

        if (animIntervalo == 1f)
        {
            obAnim.speed = 2.5f;
        }

        if (animIntervalo > 1f)
        {
            obAnim.speed = animSpeed + 0.5f; 
        }

        //switch (animIntervalo)
        //{
        //    case 0.0f:
        //        obAnim.speed = 1.0f;

        //        break;

        //    case 0.1f:
        //        obAnim.speed = 10.0f;

        //        break;



        //    case 0.5f:
        //        obAnim.speed = 15.0f;

        //        break;

        //    case 0.8f:
        //        obAnim.speed = 25.0f;

        //        break;

        //    case 1f:

        //        obAnim.speed = animSpeed + 0.75f;
        //        break;

        //    case 1.5f:

        //        obAnim.speed = animSpeed + 1.15f;

        //        break;

        //    default:
        //        break;
        //}

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
