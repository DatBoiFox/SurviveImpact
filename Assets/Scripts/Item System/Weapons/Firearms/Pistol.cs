using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Firearm
{
    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
    }

    //public override void Shoot()
    //{
    //    if (canShoot)
    //    {
    //        canShoot = false;
    //        audioSource.clip = EmptySound;
    //        if (magazine != null)
    //        {
    //            if (magazine.bulletsCount > 0)
    //            {
    //                RaycastHit hit;
    //                if (Physics.Raycast(pipe.transform.position, pipe.transform.forward, out hit))
    //                {
    //                    if (hit.transform.gameObject.tag == "Pickable" || hit.transform.gameObject.GetComponent<IDestroyable>() != null)
    //                    {
    //                        if (hit.transform.GetComponent<IDestroyable>() != null && hit.transform.GetComponent<Tree>() == null)
    //                        {
    //                            hit.transform.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
    //                        }
    //                        if (hit.transform.GetComponent<Rigidbody>())
    //                            hit.transform.GetComponent<Rigidbody>().AddForce(pipe.transform.forward * inpactForce, ForceMode.Impulse);
    //                    }
    //                }
    //                audioSource.clip = ShotSound;
    //                magazine.bulletsCount--;
    //            }
    //        }
    //        audioSource.Play();
    //    }
    //}
    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    private void Update()
    {
        if (magazine != null)
        {
            FindObjectOfType<SaveManager>().evaluateSceneItemDifference(magazine.gameObject);
        }
    }


    public override void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            audioSource.clip = EmptySound;
            if (magazine != null)
            {

                if (magazine.bulletsCount > 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(pipe.transform.position, pipe.transform.forward, out hit))
                    {
                        if (hit.transform.gameObject.tag == "Pickable" || hit.transform.gameObject.GetComponent<IDestroyable>() != null || hit.transform.root.gameObject.GetComponent<IDestroyable>() != null)
                        {
                            if (hit.transform.GetComponent<Tree>() == null)
                                if (hit.transform.GetComponent<IDestroyable>() != null)
                                {
                                    hit.transform.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
                                }
                                else if (hit.transform.root.GetComponent<IDestroyable>() != null)
                                {
                                    hit.transform.root.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
                                }
                        }

                        if (hit.transform.GetComponent<Rigidbody>())
                        {
                            hit.transform.GetComponent<Rigidbody>().AddForce(pipe.transform.forward * inpactForce, ForceMode.Impulse);
                        }

                    }
                    audioSource.clip = ShotSound;
                    magazine.bulletsCount--;
                    Destroy(Instantiate(shootFX, pipe.transform.position, pipe.transform.rotation), 0.1f);
                }
            }
            audioSource.Play();
        }
    }

    public override void EjectMagazine()
    {
        magazine = null;
    }

    public override void InsertMagazine(Magazine magazine)
    {
        this.magazine = magazine;
    }

    public override void PickUp(GameObject hand)
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        //this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.layer = 8;
        this.transform.parent = hand.transform;
        int side = 0;
        //if (LHand != null)
        //    side = LHand.name.Equals("RightHand") ? 1 : -1;
        //else if (RHand != null)
        //    side = LHand.name.Equals("RightHand") ? 1 : -1;
        this.transform.localPosition = new Vector3(0.0126f*side, -0.0331f, -0.0388f); 
        //this.transform.rotation = hand.transform.rotation;
        this.transform.localRotation = Quaternion.Euler(1.106f, 93.192f, 41.994f);
    }
    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        StartCoroutine(PlacementCooldown());
        this.transform.localScale = Vector3.one;
        this.GetComponent<Rigidbody>().isKinematic = false;
        //this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    

}
