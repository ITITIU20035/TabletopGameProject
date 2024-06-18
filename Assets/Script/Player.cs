using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Path currentPath;
    private int pathIndex;
    private int steps;
    public int Steps{get{return steps;}}
    private Rigidbody rb;

    private bool isMoving;
    private float raycastMaxDistance = 10f;
    private Tile currentTile;
    private bool moveBackward = false;
    public event EventHandler OnCollideWithBonusTile;
    public event EventHandler OnCollideWithFailTile;
    public event EventHandler OnReachedLastTile;
    public event EventHandler OnPlayerStoppedMoving;
    [SerializeField] public DiceRoller diceRoller;
    Vector3[] directions = {
        Vector3.down,
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    private void Start(){
        Dice.OnDiceStopRolling += Dice_OnDiceStopRolling;
        OnCollideWithFailTile += Player_OnCollideWithFailTile;
        rb = this.GetComponent<Rigidbody>();
    }
    private void Player_OnCollideWithFailTile(object sender, EventArgs e){
        moveBackward = true;
        steps = 3;
        StartCoroutine(Move());
    }
    IEnumerator Move(){
        if(isMoving){
            yield break;
        }else{
            isMoving = true;
            rb.isKinematic = false;
            while(steps>0){
                if(!moveBackward){
                    Vector3 nextPos = currentPath.tileList[pathIndex + 1].position;
                    while(MoveToNextTile(nextPos)){
                        yield return null;
                    }
                yield return new WaitForSeconds(0.1f);
                steps--;
                pathIndex++;
                }else{
                    Vector3 nextPos = currentPath.tileList[pathIndex-1].position;
                    while(MoveToNextTile(nextPos)){
                        yield return null;
                    }
                    yield return new WaitForSeconds(0.1f);
                    steps--;
                    pathIndex--;
                }
            }
        }
        isMoving = false;
        OnPlayerStoppedMoving?.Invoke(this,EventArgs.Empty);
        moveBackward = false;
        CheckTiles();
        rb.isKinematic = true;// The player model falls down the rock tile so raycast can't detect which tile the player's on so this is a workaround
    }
    bool MoveToNextTile(Vector3 destination){
        return destination != (transform.position = Vector3.MoveTowards(transform.position,destination,2f*Time.deltaTime));
    }
    private void Dice_OnDiceStopRolling(object sender, EventArgs e){
        if(!isMoving){
            Dice dice = sender as Dice;
            steps = dice.DiceValue;//change to 4 to test the first fail tile.
            Debug.Log("Dice Rolled: " + steps);
            if(pathIndex + steps < currentPath.tileList.Count){
                StartCoroutine(Move());
            }else{
                Debug.Log("Rolled Number is too high");
            }
        }
    }
    private void CheckTiles(){
        foreach (Vector3 direction in directions){
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, raycastMaxDistance))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile != null && tile != currentTile)
                {
                    currentTile = tile;
                    if (currentTile.CompareTag("Bonus"))
                    {
                        Debug.Log("Player is on Bonus Tile");
                        OnCollideWithBonusTile?.Invoke(this, EventArgs.Empty);
                    }
                    else if (currentTile.CompareTag("Fail"))
                    {
                        Debug.Log("Player is on Fail Tile");
                        OnCollideWithFailTile?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
        if (pathIndex >= currentPath.tileList.Count - 1)
        {
            OnReachedLastTile?.Invoke(this, EventArgs.Empty);
            Debug.Log("Player has reached the last tile.");
            steps = 0;
        }
    }
}
