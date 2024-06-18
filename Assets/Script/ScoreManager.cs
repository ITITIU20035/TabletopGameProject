using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {
    public Player player;
    public GameObject finalScoreboard;
    public GameObject bonusUI;
    public GameObject failUI;
    public GameObject turnsUI;
    private int bonusCount;
    private int turnsCount;
    private int failCount;
    void Initialize(){
        finalScoreboard.SetActive(false);
        bonusCount= 0;
        turnsCount= 0;
        failCount= 0;
        Dice.OnDiceStopRolling += Score_OnDiceStopRolling;
        player.OnCollideWithBonusTile += Score_OnCollideWithBonusTile;
        player.OnCollideWithFailTile +=Score_OnCollideWithFailTile;
        player.OnReachedLastTile +=Score_OnReachedLastTile;
    }
    void Start(){
        Initialize();
    }

    private void Score_OnReachedLastTile(object sender, EventArgs e)
    {
        finalScoreboard.SetActive(true);
        //Add more logic down here for when the game ends...
    }

    void Update(){
        bonusUI.GetComponent<TextMeshProUGUI>().text = bonusCount.ToString();
        turnsUI.GetComponent<TextMeshProUGUI>().text = turnsCount.ToString();
        failUI.GetComponent<TextMeshProUGUI>().text = failCount.ToString();
    }

    private void Score_OnCollideWithFailTile(object sender, EventArgs e)
    {
        failCount += 1;
        Debug.Log("Player fail count is: " + failCount);
    }

    private void Score_OnCollideWithBonusTile(object sender, EventArgs e)
    {
        bonusCount += 1;
        Debug.Log("Player bonus bonus is: " + bonusCount);
    }

    private void Score_OnDiceStopRolling(object sender, EventArgs e)
    {
        turnsCount += 1;
        Debug.Log("Player score is: " + turnsCount);
    }
}