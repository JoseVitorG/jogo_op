using UnityEngine;
using UnityEngine.InputSystem;

public class HorrorCameraFinal : MonoBehaviour
{
    public Transform target;
    public InteractionUI interactionUI;

    public Vector3 offsetNormal = new Vector3(0, 2f, -4f);
    public Vector3 offsetPerto = new Vector3(0, 1.8f, -2f);

    public float suavizacao = 6f;
    public float intensidadeMouse = 0.12f;

    Vector3 offsetAtual;
    Vector3 offsetMouse;
    bool zoom;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        offsetAtual = offsetNormal;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.root == target)
                {
                    zoom = !zoom;

                    if (zoom){
                        interactionUI.Show();
                    }
                    else{
                        interactionUI.Hide();
                    }
                }
            }
        }

        Vector2 mouse = Mouse.current.delta.ReadValue();
        offsetMouse = Vector3.Lerp(
            offsetMouse,
            new Vector3(mouse.x, mouse.y, 0) * intensidadeMouse,
            Time.deltaTime * 8f
        );
    }

    void LateUpdate()
    {
        Vector3 offsetBase = zoom ? offsetPerto : offsetNormal;
        Vector3 alvo = offsetBase + offsetMouse;

        offsetAtual = Vector3.Lerp(offsetAtual, alvo, suavizacao * Time.deltaTime);

        transform.position = target.position + offsetAtual;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
