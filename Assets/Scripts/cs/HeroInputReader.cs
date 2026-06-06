using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Chỉ chịu trách nhiệm nhận Input từ New Input System.
/// Không biết gì về physics, animation, hay game logic.
/// Các script khác đọc dữ liệu qua properties hoặc events.
/// </summary>
public class HeroInputReader : MonoBehaviour
{
    // ──────────────────────────────────────────────
    // Events — broadcast cho bất kỳ ai quan tâm
    // ──────────────────────────────────────────────
    public event System.Action<Vector2> OnMoveInput;   // gọi mỗi khi giá trị thay đổi
    public event System.Action          OnInteract;    // placeholder cho sau (talk, open chest...)

    // ──────────────────────────────────────────────
    // Properties — đọc trực tiếp nếu không muốn dùng event
    // ──────────────────────────────────────────────
    public Vector2 MoveInput { get; private set; }
    public bool    IsMoving  => MoveInput != Vector2.zero;

    // ──────────────────────────────────────────────
    // Private
    // ──────────────────────────────────────────────
    private PlayerInputActions _inputActions;

    // ──────────────────────────────────────────────
    // Lifecycle
    // ──────────────────────────────────────────────
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();

        _inputActions.Player.Move.performed += HandleMovePerformed;
        _inputActions.Player.Move.canceled  += HandleMoveCanceled;

        // Thêm action khác ở đây khi cần
        // _inputActions.Player.Interact.performed += HandleInteract;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= HandleMovePerformed;
        _inputActions.Player.Move.canceled  -= HandleMoveCanceled;

        _inputActions.Player.Disable();
    }

    // ──────────────────────────────────────────────
    // Handlers
    // ──────────────────────────────────────────────
    private void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        OnMoveInput?.Invoke(MoveInput);
    }

    private void HandleMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveInput = Vector2.zero;
        OnMoveInput?.Invoke(MoveInput);
    }

    // ──────────────────────────────────────────────
    // Public API — dùng khi cần khóa input (cutscene, combat...)
    // ──────────────────────────────────────────────
    public void EnableInput()
    {
        _inputActions.Player.Enable();
    }

    public void DisableInput()
    {
        MoveInput = Vector2.zero;
        OnMoveInput?.Invoke(MoveInput);
        _inputActions.Player.Disable();
    }
}
