﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeButton : MonoBehaviour {

    [SerializeField]
    private Button button;
    [SerializeField]
    private Text lineNumber;
    [SerializeField]
    private Text commandName;

    public Text CommandName
    {
        get
        {
            return commandName;
        }

        set
        {
            commandName = value;
        }
    }

    public Text LineNumber
    {
        get
        {
            return lineNumber;
        }

        set
        {
            lineNumber = value;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	


}