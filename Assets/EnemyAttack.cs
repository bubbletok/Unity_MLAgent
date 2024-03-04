using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Agent")
        {
            //print("Agent Hit");
            other.GetComponent<BattleAgent>().Hp -= 1;
        }
    }
}
