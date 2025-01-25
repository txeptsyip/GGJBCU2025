using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public MapGenerator Map;

    [SerializeField]
    private GameObject Piece;

    [SerializeField]
    private float Height = 20;

    [SerializeField]
    private float PowerUpsPerSpawn = 1;


    public void Start()
    {
        StartCoroutine(SpawnPieces());
    }

    public IEnumerator SpawnPieces()
    {
        while (true)
        {
            for (int i = 0; i < PowerUpsPerSpawn; i++)
            {
                Vector2 pos = Map.GetRandomPosition();

                Map.SpawnPU((int)pos.x, (int)pos.y, Height);
            }

            yield return new WaitForSeconds(Random.Range(20,40));
        }
    }
}