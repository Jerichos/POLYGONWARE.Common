using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common.UI.application
{
public class ProfilerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        StartCoroutine(UpdateData());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator UpdateData()
    {
        while (enabled)
        {
            var currentFps = 1.0 / Time.deltaTime;
            _text.SetText(currentFps.ToString("0"));
            yield return new WaitForSeconds(1);
        }
    }
}
}