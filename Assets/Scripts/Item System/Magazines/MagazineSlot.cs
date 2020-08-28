using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineSlot : MonoBehaviour
{
    public MagazineType attachableMagazineType;
    public Magazine currentlyAttachedMagazine;
    public Firearm weapon;

    // Audio
    public AudioClip attachmagSound;
    public AudioClip detachMagSound;
    public AudioSource audio;

    public void AttachMagazine(Magazine magazine)
    {
        if (currentlyAttachedMagazine == null)
        {
            Debug.Log("magazine attached");
            audio.clip = attachmagSound;
            audio.Play();
            currentlyAttachedMagazine = magazine;
            currentlyAttachedMagazine.GetComponent<Rigidbody>().isKinematic = true;
            
            currentlyAttachedMagazine.slot = this;
            weapon.InsertMagazine(currentlyAttachedMagazine);
        }
    }

    public void DetachMag()
    {
        if (currentlyAttachedMagazine != null)
        {
            Debug.Log("magazine dettached");
            audio.clip = detachMagSound;
            audio.Play();
            currentlyAttachedMagazine.slot = null;
            currentlyAttachedMagazine.GetComponent<Rigidbody>().isKinematic = false;
            currentlyAttachedMagazine = null;
            weapon.EjectMagazine();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 0.1f);

            foreach (Collider c in colls)
                Debug.Log(c.gameObject.name);
        }
    }
}
