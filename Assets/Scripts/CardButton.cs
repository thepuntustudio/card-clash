using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Color originalColor;

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            button.image.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = originalColor;
    }
}