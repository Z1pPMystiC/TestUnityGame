using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public RaycastGun teslaCoil;
    public GameObject player;
    private void Start()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        teslaCoil.myList.Add(other.gameObject);
        Debug.Log("Hit");
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
