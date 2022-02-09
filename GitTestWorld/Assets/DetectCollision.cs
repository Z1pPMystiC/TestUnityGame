using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public RaycastGun teslaCoil;
    public GameObject player;
    private void Start()
    {
        Physics.IgnoreCollision(player.GetComponent<CapsuleCollider>(), GetComponent<SphereCollider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        teslaCoil.myList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) 
    { 
        teslaCoil.myList.Remove(other.gameObject); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
