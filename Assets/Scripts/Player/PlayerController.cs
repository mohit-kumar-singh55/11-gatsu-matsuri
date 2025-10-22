using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    #region Input Fields
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float sprintSpeed = 5f;
    [SerializeField] float jumpForce = 400f;
    [SerializeField] float groundCheckDistance = .3f;
    [Tooltip("スタミナが0になるまでの秒数")]
    [SerializeField] float staminaInSeconds = 4f;
    [Tooltip("スタミナが最大まで回復するまでの秒数")]
    [SerializeField] float timeToRegenerateStamina = 4f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CinemachineCamera cm_cam;
    #endregion

    #region Properties
    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;
    // private CameraController cameraController;
    private bool isIdle = false;
    private bool isSprinting = false;
    private bool playerFreezed = false;      // to freeze player while game over
    private float _velocity = 0f;
    private float _curStamina;
    private int _velocityHash;
    private int _standingJumpHash;
    private int _runningJumpHash;

    // animator variables (アニメーター変数)
    const string ANIM_SPEED = "Velocity";
    const string ANIM_STANDING_JUMP = "StandingJump";
    const string ANIM_RUNNING_JUMP = "RunningJump";
    #endregion

    void Awake()
    {
        // ** singleton **
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // ** getting components **
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        // cameraController = GetComponent<CameraController>();

        // ** hiding cursor (カーソルを隠す) **
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ** initializing (初期化) **
        _curStamina = staminaInSeconds;
    }

    void Start()
    {
        _velocityHash = Animator.StringToHash(ANIM_SPEED);
        _standingJumpHash = Animator.StringToHash(ANIM_STANDING_JUMP);
        _runningJumpHash = Animator.StringToHash(ANIM_RUNNING_JUMP);
    }

    void Update()
    {
        if (rb.linearVelocity == Vector3.zero || rb.linearVelocity.magnitude < .05f) isIdle = true;
        else isIdle = false;
    }

    void FixedUpdate()
    {
        HandleMove();
        HandleStamina();

        Debug.Log(_curStamina);
    }

    // ** Input System - Callbacks (入力システム - コールバック) **
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        HandleJump();
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.Get<float>() == 1f;
    }

    // ** freeze player movements (プレイヤーの動きを止める) **
    public void FreezePlayer(bool freeze = true) => playerFreezed = freeze;

    // ** handling player movements (プレイヤーの動きを制御する) **
    void HandleMove()
    {
        if (playerFreezed) return;

        // Flatten the camera's forward and right (カメラの前方と右方向を平面化)
        Vector3 camForward = cm_cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cm_cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // ** Calculate movement direction (移動方向を計算) **
        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        move.Normalize();           // Prevent faster diagonal movement (斜め移動を防ぐ)

        // Rotate player to face movement direction (プレイヤーを移動方向に向ける)
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // ** moving player (プレイヤーを移動させる) **
        bool canSprint = isSprinting && _curStamina > 0f;
        Vector3 targetVelocity = move * (canSprint ? sprintSpeed : walkSpeed);
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        UpdateMoveAnimation();
    }

    // ** updating move animation in blend tree (移動アニメーションを更新する) **
    void UpdateMoveAnimation()
    {
        float targetVelocity;

        if (isIdle) targetVelocity = 0f;
        else targetVelocity = rb.linearVelocity.magnitude / sprintSpeed;

        _velocity = Mathf.Lerp(_velocity, targetVelocity, Time.deltaTime * 5f);

        animator.SetFloat(_velocityHash, _velocity);
    }

    void HandleJump()
    {
        if (IsGrounded() && !playerFreezed)
        {
            if (isIdle) animator.SetTrigger(_standingJumpHash);
            else animator.SetTrigger(_runningJumpHash);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

    void HandleStamina()
    {
        if (!isSprinting && _curStamina >= staminaInSeconds) return;

        if (isSprinting && moveInput != Vector2.zero) _curStamina -= Time.deltaTime;
        else _curStamina += (staminaInSeconds / timeToRegenerateStamina) * Time.deltaTime;

        _curStamina = Mathf.Clamp(_curStamina, 0f, staminaInSeconds);
    }
}
