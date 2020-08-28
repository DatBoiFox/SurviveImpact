using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider), typeof(Rigidbody))]
public abstract class Enemy : MonoBehaviour
{
    public float Health;
    public float Damage;
    public float Speed;
    public float attackRange;

    // Enemy root object, that every enemy rigid body has.
    public GameObject Root;

    // Every layer, that enemy considers target
    public LayerMask targetMask;
    // Every layer, that enemy considers obstacle
    public LayerMask obstacleMask;

    public float viewAngle;
    public float sightDistance;

    // Sounds

    public AudioSource idleAudio;
    public AudioSource runningAudio;
    public AudioSource attackAudio;
    public AudioSource takingDamageAudio;
    public AudioSource footstepsAudio;


    [SerializeField]
    protected GameObject ChaseTarget;
    public GameObject prevTarget;

    [SerializeField]
    protected float chaseTimer;
    public float maxChaseTimer;
    [SerializeField]
    protected float roamTimer;
    public float maxRoamTimer;

    //protected bool isWalking;
    [SerializeField]
    protected bool isChasing;
    [SerializeField]
    protected bool isAttacking;
    [SerializeField]
    protected bool isRoaming;
    [SerializeField]
    protected bool isHit;
    [SerializeField]
    protected bool isAlive;

    [SerializeField]
    protected NavMeshAgent AI;

    [SerializeField]
    protected Animator animator;

    // Class Methods
    public void Start()
    {
        AI.speed = Speed;
        isAlive = true;
    }

    /// <summary>
    /// Generate random direction that the zombie should move to
    /// </summary>
    /// <param name="origin">this objects position</param>
    /// <param name="distance">max distance from origin and target point</param>
    /// <param name="layerMask"></param>
    /// <returns>new position that the zombie will travel to</returns>
    protected Vector3 RandomDirection(Vector3 origin, float distance, int layerMask)
    {
        Vector3 randomDir = Random.insideUnitSphere * distance;
        randomDir += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDir, out navHit, distance, layerMask);

        return navHit.position;

    }
    public void Move(Vector3 target)
    {
        if(AI.enabled)
            AI.SetDestination(target);
    }

    protected void ResetAI()
    {
        AI.enabled = false;
        AI.enabled = true;
    }

    // Abstract Methods

    public abstract IEnumerator Attack(bool isPlayer, GameObject target);
    public abstract void Roam();
    public abstract void Chase();
    
    public void setChaseTarget(GameObject target)
    {
        prevTarget = target;
        isChasing = true;
    }

}
