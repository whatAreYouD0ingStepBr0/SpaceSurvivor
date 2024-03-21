using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerCamFollow : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        var cam = GameObject.FindGameObjectWithTag("Cam").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = transform;
    }
}
