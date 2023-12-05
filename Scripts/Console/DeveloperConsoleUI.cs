using System;
using POLYGONWARE.Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace POLYGONWARE.Common
{

[DefaultExecutionOrder(-100)]
public class DeveloperConsoleUI : BaseUI
{
    [SerializeField] private TMP_Text _consoleTextField;
    
    private string _consoleLog;
    private const int MAX_LOGS = 1000;
    private int _logs;

    protected override void Awake()
    {
        Debug.Log("Welcome to Developer Console");
        base.Awake();
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string condition, string stacktrace, LogType type)
    {
        _consoleLog += $"{DateTime.Now:T}: {condition}\n";
        _logs++;
        if (_logs > MAX_LOGS)
        {
            _consoleLog = _consoleLog.Substring(_consoleLog.IndexOf('\n') + 1);
        }
        
        if (IsOpen)
        {
            _consoleTextField.SetText(_consoleLog);
        }
    }
    
    private void Update()
    {
        // toggle ui
        if (Keyboard.current.backquoteKey.wasPressedThisFrame)
        {
            Toggle();
        }
    }

    protected override void OnOpen()
    {
        // Debug.Log("OnOpen");
        _consoleTextField.SetText(_consoleLog);
    }
}
}