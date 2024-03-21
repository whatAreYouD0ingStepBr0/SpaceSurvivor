using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class collectObjects : NetworkBehaviour
{
    public SpriteRenderer changesSprite;
    public Sprite wingedShip;
    public Sprite fullShip;
    public NetworkVariable<float> gasCollected = new NetworkVariable<float>();
    public NetworkVariable<bool> wingCollected = new NetworkVariable<bool>();
    private bool fullGas;
    //private float gasNeeded = 5;
    public TMP_Text gasText;
    public TMP_Text youNeedWings;

    // Define an enum for ship sprite state
    enum ShipSpriteState
    {
        Default,
        Winged,
        Full
    }

    void Start()
    {
        gasText.text = gasCollected.Value.ToString() + "/5";
    }

    void OnTriggerEnter2D(Collider2D collectable)
    {
        if (!IsOwner) return;

        if (collectable.gameObject.CompareTag("Gas") && !fullGas)
        {
            // Destroy locally and notify server
            //Destroy(collectable.gameObject);
            //CollectGasServerRpc();
        CollectGasServerRpc(collectable.gameObject.GetComponent<NetworkObject>().NetworkObjectId);

        }
        else if (collectable.gameObject.CompareTag("Wing"))
        {
            // Same for wings
            //Destroy(collectable.gameObject);
            //CollectWingServerRpc();
        CollectWingServerRpc(collectable.gameObject.GetComponent<NetworkObject>().NetworkObjectId);

        }
        else if (collectable.gameObject.CompareTag("Ship"))
        {
            EnterShipServerRpc();
        }
    }

    [ServerRpc]
void CollectGasServerRpc(ulong networkObjectId, ServerRpcParams rpcParams = default)
{
    var networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
    if (networkObject != null)
    {
        gasCollected.Value += 1;
        // Additional logic for updating gas collected...
        networkObject.Despawn(); // This will remove the object across the network
        UpdateGasTextClientRpc(gasCollected.Value);
    }
}


    [ClientRpc]
    void UpdateGasTextClientRpc(float gas, ClientRpcParams rpcParams = default)
    {
        gasText.text = gas.ToString() + "/5";
    }

    [ServerRpc]
void CollectWingServerRpc(ulong networkObjectId, ServerRpcParams rpcParams = default)
{
    NetworkObject wingNetworkObject;
    if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out wingNetworkObject))
    {
        // Successfully found the wing object; now despawn it
        wingNetworkObject.Despawn();
        
        // Update any relevant state, such as marking the wings as collected
        wingCollected.Value = true;
    }
    else
    {
        Debug.LogError("Wing object not found on the server.");
    }
}


    [ServerRpc]
void EnterShipServerRpc(ServerRpcParams rpcParams = default)
{
    Debug.Log("EnterShipServerRpc called");
    ShipSpriteState newState = ShipSpriteState.Default;
    if (wingCollected.Value)
    {
        newState = ShipSpriteState.Winged;
    }
    if (fullGas && wingCollected.Value)
    {
        newState = ShipSpriteState.Full;
    }

    UpdateShipSpriteClientRpc(newState);
}


    [ClientRpc]
void UpdateShipSpriteClientRpc(ShipSpriteState state, ClientRpcParams rpcParams = default)
{
    Debug.Log($"Updating ship sprite to state: {state}");
    switch (state)
    {
        case ShipSpriteState.Winged:
            changesSprite.sprite = wingedShip;
            break;
        case ShipSpriteState.Full:
            changesSprite.sprite = fullShip;
            break;
        default:
            Debug.LogError("Unexpected ship sprite state.");
            break;
    }
}


    IEnumerator PopupText()
    {
        youNeedWings.text = "You need WINGS to fly.";
        yield return new WaitForSeconds(4f);
        youNeedWings.text = "";
    }
}