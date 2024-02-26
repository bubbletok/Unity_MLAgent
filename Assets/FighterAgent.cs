using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;
using Unity.MLAgents.Policies;

public class FighterAgent : Agent
{
    public GameObject Target;
    public GameObject bullet;
    public Transform Attacker;
    public List<Transform> enemyBullets = new List<Transform>();
    public float forceMultiplier = 10;
    Rigidbody rBody;
    AgentStat stat;
    EnemyStat enemyStat;
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        stat = GetComponent<AgentStat>();
        enemyStat = Target.GetComponent<EnemyStat>();
    }

    private void Update()
    {
        for(int i=0; i<enemyBullets.Count; i++)
        {
            if (!enemyBullets[i])
            {
                enemyBullets.RemoveAt(i);
            }
        }
    }

    public override void OnEpisodeBegin()
    {
        print(GetCumulativeReward());
        stat.Hp = 100;
        enemyStat.Hp = 100;

        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0.5f, 0);
        //transform.localPosition = new Vector3(0, 0.5f, 0);

        Target.transform.localPosition = new Vector3
            (Random.Range(-8,8), 0.5f, Random.Range(8,-8));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int bulletCount = 0;
        if (enemyBullets.Count > 0)
        {
            foreach (Transform bullet in enemyBullets)
            {
                if (bullet == null) continue;
                bulletCount++;
            }
        }
        // bullets position 
        if (enemyBullets.Count > 0)
        {
            foreach (Transform bullet in enemyBullets)
            {
                if (bullet == null) continue;
                sensor.AddObservation(bullet.localPosition);
            }
        }
        // GetComponent<BehaviorParameters>().BrainParameters.VectorObservationSize = 10 + bulletCount + 1;
        // Target and Agnet positions
        sensor.AddObservation(Target.transform.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

        // Target and Agent Hp
        sensor.AddObservation(stat.Hp);
        EnemyStat enemyStat = Target.GetComponent<EnemyStat>();
        if (enemyStat)
        {
            sensor.AddObservation(enemyStat.Hp);
        }

        // Enemy Attacker positions
        //sensor.AddObservation(Attacker.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards

        // Hp lower than 0
        if (stat.Hp<=0)
        {
            AddReward(-10.0f);
            EndEpisode();
        }

        // Enemy Died
        if (enemyStat.Hp <= 0)
        {
            AddReward(10.0f);
            EndEpisode();
        }
        AddReward(0.1f);
    }

    public void AgentHit()
    {
        AddReward(-1f);
    }

    public void AgentAttack()
    {
        AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Jump");
    }
}
