using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugCommandBase
{
    private string _commandName;
    private string _commandDescription;
    private string _commandFormat;

    public string commandName { get { return _commandName; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string name, string description, string format)
    {
        _commandName = name;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string name, string description, string format, Action command) : base (name, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}