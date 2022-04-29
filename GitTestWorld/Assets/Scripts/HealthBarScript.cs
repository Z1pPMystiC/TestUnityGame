using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    public bool isDead = false;
    private float respawnCountdown = 1f;

    public playerMotor playerMotor;

    
    [SerializeField] public Transform respawnPoint;
    [SerializeField] public Transform player;

    private void Update()
    {
        respawnCountdown -= Time.deltaTime;
        if (respawnCountdown <= 0)
        {
            respawnCountdown = 1f;
            isDead = false;
        }
    }
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
            RespawnPlayer();
            playerMotor.playerDead = true;
        }
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
