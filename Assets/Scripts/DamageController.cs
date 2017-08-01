using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Damager/DamageZone:
// - Holds & Handles (onetime/continues) dmg, (fixed/relative) position, lifetime, speed, knockback, etc. of certain attack
// - Handles collision & keeps track of them
// - Disables itself after lifetime
public class DamageController : MonoBehaviour
{

    // TODO get values from future weapon / item class
    public Transform knockBackPoint;
    // TODO use units instead of force
    public float knockBackForce = 400.0f;
    public float knockBackAngle = 80.0f;
    public float damage = 20.0f;
    public int maxHits = 1;
    public LayerMask isAttackable;

    Dictionary<GameObject, int> timesHit;

    void Awake()
    {
        timesHit = new Dictionary<GameObject, int>();
    }

    void OnEnable()
    {
        timesHit.Clear();
    }

    void Start()
    {
        if (knockBackPoint == null)
            knockBackPoint = transform;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject colGObj = col.gameObject;

        if (Utilities.CheckLayerMask(isAttackable, colGObj))
        {
            if (!timesHit.ContainsKey(colGObj))
                timesHit.Add(colGObj, 0);

            if (timesHit[colGObj] < maxHits)
            {
                // TODO play animations and/or emit particles
                HealthController healthController = colGObj.GetComponent<HealthController>();
                if (healthController != null)
                {
                    healthController.TakeDamage(damage);

                    // TODO Knockback call?
                    float dirX = (colGObj.transform.position - knockBackPoint.position).normalized.x;
                    Vector2 force = Utilities.DegreeToVector2(knockBackAngle);
                    force.x *= dirX * knockBackForce;
                    force.y *= knockBackForce;
                    // FIXME Move to FixedUpdate() or is OnTriggerStay2D called on the physics update cycle (? -> it is! should it move anyway?)
                    colGObj.GetComponent<Rigidbody2D>().velocity = new Vector2();
                    colGObj.GetComponent<Rigidbody2D>().AddForce(force);
                }
                timesHit[colGObj]++;
            }
        }
    }
}
