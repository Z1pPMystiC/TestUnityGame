using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(), GetComponent<BoxCollider>());
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
