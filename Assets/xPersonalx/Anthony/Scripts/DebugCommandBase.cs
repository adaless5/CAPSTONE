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

//public class DebugUnlockWeaponCommand : DebugCommandBase
//{
//    Action

//    private int _weaponIndex;
//    private ALTPlayerController _playerController;

//    public DebugUnlockWeaponCommand(string name, string description, string format, ALTPlayerController player) : base(name, description, format)
//    {
//        _playerController = player;
//    }

//    public override void Activate()
//    {
//        if (_playerController != null)
//        {
//            foreach( Tool equipment in _playerController._weaponBelt._items)
//            {
//                equipment.ObtainEquipment();
//            }
//        }
//    }
//}

//public class DebugUnlockToolCommand : DebugCommandBase
//{
//    private int _toolIndex;
//    private ALTPlayerController _playerController;

//    public DebugUnlockToolCommand(string name, string description, string format, ALTPlayerController player) : base(name, description, format)
//    {
//        _playerController = player;
//    }

//    public override void Activate()
//    {
//        if (_playerController != null)
//        {
//            foreach (Tool equipment in _playerController._equipmentBelt._items)
//            {
//                equipment.ObtainEquipment();
//            }
//        }
//    }
//}
