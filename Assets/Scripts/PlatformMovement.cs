using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    void OnTriggerEnter(Collider other) // Attach player to platform
    {
        if (other.gameObject.name == "Player")
            other.transform.parent = transform;
    }

    void OnTriggerExit(Collider other) // Detach player from platform
    {
        if (other.gameObject.name == "Player")
            other.transform.parent = null;
    }
}
