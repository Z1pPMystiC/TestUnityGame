using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupHealthBoost : MonoBehaviour
{
    public TextMeshProUGUI centerText;
    public HealthBarScript healthBar;
    public Transform floppyDisk;


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
            if(healthBar.slider.value + 25 > 100)
            {
                healthBar.slider.value = healthBar.slider.maxValue;
            }
            else
            {
                healthBar.slider.value += 25;
            }
            centerText.SetText("Health Boost!\n+25 Health");
            Invoke("ClearText", 2f);
            gameObject.SetActive(false);
        }
    }

    public void ClearText()
    {
        if (centerText != null)
        {
            centerText.SetText("");
        }
        Destroy(gameObject);
    }
}
