using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class slimeDie : MonoBehaviour

{
    public AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            SlimeDieServerRpc();
            audioData.Play();
            Destroy(gameObject);
        }

        if (col.gameObject.tag.Equals("Player"))
        {
            Debug.LogError(" Player HIT! ");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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