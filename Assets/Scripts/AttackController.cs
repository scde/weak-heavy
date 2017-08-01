using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes GameObject Attacker:
// - Listens to attack input
// - Spawns damage zones (with dmg, position, lifetime, speed, knockback, etc.)
// - Checks current weapon (which e.g. holds damage zone vars)
// - Handles CD
// - Handles Player-Animation (no projectiles)
public class AttackController : MonoBehaviour
{

    //[HideInInspector]
    public GameObject[] weapons;
    public int weaponId = 1;
    public float damage = 20.0f;
    public float knockBack;
    public float knockBackRadius;
    //public float meleeRate;
    public float attackCooldown = 0.1f;
    public LayerMask isAttackable;
    // TODO use prefab and spawn at AttackCheck Position
    public GameObject attackCheck;

    //float nextMelee;
    //int shootableMask;
    bool attacking = false;
    float curAttackTime = 0.0f;

    Animator anim;
    BoxCollider2D box;
    Collider2D[] hitColliders;
    ContactFilter2D contactFilter2D;
    //HeavyController myPC;

    // Use this for initialization
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            isAttackable = LayerMask.GetMask(new string[] { "Enemy" });
        }
        else
        {
            //         string[] layers = new string[] { "Player_Weak", "Player_Heavy" };
            //layers = new string[2];
            //layers.SetValue("Player_Weak", 2);
            isAttackable = LayerMask.GetMask(new string[] { "Player_Weak", "Player_Heavy" });
        }
        anim = GetComponent<Animator>();
        attackCheck.SetActive(false);
        DamageController dmgScript = attackCheck.GetComponent<DamageController>();
        dmgScript.isAttackable = isAttackable;
        dmgScript.damage = damage;
        //box = attackCheck.GetComponent<BoxCollider2D>();
        //myPC = transform.root.GetComponent<HeavyController> ();
        //nextMelee = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //      if (attacking) {
        //          // TODO angle
        //          Collider2D col = Physics2D.OverlapBox(attackCheck.transform.position, box.bounds.size, 0.0f, isAttackable);
        //          if (col != null) {
        //		HealthController healthController = col.gameObject.GetComponent<HealthController>();
        //		if (healthController != null)
        //		{
        //			healthController.TakeDamage(damage);
        //		}
        //	}
        //	//print(Physics2D.OverlapBox(box.transform.position, box.bounds.size, 0.0f, contactFilter2D, hitColliders));
        //}

        //      if (hitColliders != null) {
        //	foreach (Collider2D col in hitColliders)
        //	{
        //		HealthController healthController = col.gameObject.GetComponent<HealthController>();
        //              if (healthController != null) {
        //			healthController.TakeDamage(damage);
        //		}
        //	}
        //          hitColliders = null;
        //}
        //float melee = Input.GetAxis ("Fire2");

        //if (melee > 0 && nextMelee < Time.time && !(myPC.getRunning ())) {
        //	myAnim.SetTrigger ("gunMelee");
        //	nextMelee = Time.time + meleeRate;

        //	//do damage
        //	Collider[] attacked = Physics.OverlapSphere(transform.position, knockBackRadius, shootableMask);
        //}
    }

    void Update()
    {
        // TODO move to its own script (item, weapon switcher/handler)
        if (weaponId >= 0)
        {
            foreach (GameObject weapon in weapons)
            {
                weapon.SetActive(false);
            }
            if (0 < weaponId && weaponId <= weapons.Length)
            {
                weapons[weaponId - 1].SetActive(true);
            }
            anim.SetInteger("Weapon", weaponId);
        }

        if (Input.GetButtonDown("Fire1_1") && !attacking)
        {
            attacking = true;
            //Play Animation
            // maybe use attackCooldown as value to determine animation/attack speed
            anim.SetTrigger("Attack");
            //anim.SetBool("Attack", attacking);
            curAttackTime = attackCooldown;
            if (!attackCheck.activeSelf)
            {
                attackCheck.SetActive(true);
            }
        }

        if (curAttackTime > 0.0f)
        {
            curAttackTime -= Time.deltaTime;
        }
        else if (attacking)
        {
            attacking = false;
            //anim.SetBool("Attacking", attacking);
            curAttackTime = 0.0f;
            if (attackCheck.activeSelf)
            {
                attackCheck.SetActive(false);
            }
        }
    }
}
