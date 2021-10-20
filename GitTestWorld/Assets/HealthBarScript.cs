using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : RobotMotion
{

    public Slider slider;
    public bool isDead = false;

    
    [SerializeField] public Transform respawnPoint;

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

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        slider.value -= damage;

        if (slider.value <= 0)
        {
            isDead = true;
            SetMaxHealth(100);
            RespawnPlayer(player, respawnPoint);
        }
    }

    public void RespawnPlayer(Transform player, Transform respawnPoint)
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
