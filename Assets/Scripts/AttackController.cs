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

    public Transform[] attacks;
    public DamageController[] damages;
    public float attackCooldown = 0.1f;
    public LayerMask isAttackable;

    private int curAttack;
    private float curAttackTime;
    private bool attacking;
    private ItemController itemController;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        itemController = GetComponent<ItemController>();
        if (itemController != null)
        {
            SwitchWeapon(itemController.EquipedItem);
        }
        else
        {
            curAttack = 0;
        }

        if (gameObject.CompareTag("Player"))
        {
            isAttackable = LayerMask.GetMask(new string[] { "Enemy" });
        }
        else
        {
            isAttackable = LayerMask.GetMask(new string[] { "Player_Weak", "Player_Heavy" });
        }
    }

    public void SwitchWeapon(ItemID newWeapon)
    {
        // if Armor is equiped use claw attack
        if (newWeapon == ItemID.Left)
        {
            curAttack = (int)ItemID.None;
        }
        else
        {
            curAttack = (int)newWeapon;
        }
    }

    void Update()
    {
        if (curAttackTime > 0.0f)
        {
            curAttackTime -= Time.deltaTime;
        }
        else if (attacking)
        {
            attacking = false;
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
            curAttackTime = attackCooldown;

            Instantiate(damages[curAttack], attacks[curAttack].position, Quaternion.identity);
        }
    }
}
