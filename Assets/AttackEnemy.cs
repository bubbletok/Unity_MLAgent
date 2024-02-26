using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackEnemy : MonoBehaviour
{
    public Transform target;
    public GameObject bullet;

    public float AttackRate = 3.0f;
    public int dmg = 1;

    bool bCanAttack = true;
    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            DoAttack();
            FighterAgent Agent = other.GetComponent<FighterAgent>();
            Agent.AgentAttack();
        }
    }

    void DoAttack()
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
        target.GetComponent<EnemyStat>().Hp -= dmg;
       /* Bullet newBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
        newBullet.Causer = "Agent";
        newBullet.dmg = dmg;
        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        rb.velocity = (target.transform.position - transform.position).normalized * 5;*/
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(AttackRate);
        bCanAttack = true;
    }
}
