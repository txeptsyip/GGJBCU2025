using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public GameObject player;
    public bool alive = true;
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
    bool called = false;
    void victory()
    {
        if (called) return;
        called = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].alive) continue;

            //Add victory logic here
            Debug.Log(players[i].player.tag + " Wins");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].player) { players[i].alive = false; }
            else { continue; }
            victory();
        }
    }
}
