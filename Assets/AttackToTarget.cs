using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToTarget : MonoBehaviour
{
    public Transform target;
    public GameObject bullet;

    public float AttackRate = 3.0f;

    bool bCanAttack = true;
    // Update is called once per frame
    void Update()
    {
        if (bCanAttack)
        {
            Attack();
            bCanAttack = false;
            StartCoroutine(WaitToAttack());
        }
    }
    void Attack()
    {
        Bullet newBullet = Instantiate(bullet,transform.position,Quaternion.identity).GetComponent<Bullet>();
        target.GetComponent<FighterAgent>().enemyBullets.Add(newBullet.gameObject.transform);
        EnemyStat enemyStat = GetComponent<EnemyStat>();
        if (!enemyStat) return;
        newBullet.Causer = "Enemy";
        newBullet.dmg = enemyStat.dmg;
        Rigidbody rb =  newBullet.GetComponent<Rigidbody>();
        rb.velocity = (target.position - transform.position).normalized * 5;
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(AttackRate);
        bCanAttack = true;
    }
}
