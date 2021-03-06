﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;

public class Player : Character
{
    public Tilemap map;
    public Tilemap[] alternativeMaps;


    [SerializeField] private GameObject[] spellPrefab;
    [SerializeField] private Transform[] exitPoints;
    private int exitIndex = 2;
    private Transform target;

    private Sprite groundSprite;
    private Command lastMovementCommand = Command.Walk_Bot;
    
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject victoryEffect;
    [SerializeField] private GameObject shieldEffect;

    public bool ShieldActivated { get; set; } 


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void StopAllCouroutines()
    {
        StopAllCoroutines();
        isWalking = false;
    }



    bool MoveIfPossible(Vector3 nextPosition)
    {
        if (GameManager.instance.IsWalkable(nextPosition))
        {
            StartCoroutine(Move(nextPosition));
            return true;
        }
        else
        {
            direction.x = 0;
            direction.y = 0;
            isWalking = false;
            return false;
        }
    }

    public bool CollectCoin()
    {
        return ObjectsManager.instance.RequestCoinCollect(transform);
    }

    public bool OpenChest()
    {
        if (!ObjectsManager.instance.HasCollectedCoins()) return false;
        Direction direction = ObjectsManager.instance.RequestOpenChest(transform);
        if (direction != Direction.NONE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MoveToDirection(Command direction)
    {
        if (!isWalking)
        {
            Vector3 directionBeforeMovement = new Vector3(this.direction.x, this.direction.y, 0);
            isWalking = true;
            Vector3 position = transform.position;
            switch (direction)
            {
                case (Command.Walk_Top):
                    exitIndex = 0;
                    return TryMove(Command.Walk_Top, position.x, position.y + 1, 0, 0, 1);
                case (Command.Walk_Right):
                    exitIndex = 1;
                    return TryMove(Command.Walk_Right, position.x + 1, position.y, 1, 1, 0);
                case (Command.Walk_Bot):
                    return TryMove(Command.Walk_Bot, position.x, position.y - 1, 2, 0, -1);
                case (Command.Walk_Left):
                    return TryMove(Command.Walk_Left, position.x - 1, position.y, 3, -1, 0);
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool TryMove(Command command, float posX, float posY, int exitIndex, int directionX, int directionY)
    {
        bool hasMoved = MoveIfPossible(new Vector3(posX, posY));
        if (hasMoved)
        {
            this.direction.x = directionX;
            this.direction.y = directionY;
            this.exitIndex = exitIndex;
            this.lastMovementCommand = command;
   
        }

        return hasMoved;
    }



   
    
    public bool setActiveCommand(Command playerCommand)
    {
        switch (playerCommand)
        {
            //PlayerCommand playerCommand = (PlayerCommand)Enum.Parse(typeof(PlayerCommand), command);
            case Command.Walk_Top:
            case Command.Walk_Right:
            case Command.Walk_Bot:
            case Command.Walk_Left:
                //Debug.Log("Iniciando a movimentação do personagem, ---- " + playerCommand.ToString());
                return MoveToDirection(playerCommand);
            case Command.Collect_Coin:
                return CollectCoin();
            case Command.Open_Chest:
                return OpenChest();
            case Command.Fireball:
                return AttackTest("Fireball");
            case Command.Ice:
                return AttackTest("Ice");
            case Command.Lightning:
                return AttackTest("Lightning");
            case Command.Shield:
                StartCoroutine(nameof(ActivateShield));
                return true;
            default:
                return false;
        }
    }


    public IEnumerator ActivateShield()
    {
        ShieldActivated = true;
        shieldEffect.SetActive(true);
        yield return new WaitForSeconds(3);
        shieldEffect.SetActive(false);
        ShieldActivated = false;
    }

    public bool AttackTest(string spellName)
    {
        if (isDead) return false;
        if (!isAttacking && !isWalking)
        {
            StartCoroutine(Attack(spellName));
        }

        return true;
    }

    IEnumerator Attack(string spellName)
    {
        if (!isAttacking && !isWalking)
        {
            isAttacking = true;

            Debug.Log(GameManager.instance.HasEnemy(transform.position));
            if (GameManager.instance.HasEnemy(transform.position) == -1)
            {
                lastMovementCommand = Command.Walk_Left;
                this.ChangeLookingDirection(Direction.LEFT);
           
            } else if (GameManager.instance.HasEnemy(transform.position) == 1)
            {    
                lastMovementCommand = Command.Walk_Right;
                this.ChangeLookingDirection(Direction.RIGHT);
            }

            animator.SetBool("isAttacking", isAttacking);

            yield return new WaitForSeconds(1);

            CastSpell(spellName);

            StopAttack();
        }
    }

    public void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }
    
    public override void TakeDamage(int damage)
    {
        Debug.Log( "Shield: " + ShieldActivated);
        if (!ShieldActivated)
        {
            base.TakeDamage(damage);
        }
        
    }

    public void CastSpell(string spellIndex)
    {
      
        GameObject prefab;
        switch (spellIndex)
        {
            case ("Fireball"):
                prefab = Instantiate(spellPrefab[0], exitPoints[exitIndex].position, Quaternion.identity);
                break;
            case ("Lightning"):
                prefab = Instantiate(spellPrefab[1], exitPoints[exitIndex].position, Quaternion.identity);
                break;
            case ("Ice"):
                prefab = Instantiate(spellPrefab[2], exitPoints[exitIndex].position, Quaternion.identity);
                break;
            default:
                return;
        }

        Spell spell = prefab.GetComponent<Spell>();
        Vector3 position = prefab.transform.position;

        switch (lastMovementCommand)
        {
            case Command.Walk_Top:
                prefab.transform.rotation = Quaternion.Euler(0, 0, 90);
                spell.Cast(new Vector2(position.x, position.y+3) );
                break;
            case Command.Walk_Bot:
                prefab.transform.rotation = Quaternion.Euler(0, 0, 270);
                spell.Cast(new Vector2(position.x, position.y-3) );
                break;
            case Command.Walk_Left:
                prefab.transform.rotation = Quaternion.Euler(0, 180, 0);
                spell.Cast(new Vector2(position.x-3, position.y) );
                break;
            case Command.Walk_Right:
                prefab.transform.rotation = Quaternion.Euler(0, 0, 0);
                spell.Cast(new Vector2(position.x+3, position.y) );
                break;
        }

    }

    public override void Die()
    {
        base.Die();
       
        deathEffect.SetActive(true);
        spriteRenderer.enabled = false;

    }
}
