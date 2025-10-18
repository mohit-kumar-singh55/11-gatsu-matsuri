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
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance = .3f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CinemachineCamera cm_cam;
    #endregion

    #region Properties
    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;
    // private CameraController cameraController;
    private bool isSprinting = false;
    private bool playerFreezed = false;      // to freeze player while game over
    private float _velocity = 0f;
    private int _velocityHash;

    // animator variables (アニメーター変数)
    const string ANIM_SPEED = "Velocity";
    const string ANIM_JUMP = "jump";
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
    }

    void Start()
    {
        _velocityHash = Animator.StringToHash(ANIM_SPEED);
    }

    void FixedUpdate()
    {
        HandleMove();
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
        Vector3 targetVelocity = move * (isSprinting ? sprintSpeed : walkSpeed);
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        UpdateMoveAnimation();
    }

    void HandleJump()
    {
        if (IsGrounded() && !playerFreezed)
        {
            animator.SetTrigger(ANIM_JUMP);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ** updating move animation in blend tree (移動アニメーションを更新する) **
    void UpdateMoveAnimation()
    {
        float targetVelocity;

        if (rb.linearVelocity == Vector3.zero || rb.linearVelocity.magnitude < .05f) targetVelocity = 0f;
        else targetVelocity = rb.linearVelocity.magnitude / sprintSpeed;

        _velocity = Mathf.Lerp(_velocity, targetVelocity, Time.deltaTime * 5f);

        animator.SetFloat(_velocityHash, _velocity);
    }

    bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
}
