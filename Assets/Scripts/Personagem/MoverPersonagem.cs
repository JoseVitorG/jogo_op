using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementRB : MonoBehaviour
{
    public InteractionUI interactionUI;
    public float velocidade = 4f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            input.x = (Keyboard.current.aKey.isPressed ? -1 : 0) +
                      (Keyboard.current.dKey.isPressed ? 1 : 0);

            input.y = (Keyboard.current.sKey.isPressed ? -1 : 0) +
                      (Keyboard.current.wKey.isPressed ? 1 : 0);
        }

        Vector3 movimento = (transform.forward * input.y + transform.right * input.x).normalized;
        rb.MovePosition(rb.position + movimento * velocidade * Time.fixedDeltaTime);
    }

    void OnMouseDown()
    {
        interactionUI.Show();
    }
}
