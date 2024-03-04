using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BattleAgent : Agent
{
    public float forceMultiplier = 10;

    public GameObject Attacker;

    public int Hp;
    Rigidbody Rb;

    public GameObject Target;
    EnemyStat TargetStat;

    bool bCanAttack = true;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        TargetStat = Target.GetComponent<EnemyStat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnEpisodeBegin()
    {
        Hp = 100;
        Target.GetComponent<EnemyStat>().Hp = 100;

        Rb.angularVelocity = Vector3.zero;
        Rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0.5f, 0);

        Rigidbody TargetRb = Target.GetComponent<Rigidbody>();
        TargetRb.velocity = Vector3.zero;
        TargetRb.angularVelocity = Vector3.zero;

        Target.transform.localPosition = new Vector3
            (Random.Range(-8, 8), 0.5f, Random.Range(8, -8));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agnet positions
        sensor.AddObservation(Target.transform.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(Rb.velocity.x);
        sensor.AddObservation(Rb.velocity.z);

        // Target and Agent Hp
        sensor.AddObservation(Hp);
        sensor.AddObservation(TargetStat.Hp);

        // bool variable for that Agent can attack
        sensor.AddObservation(bCanAttack);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 3
        // Actions for the movement
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        Rb.AddForce(controlSignal * forceMultiplier);

        // Action for the Attack
        bool bToAttack = actionBuffers.ContinuousActions[2] == 1;
        if (bToAttack && bCanAttack)
        {
            bCanAttack = false;
            Attacker.SetActive(true);
            StartCoroutine(WaitToDisapper());
            StartCoroutine(WaitToAttack());
        }
        if(Vector3.Distance(transform.position,Target.transform.position) <= 2.5f)
        {
            AddReward(0.1f);
        }

        if (Hp <= 0)
        {
            SetReward(-10.0f);
            EndEpisode();
        }
        else if (TargetStat.Hp <= 0)
        {
            SetReward(10.0f);
            EndEpisode();
        }
        // if fall off from the ground
        else if (this.transform.localPosition.y < 0 )
        {
            EndEpisode();
        }
    }
    IEnumerator WaitToDisapper()
    {
        yield return new WaitForSeconds(0.2f);
        Attacker.SetActive(false);
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(1.5f);
        bCanAttack = true;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[2] = Input.GetAxis("Jump");
    }
}
