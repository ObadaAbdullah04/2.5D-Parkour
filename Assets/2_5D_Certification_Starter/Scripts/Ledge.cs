using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField] private Vector3 _handPos, _standPos;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LedgeGrabChecker"))
        {
            Player player = other.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                player.GrabLedge(_handPos, this);
                // Destroy(this.gameObject);
            }
        }
    }
    
    public Vector3 GetStandPos()
    {
        return _standPos;
    }
}
