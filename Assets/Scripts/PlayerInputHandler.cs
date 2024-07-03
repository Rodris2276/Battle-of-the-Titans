using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour 
{
    private Mover mover;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        mover.OnMove(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        mover.OnJump(context);
    }
}
