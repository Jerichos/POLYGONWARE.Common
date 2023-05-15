using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace POLYGONWARE.Common
{
public abstract class CommandProviderBehaviour : MonoBehaviour
{
    public readonly Dictionary<string, object[]> Commands = new();

    protected virtual void Awake()
    {
        RegisterCommands();
        ConsoleUI.RegisterCommandProvider(this);
    }

    protected void AddCommand(string command, params object[] arguments)
    {
        Commands.Add(command, arguments);
    }

    protected abstract void RegisterCommands();


    public void ExecuteCommand(string command, params object[] arguments)
    {
        if (!Commands.ContainsKey(command))
        {
            Debug.LogError($"No such a command: {command}");
            return;
        }
        
        OnCommandExecute(command, arguments);
    }

    protected abstract void OnCommandExecute(string command, params object[] arguments);
}
}