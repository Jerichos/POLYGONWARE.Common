using UnityEngine;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common
{
public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SAttribute<bool> Hover;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover.Value = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hover.Value = false;
    }
}

}