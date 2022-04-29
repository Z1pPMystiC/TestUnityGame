using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupSpeedBoost : MonoBehaviour
{
    public TextMeshProUGUI centerText;
    public playerMotor player;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 100f * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.walkSpeed *= 2;
            player.sprintSpeed *= 2;
            centerText.SetText("Speed Boost Activated for 10 Seconds!");
            Invoke("ClearText", 2f);
            Invoke("RevertSpeed", 10f);
            gameObject.SetActive(false);
        }
    }

    public void ClearText()
    {
        if (centerText != null)
        {
            centerText.SetText("");
        }
    }

    public void RevertSpeed()
    {
        player.walkSpeed /= 2;
        player.sprintSpeed /= 2;
        Destroy(gameObject);
    }
}

