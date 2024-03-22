using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class staticSpawn : NetworkBehaviour
{
   [SerializeField]
   private GameObject slimePrefab;

    [SerializeField]
  //max enemy interval when spawning
   public float slimeInterval;

// Start is called before the first frame update
    void Start()
    {
        slimeInterval = 6f;
        //calls to start coroutine which spawns enemy
        StartCoroutine(spawnEnemy(slimeInterval, slimePrefab));
   
    }

private IEnumerator spawnEnemy(float interval, GameObject enemy)
{
    yield return new WaitForSeconds(interval);
    // Reset back to the max interval to repeat the process
    interval = 6;

    // Instantiate enemy at spawner position
    GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

    // Ensure there's a player script component before trying to access it
    var playerScript = GameObject.Find("player1")?.GetComponent<collectObjects>();
    if (playerScript != null)
    {
        // Access the Value property of the NetworkVariable when comparing
        float gasCollected = playerScript.gasCollected.Value;

        if (gasCollected == 1)
        {
            interval -= 1;
        }
        else if (gasCollected == 2)
        {
            interval -= 2;
        }
        else if (gasCollected == 3)
        {
            interval -= 3;
        }
        else if (gasCollected == 4)
        {
            interval -= 4;
        }
        else if (gasCollected == 5)
        {
            interval -= 5;
        }
        // If no gas is collected, the interval remains at the max rate
    }
    else
    {
        Debug.LogError("Player script not found.");
    }

    // Start the coroutine again with the adjusted interval
    StartCoroutine(spawnEnemy(interval, enemy));
}


}
 

