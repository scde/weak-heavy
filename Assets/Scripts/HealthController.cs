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
    public float RespawnTime = 1f;
    public float invincibilityTime = 0.5f;
    float currentHealth;
    float currentInvincibilityTime;

    void Start()
    {
        currentHealth = maxHealth;
        if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
        {
            GUIController.Instance.UpdateHealth(gameObject, currentHealth);
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
        if (!(currentInvincibilityTime > 0))
        {
            currentInvincibilityTime = invincibilityTime;
            currentHealth -= damage;
            if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
            {
                GUIController.Instance.UpdateHealth(gameObject, currentHealth);
            }
            // TODO Display Stuff (on HUD, Avatar[red flash], etc.)
            if (currentHealth <= 0.0f)
            {
                if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
                {
                    Respawn(RespawnPoint);
                    // TODO handle Respawn/GameOver
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void Update()
    {
        if (currentInvincibilityTime > 0)
        {
            currentInvincibilityTime -= Time.deltaTime;
        }
    }

    private void Respawn(GameObject CurrentRespawnPoint)
    {
        StartCoroutine(Utilities.waitForRespawn(RespawnTime, gameObject));
        transform.position = CurrentRespawnPoint.transform.position;
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        GUIController.Instance.UpdateHealth(gameObject, currentHealth);
    }

}
