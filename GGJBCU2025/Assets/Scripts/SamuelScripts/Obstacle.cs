using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Meshes;

    [SerializeField]
    private int nochangeprobability = 3;


    public void ChangeMesh()
    {
        if (Random.Range(0, nochangeprobability) > 1)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            int index = Random.Range(0, Meshes.Length);

            Quaternion rot = Quaternion.identity;
            Instantiate(Meshes[index], transform.position, rot, transform);
        }
    }
}