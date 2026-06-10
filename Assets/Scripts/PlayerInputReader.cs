using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInputActions playerControls;

    public Vector2 MoveInput { get; private set; }
    public bool IsMoving => MoveInput != Vector2.zero;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    private void Update()
    {
        MoveInput = playerControls.Player.Move.ReadValue<Vector2>();
    }
}
