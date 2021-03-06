﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;


    private string ActiveLevel { get; set; }

    void Awake()
    {
        if(instance == null)
        {
           instance = this;
        
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        
    }

   

    public void NextLevel()
    {
        Debug.Log(ActiveLevel);;
        int nextLevel = int.Parse(ActiveLevel)+1;
        Debug.Log(nextLevel);
        SwitchScene(nextLevel.ToString());
        
    }


    public void SwitchScene(string level)
    {
        this.ActiveLevel = level;
        SceneManager.LoadScene("Level_" + level);
       
    }
    

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


    public string GetActiveLevel()
    {
        return ActiveLevel;
        //int activeIndex = SceneManager.GetActiveScene().buildIndex;
        
       // return levelList[activeIndex];
    }

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
