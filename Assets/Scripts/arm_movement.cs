using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
 
public class arm_movement : NetworkBehaviour
{
 
    public GameObject myPlayer;
 
 
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
 
        difference.Normalize();
 
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
 
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
 
        if (rotationZ < -90 || rotationZ > 90)
        {
 
 
 
            if(myPlayer.transform.eulerAngles.y == 0)
            {
 
 
                transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
 
 
            } else if (myPlayer.transform.eulerAngles.y == 180) {
 
 
                transform.localRotation = Quaternion.Euler(180, 180, rotationZ);
 
 
            }
 
        }
 
    }
 
}