using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Explosives
{
    private void Update()
    {
        if (safetyPin != null && safetyPin.GetComponent<FixedJoint>() == null && !exploded)
        {
            safetyPin.gameObject.transform.parent = null;
            Destroy(safetyPin.gameObject, 2f);
            StartCoroutine(DoExplosion());
            exploded = true;
        }
        
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        this.transform.localScale = Vector3.one;
        this.GetComponent<Rigidbody>().isKinematic = false;
        LHand = null;
        RHand = null;
    }

    public override void PickUp(GameObject hand)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 8;
        this.transform.parent = hand.transform;
        int side = 1;
        this.transform.localPosition = new Vector3(-0.0017f * side, 0.0166f, -0.0848f);
        this.transform.localRotation = Quaternion.Euler(50.613f, 2.595f, 9.499001f);
    }


    public override void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        audioSource.PlayOneShot(audioSource.clip);
        foreach(Collider c in colliders)
        {
            if (c.GetComponent<Rigidbody>())
            {
                c.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
            }
            if (c.GetComponent<Item>())
            {
                c.GetComponent<Item>().ApplyDamage(Damage);
            }
            if(c.gameObject.tag == "Player")
            {
                Debug.Log(c.name);
                //c.transform.parent.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
                c.transform.parent.GetComponent<PlayerObject>().playerStatus.ApplyDamage(Damage * 1f);

            }
        }
        foreach (MeshRenderer mr in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            mr.enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void RemoveSafetyPin()
    {
        safetyPin = null;
    }

    private IEnumerator DoExplosion()
    {
        yield return new WaitForSeconds(explodeAfterSeconds);
        Explode();
        Instantiate(explosionFX, this.transform.position, Quaternion.identity);
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(this.gameObject);
    }
}
