using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace POLYGONWARE.Common.UI
{
    public class TooltipPanelUI : MonoBehaviour
    {
        [SerializeField] private Image _iconMage;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        
        public void Open(TooltipData data)
        {
            _titleText.SetText(data.Title);
            _descriptionText.SetText(data.Description);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}