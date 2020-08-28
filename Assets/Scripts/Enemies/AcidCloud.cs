using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidCloud : MonoBehaviour
{
    public float damage;
    //damage over time, timer (damage tick)
    private float damageTimer;
    public float maxDamageTimer;
    private ParticleSystem particleSystem;
    private SphereCollider hitBox;
    public LayerMask layerMask;
    private bool damagedPlayer;
    void Start()
    {
        hitBox = this.GetComponent<SphereCollider>();
        particleSystem = this.GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.startLifetime);
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;
        Damage();
    }

    //Apply damage to every colliding object
    private void Damage()
    {
        if(damageTimer >= maxDamageTimer)
        {
            Debug.Log("Damage: " + Mathf.Clamp01(damage));
            damagedPlayer = false;
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, hitBox.radius, layerMask);

            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Item>())
                {
                    colliders[i].GetComponent<Item>().ApplyDamage(damage);
                }
                else if (colliders[i].transform.root.GetComponent<Item>())
                {
                    colliders[i].transform.root.GetComponent<Item>().ApplyDamage(damage);
                }
                else if (colliders[i].GetComponent<PlayerObject>() && !damagedPlayer)
                {
                    colliders[i].GetComponent<PlayerObject>().playerStatus.ApplyDamage(damage);
                    damagedPlayer = true;
                }
                else if (colliders[i].transform.root.GetComponent<PlayerObject>() && !damagedPlayer)
                {
                    colliders[i].transform.root.GetComponent<PlayerObject>().playerStatus.ApplyDamage(damage);
                    damagedPlayer = true;
                }
            }
            damageTimer = 0;

        }
    }

}
