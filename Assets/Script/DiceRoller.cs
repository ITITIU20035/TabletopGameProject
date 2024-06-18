using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public Dice diceToThrow;
    public int amountOfDice = 2;
    public float throwForce = 5f;
    public float rollForce = 10f;
    private List<GameObject> spawnedDice = new List<GameObject>();

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            RollDice();
        }
    }
    private async void RollDice(){
        if(diceToThrow == null) return;
        if(spawnedDice!= null){
            foreach(GameObject die in spawnedDice){
                Destroy(die);
            }
        }
        for (int i = 0; i< amountOfDice; i++){
            Dice dice = Instantiate(diceToThrow, transform.position, transform.rotation);
            spawnedDice.Add(dice.gameObject);
            dice.RollDice(throwForce, rollForce,i);
            await Task.Yield();
        }
    }
}
