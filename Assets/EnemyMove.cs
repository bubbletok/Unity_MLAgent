using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    Vector3 TargetPos;
    public float MovementMultiplier = 0.01f;
    //public float TimeToChangePos = 3.5f;
    public float MaxVelocity = 5.0f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindNewPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(TargetPos, transform.position) <= 1f)
        {
            FindNewPos();
        }
        else
        {
            Vector3 dir = (TargetPos - transform.position).normalized;
            dir.x *= MovementMultiplier;
            dir.y = 0;
            dir.z *= MovementMultiplier;
            rb.AddForce(dir);
            //transform.Translate((TargetPos - transform.position).normalized);
        }
        Vector2 vel = rb.velocity;
        vel.x = vel.x >= MaxVelocity ? MaxVelocity : vel.x;
        vel.y = vel.y >= MaxVelocity ? MaxVelocity : vel.y;
        rb.velocity = vel;        
    }

/*    IEnumerator WaitToFindNewPos()
    {
        yield return new 
    }*/

    void FindNewPos()
    {
        //print("Find New Position");
        TargetPos = new Vector3
    (Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }
}
