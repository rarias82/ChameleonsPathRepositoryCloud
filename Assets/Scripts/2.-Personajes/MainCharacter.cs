using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MainCharacter : MonoBehaviour
{
    public static MainCharacter sharedInstance;

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
    public GameObject cara;

    [Header("Gravity Variables")]
    [SerializeField] float forceGravity;
    [SerializeField] float forceGravityScale;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundDistance;
    public LayerMask layerGround;
    [SerializeField] float radius;
    
    [Header("Anim variables")]
    public Vector3 vectorForAnim;
    public Animator obAnim;
    public float intervalo, animSpeed, animIntervalo;
    public int eAnim;

    [Header("Effects")]
    [SerializeField] ParticleSystem polvoTierra;
    [SerializeField] ParticleSystem.EmissionModule polvoTierraEmission;
    [SerializeField] AudioClip[] vocesProtagonista;
    int rvozLogan0, rvozLogan1;

    public Mapa _map;
    public bool botonIn;

    public NPC_Dialogue _leahn;
    public NPC_Rana _rana;

    [Header("Pausas")]
    public bool puedePausar = true;
    public Vector3 mInput;

    [Header("Capas Outline")]
    public GameObject capaObj;
    public LayerMask capa;
    public LayerMask capa0;

    void Awake()
    {
        sharedInstance = this;
    }

    private void OnEnable()
    {
        puedePausar = true;
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
    public void VozLogan()
    {
        rvozLogan1 = rvozLogan0;
        rvozLogan0 = Random.Range(0, vocesProtagonista.Length);

        while (rvozLogan0 == rvozLogan1)
        {
            rvozLogan0 = Random.Range(0, vocesProtagonista.Length);
        }

        AudioManager.Instance.PlaySound(vocesProtagonista[rvozLogan0]);
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
        obAnim = transform.GetComponentInChildren<Animator>();
        _leahn = FindObjectOfType<NPC_Dialogue>();
        playerCamera = Camera.main.transform;
        moveSpeed = MaxmoveSpeed;
        canMove = true;
        cc = GetComponent<CharacterController>();
        polvoTierraEmission = polvoTierra.emission;
        polvoTierra.Stop();
        _map = new Mapa();
		_leahn.SetInputActions(_map); // VOLBER A ACTIVAR
        cara = transform.Find("Cara").gameObject;

        //_map.Jugador.Enable();
    }
    void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }

        mInput= _map.Jugador.Move.ReadValue<Vector2>().normalized;

        if (puedePausar)
        {
            if (_map.Jugador.Pausa.WasPressedThisFrame()  && mInput.magnitude == 0)
            {
                UIManager.InstanceGUI.MostrarCartelPausa();
                AudioManager.Instance.PlaySound(AudioManager.Instance.pausas);
            }
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

        obAnim.SetInteger("Animo", eAnim);

       
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * groundDistance), radius);
    }

}
