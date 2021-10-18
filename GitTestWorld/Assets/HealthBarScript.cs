using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;

    
    [SerializeField] public Transform respawnPoint;
    [SerializeField] public Transform player;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

   public void SetHealth(int health)
    {
        slider.value = health;
    }

    public int GetHealth()
    {
        return (int)slider.value;
    }

    public void TakeDamage(int damage)
    {
        slider.value -= damage;

        if (slider.value <= 0)
        {
            SetMaxHealth(100);
            RespawnPlayer(player, respawnPoint);
        }
    }

    public void RespawnPlayer(Transform player, Transform respawnPoint)
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
