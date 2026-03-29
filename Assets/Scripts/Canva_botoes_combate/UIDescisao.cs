using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(5f, 0, 2f);

    void Update()
    {
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.LookAt(Camera.main.transform);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
