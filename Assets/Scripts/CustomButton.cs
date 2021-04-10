using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{

    private void Update()
    {
        if (CustomEventSystem.current.selectOnHighlight && IsHighlighted())
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
