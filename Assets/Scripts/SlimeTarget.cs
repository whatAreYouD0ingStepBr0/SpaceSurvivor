using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Pathfinding;

public class SlimeTarget : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int target = Random.Range(0, players.Length);

        var targetTransform = players[target].transform;

        AIDestinationSetter setter = GetComponent<AIDestinationSetter>();
        setter.target = targetTransform;


    }
}
