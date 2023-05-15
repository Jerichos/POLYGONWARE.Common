using System;
using System.Collections.Generic;
using System.Linq;
using POLYGONWARE.Common.UI;
using TMPro;
using UnityEngine;

namespace POLYGONWARE.Common
{

[DefaultExecutionOrder(-999)]
public class ConsoleUI : BaseUI
{
    [SerializeField] private TMP_Text _consoleText;
    [SerializeField] private TMP_InputField _commandInputField;

    private int _logCount;
    private readonly int _maxLogCount = 500;
    private string _logText;

    private static readonly List<CommandProviderBehaviour> _commandProviders = new();

    protected override void Awake()
    {
        base.Awake();
        _consoleText.SetText("");
        _commandInputField.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Debug.Log("toggle console");
            Toggle();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AutoComplete();
        }
        

        if (Input.GetKeyDown(KeyCode.Return) && IsOpen)
        {
            if (!_commandInputField.gameObject.activeSelf)
            {
                _commandInputField.gameObject.SetActive(true);
                _commandInputField.ActivateInputField();
            }
            else
            {
                if(string.IsNullOrEmpty(_commandInputField.text))
                    _commandInputField.gameObject.SetActive(false);
            }
        }
    }

    private void AutoComplete()
    {
        string curText = _commandInputField.text;
        
        if(!_commandInputField.gameObject.activeSelf)
            return;
        
        string[] split = curText.Split(" ");
        string command = split[0];
        
        if(split.Length <= 1)
            ListAllCommandsOf(command);
        
        List<string> availableCommands = new();

        CommandProviderBehaviour currentCommandProvider = null;
        
        foreach (CommandProviderBehaviour commandProvider in _commandProviders)
        {
            foreach (var commands in commandProvider.Commands)
            {
                if (commands.Key.Contains(command))
                {
                    currentCommandProvider = commandProvider;
                    availableCommands.Add(commands.Key);
                }
            }
        }

        if (currentCommandProvider && availableCommands.Count == 1)
        {
            if (command != availableCommands[0])
            {
                command = availableCommands[0];
                _commandInputField.SetTextWithoutNotify(availableCommands[0]);
                _commandInputField.caretPosition = _commandInputField.text.Length;
            }
        }

        if (currentCommandProvider && currentCommandProvider.Commands.ContainsKey(command) && currentCommandProvider.Commands[command].Length > 0)
        {
            //Debug.Log($"{command} args:");
            _commandInputField.SetTextWithoutNotify(_commandInputField.text + " ");
            _commandInputField.caretPosition = _commandInputField.text.Length;
                
            _logCount++;
            _logText += currentCommandProvider.Commands[command].Aggregate("", (current, s) => current + s + "\t") + "\n";
            _consoleText.SetText(_logText);
        }
        else
        {
            _logCount++;
            _logText += availableCommands.Aggregate("", (current, s) => current + s + "\t") + "\n";
            _consoleText.SetText(_logText);
        }
    }

    private void ListAllCommandsOf(string command)
    {
        
    }

    private void OnCommandTextChanged(string text)
    {
        // TODO: auto-complete
    }

    private void OnCommandTextEnd(string text)
    {
        Debug.Log($"Command {text}");
        _commandInputField.SetTextWithoutNotify("");
        string[] command = text.Split(" ");
        
        foreach (CommandProviderBehaviour commandProvider in _commandProviders)
        {
            foreach (var commands in commandProvider.Commands)
            {
                // upper level commands
                if (command[0] == commands.Key)
                {
                    for (int i = 0; i < commands.Value.Length; i++)
                    {
                        if (command[1] == commands.Value[i].ToString())
                        {
                            commandProvider.ExecuteCommand(commands.Key, commands.Value[i]);
                        }
                    }
                }
            }
        }
    }

    public static void RegisterCommandProvider(CommandProviderBehaviour commandProviderBehaviour)
    {
        _commandProviders.Add(commandProviderBehaviour);
    }

    private void OnLogMessage(string condition, string stacktrace, LogType type)
    {
        _logCount++;
        _logText += $"[{DateTime.Now:hh:mm:ss}] {type}: {condition}\n";
        _consoleText.SetText(_logText);
    }
    
    private void OnEnable()
    {
        Application.logMessageReceived += OnLogMessage;
        _commandInputField.onValueChanged.AddListener(OnCommandTextChanged);
        _commandInputField.onEndEdit.AddListener(OnCommandTextEnd);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= OnLogMessage;
        _commandInputField.onValueChanged.RemoveAllListeners();
        _commandInputField.onEndEdit.RemoveAllListeners();
    }
}
}