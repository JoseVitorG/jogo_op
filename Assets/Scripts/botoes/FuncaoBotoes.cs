using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public Image image;
    public Color normal = Color.white;
    public Color hover = Color.gray;
    public Color click = Color.black;

    void Start()
    {
        image.color = normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = click;
    }
}
