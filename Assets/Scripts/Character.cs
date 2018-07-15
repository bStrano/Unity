﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Character : MonoBehaviour {
    [SerializeField]
    protected float speed;
    protected Vector3 direction;

    protected Rigidbody2D rb;
    protected Animator animator;

    protected bool isWalking, isAttackink;


    protected bool isAttacking = false;
  


    // Use this for initialization
    protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        HandleLayers();
        

    }

    protected virtual void FixedUpdate()
    {
    
    }



    public void HandleLayers()
    {
        if (isWalking && !isAttacking)
        {
            ActivateLayer("Walk Layer");
            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);

        } else if (isAttacking)
        {
            ActivateLayer("Attack Layer");
        } else 
        {
            ActivateLayer("Iddle Layer");
        }

        
    }

    public void ActivateLayer(string layerName)
    {
        for(int i = 0; i < animator.layerCount; i++ )
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }
 

    public void AnimateMovement()
    {
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

    public bool IsMoving()
    {
        if( (direction.x == 0) && (direction.y == 0) )
        {
            return false;
        }
        return true;
    }

    
}