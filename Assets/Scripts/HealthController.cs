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

    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (gameObject == WeakController.Instance.gameObject || gameObject == HeavyController.Instance.gameObject)
        {
            GUIController.Instance.UpdateHealth(gameObject, currentHealth);
        }
    }

    public void TakeDamage(float damage)
    {
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
                // TODO handle Respawn/GameOver
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
