using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class Submashine : Firearm
{

    private AudioClip currentClip;


    public float shootSpeed;

    public GameObject bulletHole;

    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
    }
    private void Update()
    {
        HandleSecondHand();
        if (magazine != null)
        {
            FindObjectOfType<SaveManager>().evaluateSceneItemDifference(magazine.gameObject);
        }
        //if (LHand != null && RHand != null)
        //{
        //    this.transform.LookAt(LastHand.transform);
        //}
        //else if(LHand != null && RHand == null || LHand == null && RHand != null)
        //{
        //    this.transform.localPosition = new Vector3(-0.0017f * 1, 0.0166f, -0.0848f);
        //    this.transform.localRotation = Quaternion.Euler(50.613f, 2.595f, 9.499001f);
        //}

        //if (LHand.GetComponent<HandGrab>().objectInHand != this.gameObject)
        //    LHand = null;
        //if (RHand.GetComponent<HandGrab>().objectInHand != this.gameObject)
        //    RHand = null;

        //if (LHand == null && RHand != null)
        //    PickUp(LHand);
        //if (RHand == null && LHand != null)
        //{
        //    PickUp(RHand);
        //}
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }
    public override void Shoot()
    {
        if (canShoot) {
            audioSource.clip = EmptySound;
            currentClip = EmptySound;
            canShoot = false;
            StartCoroutine(ShotCooldown());
            if (magazine != null)
            {
                
                if (magazine.bulletsCount > 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(pipe.transform.position, pipe.transform.forward, out hit))
                    {
                        Destroy(Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)), 1f);
                        if (hit.transform.gameObject.tag == "Pickable" || hit.transform.gameObject.GetComponent<IDestroyable>() != null || hit.transform.root.gameObject.GetComponent<IDestroyable>() != null)
                        {
                            if(hit.transform.GetComponent<Tree>() == null)
                                if (hit.transform.GetComponent<IDestroyable>() != null)
                                {
                                    hit.transform.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
                                }else if (hit.transform.root.GetComponent<IDestroyable>() != null)
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
                    currentClip = ShotSound;
                    magazine.bulletsCount--;
                    Destroy(Instantiate(shootFX, pipe.transform.position, pipe.transform.rotation), 0.1f);
                }
            }
            audioSource.PlayOneShot(currentClip, 1f);
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
        if (hand.GetComponent<HandGrab>().handType == SteamVR_Input_Sources.LeftHand)
        {
            LHand = hand;
            handQueue.Add(hand);
        }
        else
        {
            RHand = hand;
            handQueue.Add(hand);
        }

        if (handQueue.Count > 1)
        {
            if (twohanded)
            {
                //this.transform.LookAt()
                return;
            }
        }

        this.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.layer = 8;
        Debug.Log(hand.name);
        this.transform.parent = hand.transform;
        int side = 1;
        this.transform.localPosition = new Vector3(-0.0017f * side, 0.0166f, -0.0848f);
        this.transform.localRotation = Quaternion.Euler(50.613f, 2.595f, 9.499001f);
    }
    public override void Drop(GameObject hand)
    {
        if(handQueue[0] == hand)
        {
            Debug.Log("PrimaryHand");
            this.transform.parent = null;
            StartCoroutine(PlacementCooldown());
            this.transform.localScale = Vector3.one;
            this.GetComponent<Rigidbody>().isKinematic = false;
            LHand = null;
            RHand = null;
            handQueue = new List<GameObject>();
        }
        handQueue.Remove(hand);

        if(handQueue.Count <= 0)
            this.gameObject.layer = 0;

    }


    private IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(shootSpeed);
        canShoot = true;
    }

    private void HandleSecondHand()
    {
        if (handQueue.Count <= 0)
            return;

        if (handQueue.Count > 1)
        {
            this.transform.LookAt(new Vector3(handQueue[1].transform.position.x, handQueue[1].transform.position.y, handQueue[1].transform.position.z));
        }
        else
        {
            this.transform.localPosition = new Vector3(-0.0017f * 1, 0.0166f, -0.0848f);
            this.transform.localRotation = Quaternion.Euler(50.613f, 2.595f, 9.499001f);
        }
    }

}
