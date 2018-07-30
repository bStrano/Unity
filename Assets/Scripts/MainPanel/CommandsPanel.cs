﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public enum Command
{
    None,
    Walk_Top,
    Walk_Right,
    Walk_Bot,
    Walk_Left,
    Attack,
    Spell1,
    Spell2,
    Spell3
}

public class CommandsPanel : MonoBehaviour
{
    private List<Command> commands;
    [SerializeField]
    private Text entries;
    private int maxComands;

    public List<Command> Comands
    {
        get
        {
            return commands;
        }

        set
        {
            commands = value;
        }
    }

    public void Start()
    {
        Debug.Log(LevelManager.instance);
        maxComands = LevelManager.instance.ActiveLevel.ComandsAvaiable;
        commands = new List<Command>();
        UpdateEntries();
    }


    public bool AddComand(Command command)
    {
        if (commands.Count < maxComands)
        {
            commands.Add(command);
            return true;
        }
        else
        {
            return false;
            // Not implemented yet;
            Debug.Log("Lista Cheia");
        }
    }

    public void UpdateEntries()
    {
        entries.text = commands.Count + " / " + maxComands;
        if(commands.Count == maxComands)
        {
            entries.color = Color.red;
        }
    }

    public Command ReciveComand(string commandString)
    {
        Command command = ((Command)Enum.Parse(typeof(Command), commandString));

        if (AddComand(command))
        {
            UpdateEntries();
            return command;
        } else
        {
           return Command.None;
        }
        
        

        //switch ((Command)Enum.Parse(typeof(Command), commandString))
        //{
        //    case Command.Walk_Top:
        //        Debug.Log("Walk_Top");
        //        AddComand(Command.Walk_Top);
        //        break;
        //    case Command.Walk_Right:
        //        Debug.Log("Walk Right");
        //        AddComand(Command.Walk_Right);
        //        break;
        //    case Command.Walk_Bot:
        //        Debug.Log("Walk Bot");
        //        AddComand(Command.Walk_Bot);
        //        break;
        //    case Command.Walk_Left:
        //        Debug.Log("Walk Left");
        //        AddComand(Command.Walk_Left);
        //        break;
        //    case Command.Spell1:
        //        AddComand(Command.Spell1);
        //        break;
        //    case Command.Spell2:
        //        AddComand(Command.Spell2);
        //        break;
        //    case Command.Spell3:
        //        AddComand(Command.Spell3);
        //        break;
        //}

    }

   

}

