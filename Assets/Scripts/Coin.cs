﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private bool canExplode;
    [SerializeField] private ParticleSystem defaultEffect;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private bool explode;
    private Renderer spriteRenderer;
    
    public bool IsTrap { get; set; }


    public bool HasCoin(Transform transform)
    {
        Vector3 transformAbs = new Vector3(transform.position.x, (float) Math.Floor(transform.position.y), 0);
//        Debug.Log("HAS COIN" + this.transform.position.x + " / " +  Math.Floor(this.transform.position.y) + " / " +  transform.position.x + " / " +  Math.Floor(transform.position.y));
        Vector3 thisTransformAbs = new Vector3(this.transform.position.x, (float) Math.Floor(this.transform.position.y), 0);
        if ( transformAbs == thisTransformAbs)
        {
            return true;
        } else
        {
            return false;
        }
    }

    
    
    public void Hide()
    {
        defaultEffect.gameObject.SetActive(false);
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        defaultEffect.gameObject.SetActive(true);
        spriteRenderer.enabled = true;
    }
    
    public bool RemoveCoin(Transform transform)
    {
        if (canExplode && IsTrap)
        {
            defaultEffect.gameObject.SetActive(false);
            explosionEffect.gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            GameManager.instance.HandleExplosion();
            return true;
        }

        if (HasCoin(transform) && spriteRenderer.enabled )
        {    
            defaultEffect.gameObject.SetActive(false);
            collectEffect.gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            //this.gameObject.SetActive(false);
            return true;
        }

        return false;


    }


	// Use this for initialization
	void Start ()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
       
        Random rng = new Random(DateTime.Now.Millisecond);
        IsTrap = rng.Next(0, 2) == 1;
        Debug.Log("Coin is Trap: " + IsTrap);
        if (explode)
        {
            IsTrap = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
