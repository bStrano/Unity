﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest : MonoBehaviour {
    private Animator animator;
    [SerializeField] private bool isOpen;

    public Direction IsNear(Transform transform)
    {
        double thisX = Math.Truncate(this.transform.position.x);
        double x = Math.Truncate(transform.position.x);
        double thisY = Math.Truncate(this.transform.position.y);
        double y = Math.Truncate(transform.position.y);

        Debug.Log(this.transform.position.x + " + " + Math.Truncate(this.transform.position.y) + " + " + transform.position.x + " + " + Math.Truncate(transform.position.y));

        if (x == thisX)
        {
            if ((y + 1) == thisY)
            {
                return Direction.TOP;
            } else if ((y - 1) == thisY)
            {
                return Direction.BOT;
            } else
            {
                return Direction.NONE;
            }
        } else if (y == thisY)
        {
            if((x+1) == thisX)
            {
                return Direction.RIGHT;
            } else if ((x-1) == thisX)
            {
                return Direction.LEFT;
            } else
            {
                return Direction.NONE;
            }
        } else
        {
            return Direction.NONE;
        }

    }

    public Direction OpenChest(Transform transform)
    {
        Direction direction = IsNear(transform);
         if(direction != Direction.NONE)
        {
            Debug.Log("Opening Chest");
            animator.SetTrigger("Open");
        }
        return direction;
    }

	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();
        if (isOpen)
        {
            animator.SetTrigger("Open");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
