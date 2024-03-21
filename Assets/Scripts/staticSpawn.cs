using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class staticSpawn : NetworkBehaviour
{
    [SerializeField]
    private GameObject slimePrefab;
    public float slimeInterval = 6f;

    private collectObjects playerScript; // Reference to the player's script

    void Start()
    {
        if (IsServer) // Ensure that only the server controls the spawning logic
        {
            playerScript = FindObjectOfType<collectObjects>(); // Ideally, replace this with a more reliable way to get the player script
            if (playerScript != null)
            {
                StartCoroutine(SpawnEnemy(slimeInterval));
            }
            else
            {
                Debug.LogError("Player script not found. Ensure the player is spawned before enemies.");
            }
        }
    }

    private IEnumerator SpawnEnemy(float interval)
    {
        while (true) // Changed to a loop for continuous spawning
        {
            yield return new WaitForSeconds(interval);
            
            // Adjust spawn interval based on gas collected
            float gasCollected = playerScript.gasCollected.Value; // Safe access to NetworkVariable
            interval = 6 - Mathf.Clamp(gasCollected, 0, 5); // Ensure interval is adjusted correctly
            
            // Spawn the enemy
            NetworkObject.Instantiate(slimePrefab, transform.position, Quaternion.identity);

            // No need for repeated StartCoroutine calls; using a while loop instead
        }
    }
}
