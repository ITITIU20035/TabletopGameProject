using System;
using System.Collections;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public Player player;
    private bool reachLastTile = false;
    public GameObject spacePressUI;
    private Coroutine waitForSpaceCoroutine;

    void Start()
    {
        player.OnPlayerStoppedMoving += UI_OnPlayerStopMoving;
        player.OnReachedLastTile += UI_OnReachedLastTile;
    }

    private void UI_OnReachedLastTile(object sender, EventArgs e)
    {
        reachLastTile = true;
    }

    private void UI_OnPlayerStopMoving(object sender, EventArgs e)
    {
        if (waitForSpaceCoroutine != null)
        {
            StopCoroutine(waitForSpaceCoroutine);
        }
        waitForSpaceCoroutine = StartCoroutine(WaitForTenSeconds());
    }

    private IEnumerator WaitForTenSeconds()
    {
        float timer = 0f;

        while (timer < 10f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        if(!reachLastTile){
            spacePressUI.SetActive(true);
        }
        
    }

    void Update()
    {
        if (spacePressUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            spacePressUI.SetActive(false);
        }
    }
}
