using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class slimeDie : MonoBehaviour

{

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            SlimeDieServerRpc();
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    [ServerRpc]
    void SlimeDieServerRpc()
    {
        Destroy(gameObject);
        var netObj = GetComponent<NetworkObject>();
        netObj.Despawn();

    }
}