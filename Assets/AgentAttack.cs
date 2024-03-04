using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            print("Enemy Hit");
            other.GetComponent<EnemyStat>().Hp -= 50;
        }
    }
}
