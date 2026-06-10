using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimController : MonoBehaviour
{
    private static readonly int IsMovingHash  = Animator.StringToHash("IsMoving");
    private static readonly int MoveXHash     = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash     = Animator.StringToHash("MoveY");
    private static readonly int LastMoveXHash = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveYHash = Animator.StringToHash("LastMoveY");

    private static readonly string[] RequiredParams = { "IsMoving", "MoveX", "MoveY", "LastMoveX", "LastMoveY" };

    private PlayerInputReader playerInput;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastMovement = Vector2.down;
    private bool parametersReady;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInputReader>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        parametersReady = ValidateAnimatorParameters();
    }

    private void Update()
    {
        if (!parametersReady) return;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 move = playerInput.MoveInput;
        bool isMoving = playerInput.IsMoving;

        anim.SetBool(IsMovingHash, isMoving);

        if (isMoving)
        {
            Vector2 snapped = SnapToCardinal(move);
            lastMovement = snapped;
            anim.SetFloat(MoveXHash, snapped.x);
            anim.SetFloat(MoveYHash, snapped.y);
        }
        else
        {
            anim.SetFloat(MoveXHash, 0f);
            anim.SetFloat(MoveYHash, 0f);
        }

        anim.SetFloat(LastMoveXHash, lastMovement.x);
        anim.SetFloat(LastMoveYHash, lastMovement.y);

        if (lastMovement.x != 0f)
            spriteRenderer.flipX = lastMovement.x < 0f;
    }

    // Snap diagonal input to the dominant axis to avoid blend tree flickering
    private static Vector2 SnapToCardinal(Vector2 input)
    {
        if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            return new Vector2(Mathf.Sign(input.x), 0f);
        return new Vector2(0f, Mathf.Sign(input.y));
    }

    private bool ValidateAnimatorParameters()
    {
        var existing = new HashSet<string>();
        foreach (var p in anim.parameters)
            existing.Add(p.name);

        bool allFound = true;
        foreach (var name in RequiredParams)
        {
            if (!existing.Contains(name))
            {
                Debug.LogWarning($"[AnimController] Animator thiếu parameter: '{name}'");
                allFound = false;
            }
        }
        return allFound;
    }
}
