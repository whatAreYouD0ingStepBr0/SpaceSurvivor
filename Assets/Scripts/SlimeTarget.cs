using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Pathfinding;

public class SlimeTarget : NetworkBehaviour
{
    private Transform targetTransform;
    public override void OnNetworkSpawn()
    {
        SetTargetServerRpc();
    }

    private void Update()
    {
        if (targetTransform != null) return;
        Debug.Log("Looking for target");
        SetTargetServerRpc();
    }

    [ServerRpc]
    private void SetTargetServerRpc()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int target = Random.Range(0, players.Length);

        targetTransform = players[target].transform;

        AIDestinationSetter setter = GetComponent<AIDestinationSetter>();
        setter.target = targetTransform;
    }


}
