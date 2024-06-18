using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{
    [SerializeField] public Transform[] diceFaces;
    public Rigidbody rb;

    //tracking which dice we're tracking, may remove since there's only 1 dice
    private int diceIndex = -1;
    private int diceValue;
    public int DiceValue{get{return diceValue;}}

    private bool hasStoppedRolling;
    private bool delayFinished;
    
    public static UnityAction<int, int> OnDiceResult;
    public static event EventHandler OnDiceStopRolling;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }
    private void Update(){
        if(!delayFinished){return;}
        if(!hasStoppedRolling && rb.velocity.sqrMagnitude == 0f){
            hasStoppedRolling = true;
            diceValue = GetDiceValue();
            OnDiceStopRolling?.Invoke(this,EventArgs.Empty);
        }
    }
    private int GetDiceValue(){
        if(diceFaces == null){return -1;}
        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for(int i = 0; i<diceFaces.Length;i++){
            if(diceFaces[i].position.y > lastYPosition){
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }
        // Debug.Log("Dice Result: " + (topFace + 1));

        OnDiceResult?.Invoke(diceIndex, topFace +1);
        return topFace + 1;
    }

    public void RollDice(float throwForce, float rollForce, int i){
        var randomResults = UnityEngine.Random.Range(-1f,1f);
        var force = new Vector3(transform.forward.x, 1f, transform.forward.z).normalized * throwForce;
        rb.AddForce(force + new Vector3(0f,randomResults, 0f), ForceMode.Impulse);

        var randX = UnityEngine.Random.Range(0f,1f);
        var randY = UnityEngine.Random.Range(0f,1f);
        var randZ = UnityEngine.Random.Range(0f,1f);

        rb.AddTorque(new Vector3(randX,randY,randZ)*(rollForce + randomResults), ForceMode.Impulse);
        DelayResult();
    }
    private async void DelayResult(){
        await Task.Delay(1000);
        delayFinished = true;
    }
}


