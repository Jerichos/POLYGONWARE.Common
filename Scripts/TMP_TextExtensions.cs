using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common
{
    public static class TMP_TextExtensions
    {
        public static void SetText(this TMP_Text tmpText, string text, Color color)
        {
            tmpText.SetText(text);
            tmpText.color = color;
        }
    }
}