﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject[] maps;
    [SerializeField] private AlertDialog alertDialog;
    [SerializeField] private List<Variable> variables;
    [SerializeField] private int commandsAvailable = 0;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private Player player;
    private GameObject[] coins;
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private CodeOutputPanel codeOutputPanel;
    [SerializeField] private List<CEnemy> enemies;
    public static GameManager instance;


    private bool tutoredGameplayMode;
   
    private bool loopMode;
    private bool functionMode;
    private bool varMode;
    // 0 - False, 1 - If , 2 - Else
    private int conditionalMode;

    
    
    
    public void SetupCodeMode()
    {
        this.loopMode = false;
        this.varMode = false;
        this.functionMode = false;
    }

    public int HasEnemy(Vector3 position)
    {
        // 0 Diferente  / -1 Esquerda / 1 Direita /  2 Mesma Posição
        foreach (CEnemy enemy in enemies)
        {
            if (enemy.IsDead) continue;
            var enemyPosition = enemy.transform.position;
            var enemyIntPosition = new Vector2((float) Math.Truncate(enemyPosition.x),(float) Math.Truncate(enemyPosition.y));
       
            var positionInt = new Vector2((float) Math.Truncate(position.x), (float) Math.Truncate(position
                .y));


            int xDifference = (int) (enemyIntPosition.x -  positionInt.x);
//            Debug.Log("Diferença");
//            Debug.Log(xDifference);
            if (positionInt.y != enemyIntPosition.y) continue;
            if (xDifference == 0)
            {
                return 2;
            } else
            if (xDifference >= 0 && xDifference <= 3)
            {
                return 1;
            } else if (xDifference <= 0 && xDifference >= -3)
            {
                return -1;
            }
           
        }

        return 0;
    }
    
    public bool IsWalkable(Vector3 position)
    {

        Debug.Log("A");
        if (HasEnemy(position) == 2)  return false;
        Debug.Log("b");
        if (ObjectsManager.instance.HasChest(position)) return false;
        Debug.Log("c");
        foreach (var tilemap in maps)
        {
            Sprite nextPositionSprite =
                tilemap.GetComponent<Tilemap>().GetSprite(Vector3Int.RoundToInt(new Vector3(position.x - 0.10f, position.y)));
            Debug.Log("Next Position Sprite: " + nextPositionSprite);
            if (nextPositionSprite == null)
            {
                continue;
            }
            Debug.Log(nextPositionSprite);
            if (!(nextPositionSprite.name == "sand_tile" || nextPositionSprite.name.Contains("grass") || nextPositionSprite.name.Contains("concrete")))
            {
                
                return false;
            }
        }
        return true;
    }

    public bool SendCommandToPlayer(Command command)
    {
        if (command == Command.Open_Chest)
        {
            if (Player1.setActiveCommand(command))
            {
                LevelManager.instance.NextLevel();
                return true;
            }
        }

        return Player1.setActiveCommand(command);
    }

    public void NotifyEnemies(Command command)
    {
        Vector2 nextPlayerPosition = Vector2.zero;
        Vector2 playerPosition = Player1.transform.position;
        switch (command)
        {
            case Command.Walk_Top:
                nextPlayerPosition = new Vector2(playerPosition.x, playerPosition.y + 1);
                break;
            case Command.Walk_Bot:
                nextPlayerPosition = new Vector2(playerPosition.x, playerPosition.y -1);
                break;
            case Command.Walk_Right:
                nextPlayerPosition = new Vector2(playerPosition.x + 1, playerPosition.y);
                break;
            case Command.Walk_Left:
                nextPlayerPosition = new Vector2(playerPosition.x -1, playerPosition.y);
                break;
        }


        foreach (var enemy in enemies)
        {
            Debug.Log("Enemy");
            enemy.AttackPlayer(Player1,nextPlayerPosition);
        }
    }

    public bool CheckPlayerDied()
    {
        if (Player1.IsDead)
        {
            foreach (var enemy in enemies)
            {
                enemy.StopAllCoroutines();
            }   
        }
        return Player1.IsDead;
        
    }
    
    public void NextCommandTutoredGameplay()
    {
        if (TutoredGameplayMode)
        {
            TutoredGameplayMode = parentPanel.GetComponent<TutoredGameplay>().NextButton();
        }
    }


    public void HandleExplosion()
    {
        Player1.TakeDamage(1000);
    }
    
    public void ResetGame()
    {
        Debug.Log("Rest Game: " + coins.Length);
        foreach (GameObject coin in coins)
        {
            Debug.Log("COIN");
            coin.SetActive(true);
            coin.GetComponent<Coin>().Show();
        }

        foreach (var enemy in enemies)
        {
            enemy.Respawn();
        }

        Player1.StopAllCoroutines();
        Player1.stopWalking();
        codeOutputPanel.StopAllCoroutines();
        Player1.transform.position = spawnpoint.transform.position;
        StopAllCoroutines();
    }


    public void FinishTutorial()
    {
        parentPanel.GetComponent<TutoredGameplay>().ShowLabelPanel();
    }

    void Awake()
    {
        instance = this;
        spawnpoint.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }

    // Use this for initialization
    void Start()
    {
        maps = GameObject.FindGameObjectsWithTag("Map");
  
        coins = GameObject.FindGameObjectsWithTag("Coin");


        if (parentPanel.GetComponent<TutoredGameplay>().Buttons.Count > 0)
        {
            TutoredGameplayMode = true;
            //Image image = parentPanel.GetComponent<Image>();
            //var tempColor = image.color;
            //tempColor.a = 0.6f;
            //image.color = tempColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ExitGame()
    {
        LevelManager.instance.BackToMenu();
    }


    public bool LoopMode
    {
        get { return loopMode; }

        set { loopMode = value; }
    }

    public bool FunctionMode
    {
        get { return functionMode; }

        set { functionMode = value; }
    }

    public bool VarMode
    {
        get { return varMode; }

        set { varMode = value; }
    }

    public bool TutoredGameplayMode
    {
        get { return tutoredGameplayMode; }

        set { tutoredGameplayMode = value; }
    }

    public int CommandsAvailable
    {
        get { return commandsAvailable; }
        set { commandsAvailable = value; }
    }

    public AlertDialog AlertDialog
    {
        get { return alertDialog; }
        set { alertDialog = value; }
    }

    public List<Variable> Variables
    {
        get { return variables; }
        set { variables = value; }
    }



    public int ConditionalMode
    {
        get { return conditionalMode; }
        set { conditionalMode = value; }
    }

    public Player Player1
    {
        get { return player; }
        set { player = value; }
    }
}