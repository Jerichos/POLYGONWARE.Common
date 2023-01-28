using POLYGONWARE.Common.Util;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common
{
    public interface ISelectable : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        event VoidDelegate ESelected;
        event VoidDelegate EDeselected;
        
        void Select();
        void Deselect();
    }
}