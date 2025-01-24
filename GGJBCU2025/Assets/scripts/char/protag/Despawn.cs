using UnityEngine;

public class Despawn : MonoBehaviour
{
    float DespawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DespawnTimer = Time.time + 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > DespawnTimer) { Destroy(gameObject); }
    }
}
