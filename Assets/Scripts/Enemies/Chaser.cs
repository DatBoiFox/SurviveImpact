//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class Chaser
//{

//    public float radius;
//    public float timer;
//    private float roamTimer;

//    public float chaseTimer;
//    private float chaseTimerReset;

//    public Animator anim;

//    [Header("Line Of Sight Parameters")]
//    public Transform eyes;
//    public LayerMask targetMask;
//    public LayerMask obstacleMask;
//    public float viewAngle;
//    public float sightDistance;
//    public GameObject CurrentlyChasing;

//    private void Start()
//    {
//        AI.speed = Speed;
//    }

//    public override void Attack()
//    {
//        isAttacking = true;
//        isChasing = false;
//        isRoaming = false;
//        AI.Stop();
//        AI.ResetPath();
//        AI.velocity = Vector3.zero;
//        if(CurrentlyChasing.GetComponent<Building>())
//            anim.SetTrigger("AttackObstacle");
//        else if (CurrentlyChasing.GetComponent<PlayerObject>() || CurrentlyChasing.transform.parent.GetComponent<PlayerObject>())
//            anim.SetTrigger("AttackPlayer");
//        Debug.Log("Attack");
//        AI.Resume();
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if (other.tag == "Player" || other.tag == "Building")
//        {
//            if (!isAttacking)
//            {
//                isAttacking = true;
//                chaseTimerReset -= chaseTimer; 
//                Attack();
//            }
//        }
//    }

//    private void Update()
//    {

//        ManageStates();
//        roamTimer += Time.deltaTime;
//        chaseTimerReset += Time.deltaTime;
        
//        if (!isAttacking)
//            ChaseTarget();
//        if (!isAttacking && !isChasing)
//            Roam();

//        if(chaseTimerReset >= chaseTimer)
//        {
            
//            isChasing = false;
//            CurrentlyChasing = null;
//            chaseTimerReset = 0;
//        }
//    }

//    public override void Roam()
//    {
//        if (roamTimer >= timer)
//        {
//            Vector3 newPos = RandomDirection(this.transform.position, radius, -1);
//            Move(newPos);
//            roamTimer = 0;
//            isRoaming = true;
//        }
//    }

//    public void ChaseTarget()
//    {
        
//        GameObject target;
//        if (playerIsInSight(this.transform, out target))
//        {
//            isRoaming = false;
//            Debug.DrawLine(eyes.position, target.transform.position, Color.red);
//            CurrentlyChasing = target;
//            isChasing = true;
            
//        }

//        if(CurrentlyChasing != null)
//        {
//            Move(CurrentlyChasing.transform.position);
//        }
//        else
//        {
//            isChasing = false;
//        }
//    }

//    private bool playerIsInSight(Transform origin, out GameObject target)
//    {
//        Collider[] tagetColliders = Physics.OverlapSphere(origin.position, sightDistance, targetMask);
//        target = null;
//        foreach (Collider c in tagetColliders)
//        {
//            target = c.gameObject;
//            Vector3 directionToTarget = (target.transform.position - origin.position).normalized;

//            if (Vector3.Angle(origin.forward, directionToTarget) < viewAngle / 2)
//            {
//                float distanceToTarget = Vector3.Distance(origin.position, target.transform.position);
//                if (!Physics.Raycast(origin.position, directionToTarget, distanceToTarget, obstacleMask))
//                {
//                    return true;
//                }
//            }

//        }
//        return false;

//    }
//    private bool hasAttacked;
//    private void ManageStates()
//    {
        

//        if(isRoaming && AI.hasPath && AI.remainingDistance <= .1f)
//        {
//            isRoaming = false;
//            AI.enabled = false;
//            AI.enabled = true;
//        }

//        if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackPlayer") || anim.GetCurrentAnimatorStateInfo(0).IsName("AttackObstacle"))
//        {
//            anim.SetBool("isIdle", true);
//            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
//            {
//                isAttacking = false;
//                hasAttacked = false;
//                //CurrentlyChasing = null;
//            }

//        }


//        if (isRoaming || isChasing)
//        {
//            anim.SetBool("isIdle", false);
//        }
//        else
//        {
//            anim.SetBool("isIdle", true);
//        }
//    }

//    public override void Chase()
//    {
//        throw new System.NotImplementedException();
//    }
//}
