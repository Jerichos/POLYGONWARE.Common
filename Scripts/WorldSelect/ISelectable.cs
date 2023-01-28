using POLYGONWARE.Common.Util;
using UnityEngine.EventSystems;

namespace POLYGONWARE.Common
{
    public interface ISelectable : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        static event GenericDelegate<ISelectable> ESelected; 
        void Select();
        void Deselect();
        void OnSelect();
        void OnDeselect();
    }
}