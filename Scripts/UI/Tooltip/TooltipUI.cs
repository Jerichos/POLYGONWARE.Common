using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common.UI
{
    public class TooltipUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        
        public void Open(TooltipData data)
        {
            _titleText.SetText(data.Title);
            _descriptionText.SetText(data.Description);
        }

        public void Close()
        {
            
        }
    }
}