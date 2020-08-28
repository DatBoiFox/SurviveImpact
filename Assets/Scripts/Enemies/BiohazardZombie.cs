using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiohazardZombie : Enemy, IDestroyable
{
    public bool hasSpit;
    public float spitRange;

    public ParticleSystem spitParticles;
    public GameObject acidCloud;
    public AudioSource AcidSpitAudio;

    private void Update()
    {
        roamTimer += Time.deltaTime;
        chaseTimer += Time.deltaTime;
        if (Health <= 0 && isAlive)
        {
            isAlive = false;
            spitParticles.Stop();
            Destroy();
        }

        if (isAlive && !isHit)
        {

            HandleAnimations();
            if (!targetIsInSight(this.transform, out ChaseTarget) & !isChasing & !isAttacking)
            {
                Roam();
                AI.speed = Speed;
                if (!idleAudio.isPlaying)
                    idleAudio.Play();
            }
            else if (!isAttacking)
            {
                AI.speed = Speed * 2;
                isRoaming = false;
                isChasing = true;
                Chase();
                if (!runningAudio.isPlaying)
                {
                    idleAudio.Stop();
                    runningAudio.Play();
                }
            }

            if (ChaseTarget != null || prevTarget != null)
            {
                AI.speed = Speed * 2;
                isRoaming = false;
                isChasing = true;
                Chase();
                if (!runningAudio.isPlaying)
                {
                    idleAudio.Stop();
                    runningAudio.Play();
                }
            }

            if (AI.velocity.magnitude > 0f && AI.velocity.magnitude < 2.4)
            {
                footstepsAudio.pitch = 0.38f;
                if (!footstepsAudio.isPlaying)
                    footstepsAudio.Play();
            }
            else if (AI.velocity.magnitude >= 2.4f)
            {
                footstepsAudio.pitch = 0.55f;
                if (!footstepsAudio.isPlaying)
                    footstepsAudio.Play();
            }
        }
    }
    public override IEnumerator Attack(bool isPlayer, GameObject target)
    {
        //ResetAI();

        isRoaming = false;
        AI.enabled = false;
        if (isPlayer)
            animator.SetTrigger("AttackPlayer");
        else
            animator.SetTrigger("AttackObstacle");

        runningAudio.Stop();
        idleAudio.Stop();
        attackAudio.Play();
        yield return new WaitForSeconds(1.1f);
        if (isInAttackRange())
        {
            if (isPlayer)
            {
                //target.GetComponent<PlayerObject>().playerStatus.ApplyDamage(Damage);
                target.transform.parent.GetComponent<PlayerObject>().playerStatus.ApplyDamage(Damage);
            }
            else
                target.GetComponent<Item>().ApplyDamage(Damage);
        }
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackPlayer") || animator.GetCurrentAnimatorStateInfo(0).IsName("AttackObstacle"))
            yield return new WaitForFixedUpdate();
        isAttacking = false;
        isChasing = true;
        AI.enabled = true;
    }

    public override void Roam()
    {
        if (roamTimer < maxRoamTimer)
            return;
        Move(RandomDirection(this.transform.position, 10, -1));
        roamTimer = 0;
        isRoaming = true;
    }

    private void HandleAnimations()
    {
        if (isRoaming)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else if (isChasing)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }

        if (isRoaming && AI.hasPath && AI.remainingDistance <= .1f)
        {
            ResetAI();
            isRoaming = false;
            animator.SetBool("isWalking", false);
        }

        if (isChasing && hasSpit && !isHit && AI.hasPath && /*AI.remainingDistance <= spitRange*/ Vector3.Distance(this.transform.position, prevTarget.transform.position) <= spitRange)
        {
            hasSpit = false;
            ResetAI();
            StartCoroutine(Spit());
        }

    }

    public override void Chase()
    {
        if (ChaseTarget != null)
        {
            prevTarget = ChaseTarget;
            chaseTimer = 0;
        }


        if (chaseTimer >= maxChaseTimer && !targetIsInSight(this.transform, out ChaseTarget))
        {

            isChasing = false;
            chaseTimer = 0;
            prevTarget = null;
            return;
        }

        if (prevTarget == null)
        {
            isChasing = false;
            chaseTimer = 0;
            prevTarget = null;
            return;
        }

        Move(prevTarget.transform.position);
    }

    public bool targetIsInSight(Transform origin, out GameObject target)
    {
        Collider[] tagetColliders = Physics.OverlapSphere(origin.position, sightDistance, targetMask);
        target = null;
        foreach (Collider c in tagetColliders)
        {
            target = c.gameObject;
            Vector3 directionToTarget = (target.transform.position - origin.position).normalized;

            if (Vector3.Angle(origin.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(origin.position, target.transform.position);
                if (Physics.Raycast(origin.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    isChasing = true;
                    return true;
                }
            }

        }
        target = null;
        isChasing = false;
        return false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PlayerBody")
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(Attack(true, other.gameObject));
            }
        }
        else if (other.tag == "Building")
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(Attack(false, other.gameObject));
            }
        }
    }

    private bool isInAttackRange()
    {
        if (prevTarget == null)
            return false;

        float distance = Vector3.Distance(this.transform.position, prevTarget.transform.position);

        if (distance <= attackRange)
            return true;

        return false;
    }

    public void Destroy()
    {
        Destroy(animator);
        idleAudio.Stop();
        runningAudio.Stop();
        attackAudio.Stop();
        takingDamageAudio.Stop();
        footstepsAudio.Stop();
        AcidSpitAudio.Stop();
        this.GetComponent<Collider>().enabled = false;
        AI.enabled = false;
        //this.GetComponent<Rigidbody>().isKinematic = false;
        //animator.SetTrigger("Die");
        foreach (Rigidbody rb in Root.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            Debug.Log(rb.gameObject.name);
            rb.isKinematic = false;
        }
        Destroy(this.gameObject, 20f);
        Destroy(this);

    }

    public void ApplyDamage(float damage)
    {
        if (isAlive)
        {
            isHit = true;
            AI.enabled = false;
            chaseTimer = 0;
            Health -= damage;
            StartCoroutine(HandleHit());
        }
    }

    // Handles when the enemy itself gets hit
    private IEnumerator HandleHit()
    {

        animator.SetTrigger("Hit");
        takingDamageAudio.Play();
        yield return new WaitForSeconds(1.2f);
        Debug.Log("After Hit");
        AI.enabled = true;
        isHit = false;
    }

    private IEnumerator Spit()
    {
        Debug.Log("Spit");
        animator.SetTrigger("Spit");
        AI.enabled = false;
        yield return new WaitForSeconds(0.5f);
        spitParticles.Play();
        AcidSpitAudio.Play();
        Vector3 tempPos = prevTarget.transform.position;
        Instantiate(acidCloud, tempPos, Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(1.6f);
        spitParticles.Stop();
        AcidSpitAudio.Stop();
        AI.enabled = true;
        yield return new WaitForSeconds(5f);
        hasSpit = true;

    }
}
