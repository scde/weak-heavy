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
    public bool isWaterDamage;
    public float lifetime;
    public float speed;
    public Transform followPoint;
    public Transform knockBackPoint;
    // TODO use units instead of force
    public float knockBackForce = 400.0f;
    public float knockBackAngle = 80.0f;
    public float damage = 20.0f;
    // if maxHits = 0 unlimited Hits
    public int maxHits = 1;
    public LayerMask isAttackable;

    Dictionary<GameObject, int> timesHit;
    Rigidbody2D rb2d;

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

        rb2d = GetComponent<Rigidbody2D>();

        if (lifetime > 0)
            Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (speed > 0)
        {
            // TODO FIXME magnitude and direction
            rb2d.AddForce(new Vector2(speed, 0.0f));
            speed = 0;
        }
    }

    //void Update()
    //{
    // TODO object pooling (instead of instantiate-destroy-cycle):
    // https://unity3d.com/learn/tutorials/topics/scripting/object-pooling?playlist=17117
    //if (lifetime <= 0)
    //{
    //gameObject.SetActive(false);
    //}
    //lifetime -= Time.deltaTime;
    //}

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject obj = col.gameObject;

        if (Utilities.CheckLayerMask(isAttackable, obj))
        {
            if (!timesHit.ContainsKey(obj))
                timesHit.Add(obj, 0);

            if (maxHits == 0)
            {
                DealDamage(obj);
            }
            else if (timesHit[obj] < maxHits)
            {
                DealDamage(obj);
            }
        }
    }

    void DealDamage(GameObject obj)
    {
        // TODO play animations and/or emit particles
        HealthController healthController = obj.GetComponent<HealthController>();
        if (healthController != null)
        {
            bool damageWasDealt;
            if (isWaterDamage)
            {
                damageWasDealt = healthController.TakeWaterDamage(damage);
            }
            else
            {
                healthController.TakeDamage(damage);
                damageWasDealt = true;
            }

            if (damageWasDealt)
            {
                // TODO Knockback call?
                float dirX = (obj.transform.position - knockBackPoint.position).normalized.x;
                Vector2 force = Utilities.DegreeToVector2(knockBackAngle);
                force.x *= dirX * knockBackForce;
                force.y *= knockBackForce;
                // FIXME Move to FixedUpdate() or is OnTriggerStay2D called on the physics update cycle (? -> it is! should it move anyway?)
                obj.GetComponent<Rigidbody2D>().velocity = new Vector2();
                obj.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
        timesHit[obj]++;
    }
}
