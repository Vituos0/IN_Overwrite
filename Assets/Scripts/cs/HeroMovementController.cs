using UnityEngine;

/// <summary>
/// Chịu trách nhiệm di chuyển Rigidbody2D và điều khiển Animator.
/// Đọc input từ HeroInputReader — không biết gì về bàn phím hay gamepad.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HeroInputReader))]
public class HeroMovementController : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // Inspector
    // ──────────────────────────────────────────────
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    // ──────────────────────────────────────────────
    // References
    // ──────────────────────────────────────────────
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private HeroInputReader _inputReader;

    // ──────────────────────────────────────────────
    // State
    // ──────────────────────────────────────────────
    private Vector2 _moveInput;

    // Hướng nhìn cuối cùng khi đứng yên (dùng cho idle front/back và combat)
    private Vector2 _lastMoveDir = Vector2.down; // mặc định nhìn về phía trước

    // Animator parameter hashes
    private static readonly int HashSpeed = Animator.StringToHash("Speed");
    private static readonly int HashMoveX = Animator.StringToHash("MoveX");
    private static readonly int HashMoveY = Animator.StringToHash("MoveY");
    private static readonly int HashLastMoveY = Animator.StringToHash("LastMoveY");

    // ──────────────────────────────────────────────
    // Lifecycle
    // ──────────────────────────────────────────────
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputReader = GetComponent<HeroInputReader>();
    }

    private void OnEnable()
    {
        // Lắng nghe event từ InputReader thay vì tự poll mỗi frame
        _inputReader.OnMoveInput += HandleMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.OnMoveInput -= HandleMoveInput;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateAnimator();
    }

    // ──────────────────────────────────────────────
    // Event handler
    // ──────────────────────────────────────────────
    private void HandleMoveInput(Vector2 input)
    {
        _moveInput = input;

        if (input != Vector2.zero)
            _lastMoveDir = input;
    }

    // ──────────────────────────────────────────────
    // Movement
    // ──────────────────────────────────────────────
    private void ApplyMovement()
    {
        _rb.velocity = _moveInput * moveSpeed;
    }

    // ──────────────────────────────────────────────
    // Animation
    // ──────────────────────────────────────────────
    private void UpdateAnimator()
    {
        float speed = _moveInput.magnitude;

        _animator.SetFloat(HashSpeed, speed);
        _animator.SetFloat(HashMoveX, _moveInput.x);
        _animator.SetFloat(HashMoveY, _moveInput.y);

        // LastMoveY chỉ update khi đứng yên → giữ đúng idle front / back
        if (speed < 0.01f)
            _animator.SetFloat(HashLastMoveY, _lastMoveDir.y);

        // Flip sprite khi đi sang trái, chỉ khi đang di chuyển ngang
        if (Mathf.Abs(_moveInput.x) > 0.01f)
            _spriteRenderer.flipX = _moveInput.x < 0f;
    }

    // ──────────────────────────────────────────────
    // Public API
    // ──────────────────────────────────────────────

    /// <summary>
    /// Hướng hero đang nhìn — dùng để xác định hướng tấn công, spawn projectile, v.v.
    /// </summary>
    public Vector2 FacingDirection => _lastMoveDir;

    /// <summary>
    /// Dừng hẳn Rigidbody (gọi khi bị stun, knock, freeze...).
    /// </summary>
    public void StopImmediately()
    {
        _moveInput = Vector2.zero;
        _rb.velocity = Vector2.zero;
    }
}