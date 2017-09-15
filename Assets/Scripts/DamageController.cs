using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Damager/DamageZone:
// - Holds & Handles (onetime/continues) dmg, (fixed/relative) position, lifetime, speed, knockback, etc. of certain attack
// - Handles collision & keeps track of them
// - Disables itself after lifetime
public class DamageController : MonoBehaviour
{
    public bool isWaterDamage;
    public float lifetime;
    public float speed;
    public Transform followPoint;
    public Transform knockBackPoint;
    public float knockBackForce = 400.0f;
    public float knockBackAngle = 80.0f;
    public float damage = 20.0f;
    // if maxHits = 0 unlimited Hits
    public int maxHits = 1;
    public LayerMask isAttackable;

	public bool HeavyIsLauncher = false;

	private HeavyController HeavyLauncher;
	private turnToPlayer EnemyLauncher;
	private Transform LauncherTransform;

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

		if (lifetime > 0)
			Destroy(gameObject, lifetime);

		if (HeavyIsLauncher) {
			HeavyLauncher = FindObjectOfType<HeavyController> ();
		} else {
			EnemyLauncher = FindObjectOfType<turnToPlayer> ();
		}


		if (HeavyLauncher != null) {
			LauncherTransform = HeavyLauncher.transform;
		} else if (EnemyLauncher != null) {
			LauncherTransform = EnemyLauncher.transform;
		}

		if (LauncherTransform.rotation.y < 0) {
			transform.rotation = Quaternion.Euler (0, 0, 180);
		} else {
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}

        if (knockBackPoint == null)
            knockBackPoint = transform;

        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
		if (speed != 0)
        {
			rb2d.velocity = transform.right * speed;
        }
    }

	void Update()
	{
		// TODO object pooling (instead of instantiate-destroy-cycle):
		// https://unity3d.com/learn/tutorials/topics/scripting/object-pooling?playlist=17117
		if (lifetime <= 0)
		{
		gameObject.SetActive(false);
		}
		lifetime -= Time.deltaTime;
	}


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
