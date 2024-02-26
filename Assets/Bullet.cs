using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public int dmg;
    public string Causer;
    // Update is called once per frame

    private void Start()
    {
        StartCoroutine(WaitToDestory());
    }
    void Update()
    {
        
    }

    public void ToDestory()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Causer == "Agent" && other.tag == "Enemy")
        {
            EnemyStat enemy = other.GetComponent<EnemyStat>();
            if (!enemy) return;
            enemy.Hp -= dmg;
            Destroy(gameObject);
        }
        if (Causer == "Enemy" && other.tag == "Agent")
        {
            FighterAgent Agent = other.GetComponent<FighterAgent>();
            AgentStat AgentStat = other.GetComponent<AgentStat>();
            if (!AgentStat) return;
            AgentStat.Hp -= dmg;
            Agent.AgentHit();
            Destroy(gameObject);
        }
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitToDestory()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
