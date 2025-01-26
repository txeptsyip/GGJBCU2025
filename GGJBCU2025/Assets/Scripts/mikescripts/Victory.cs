using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public GameObject player;
    public bool alive = true;
    public int score = 0;
}
public class Victory : MonoBehaviour
{
    List<Player> players = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players.Add(new Player { player = GameObject.FindGameObjectWithTag("Player") });
        players.Add(new Player { player = GameObject.FindGameObjectWithTag("Player2") });
    }
    bool victoryCalled = false;
    bool incrementCalled = false;
    int victoryScore = 2;
    void victory(int player)
    {
        if (players[player].score != victoryScore) return;
        victoryCalled = true;
        Debug.Log(players[player].player.tag + " Wins");
    }

    void incrementScore ()
    {
        if (incrementCalled) return;
        incrementCalled = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].alive) continue;

            //Add score logic here
            players[i].score++;
            victory(i);
            Debug.Log("The player's score is " + players[i].score.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (victoryCalled) return;
        for (int i = 0; i < players.Count; i++)
        {
            if (incrementCalled)
            {
                if (!players[i].player) { return; } else { incrementCalled = false; }
            }
            if (!players[i].player && !incrementCalled) { players[i].alive = false; incrementScore(); }
        }
    }
}
