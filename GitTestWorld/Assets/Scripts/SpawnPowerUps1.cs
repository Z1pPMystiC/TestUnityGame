using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUps1 : MonoBehaviour
{
    public GameObject powerUp1;
    public GameObject powerUp2;
    public GameObject powerUp3;
    public GameObject powerUp4;
    public GameObject powerUp5;
    public Transform powerSpawn2;
    public Transform powerSpawn3;
    public Transform powerSpawn4;
    public Transform powerSpawn5;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(powerUp1, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        Instantiate(powerUp2, powerSpawn2.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        Instantiate(powerUp3, powerSpawn3.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        Instantiate(powerUp4, powerSpawn4.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        Instantiate(powerUp5, powerSpawn5.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
