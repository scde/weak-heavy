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
    public Transform[] attacks;
    public DamageController[] damages;
    // TODO move to item/weapon script and send (ref) to damage controller
    //public float damage = 20.0f;
    //public float knockBack;
    //public float knockBackRadius;
    //public float meleeRate;
    public float attackCooldown = 0.1f;
    public LayerMask isAttackable;

    int curWeapon;
    float curAttackTime;
    bool attacking;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (gameObject.CompareTag("Player"))
        {
            isAttackable = LayerMask.GetMask(new string[] { "Enemy" });
        }
        else
        {
            isAttackable = LayerMask.GetMask(new string[] { "Player_Weak", "Player_Heavy" });
        }

        SwitchWeapon(curWeapon);
    }

    public void SwitchWeapon(int newWeapon)
    {
        curWeapon = newWeapon;
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        if (0 < curWeapon && curWeapon <= weapons.Length)
        {
            weapons[curWeapon - 1].SetActive(true);
        }
        anim.SetInteger("Weapon", curWeapon);
    }

    void Update()
    {
        // TODO move to its own script (item, weapon switcher/handler)
        //SwitchWeapon(curWeapon);

        if (curAttackTime > 0.0f)
        {
            curAttackTime -= Time.deltaTime;
        }
        else if (attacking)
        {
            attacking = false;
            //anim.SetBool("Attacking", attacking);
            curAttackTime = 0.0f;
        }
    }

    public void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            //Play Animation
            // maybe use attackCooldown as value to determine animation/attack speed
            anim.SetTrigger("Attack");
            //anim.SetBool("Attacking", attacking);
            curAttackTime = attackCooldown;

            Instantiate(damages[curWeapon], attacks[curWeapon].position, Quaternion.identity);
        }
    }
}
