using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public int Hp;
    public int dmg;

    private void Update()
    {
        print("Enemy Hp: " + Hp);
    }
}
