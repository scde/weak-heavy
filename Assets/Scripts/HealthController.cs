using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject attackable/mortal:
// - Keeps track of health
// - Keeps track of invincibility
// - Handles death (fires death event?)
public class HealthController : MonoBehaviour
{

    public float maxHealth = 100.0f;
    public GameObject RespawnPoint;
    public float invincibilityTime = 0.5f;

    private float currentHealth;
    private float currentInvincibilityTime;

    private void Start()
    {
        currentHealth = maxHealth;
        if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
        {
            GUIController.Instance.UpdateHealth(gameObject, currentHealth);
        }
    }

    private void Update()
    {
        if (currentInvincibilityTime > 0)
        {
            currentInvincibilityTime -= Time.deltaTime;
        }
    }

    public bool TakeWaterDamage(float damage)
    {
        if (gameObject == WeakController.Instance.gameObject)
        {
            if (!WeakController.Instance.IsWaterImmune)
            {
                TakeDamage(damage);
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(float damage)
    {
        if (currentInvincibilityTime <= 0.0f)
        {
            currentInvincibilityTime = invincibilityTime;
            currentHealth -= damage;
            // TODO Display Stuff (on HUD, Avatar[red flash], etc.)
            if (gameObject == WeakController.Instance.gameObject)
            {
                EventManager.Instance.TriggerEvent("Hit_" + WeakController.Instance.PlayerId);
                GUIController.Instance.UpdateHealth(gameObject, currentHealth);
            }
            else if (gameObject == HeavyController.Instance.gameObject)
            {
                EventManager.Instance.TriggerEvent("Hit_" + HeavyController.Instance.PlayerId);
                GUIController.Instance.UpdateHealth(gameObject, currentHealth);
            }
            if (currentHealth <= 0.0f)
            {
                if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
                {
                    EventManager.Instance.TriggerEvent("Respawn");
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        GUIController.Instance.UpdateHealth(gameObject, currentHealth);
    }
}
