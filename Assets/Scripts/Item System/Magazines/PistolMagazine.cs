using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolMagazine : Magazine
{
    private Vector3 scale;
    private void Awake()
    {
        //ItemID = this.GetType().Name.GetHashCode();
        scale = this.transform.localScale;
    }

    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<MagazineSlot>() && other.GetComponent<MagazineSlot>().attachableMagazineType == magazineType && LHand == null && RHand == null)
        {
            //if(Hand != null)
            //    Hand.GetComponent<EditorGrabtest>().ReleaseObject();
            other.GetComponent<MagazineSlot>().AttachMagazine(this);
            GetComponent<BoxCollider>().enabled = false;
            transform.position = slot.transform.Find("AttachPoint").position;
            transform.rotation = slot.transform.Find("AttachPoint").rotation;
            transform.parent = slot.transform.Find("AttachPoint");
        }
        else if (other.GetComponent<Ammunition>())
        {
            AddBullets(other.GetComponent<Ammunition>());
        }
    }

    public override void AddBullets(Ammunition ammo)
    {
        if (ammo.ammunitionType != ammunitionType)
            return;

        if (bulletsCount == size)
            return;

        if (bulletsCount + ammo.Quantity > size)
        {

            Debug.Log(size);
            ammo.Quantity = Mathf.Abs(size - (ammo.Quantity + bulletsCount));
            bulletsCount = size;
        }
        else if (bulletsCount + ammo.Quantity <= size)
        {
            bulletsCount += ammo.Quantity;
            Destroy(ammo.gameObject);
        }
    }

    public override void Drop(GameObject hand)
    {
        this.transform.parent = null;
        //this.transform.localScale = scale;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    public override void PickUp(GameObject hand)
    {
        // this.transform.localScale = scale;
        this.transform.parent = null;
        //StartCoroutine(PlacementCooldown());
        if (slot != null)
        {
            StartCoroutine(PlacementCooldown());
            slot.DetachMag();
            if(LHand != null && RHand == null)
                LHand.GetComponent<HandGrab>().GrabObject();
            else if (RHand != null && LHand == null)
                RHand.GetComponent<HandGrab>().GrabObject();
            else if (RHand != null && LHand != null)
            {
                RHand.GetComponent<HandGrab>().GrabObject();
                LHand.GetComponent<HandGrab>().GrabObject();
            }
        }
    }

    
}


//private void OnTriggerStay(Collider other)
//{
//    if (other.gameObject.tag == "WeaponMagazinePlace" && !inGun)
//    {
//        Hand.GetComponent<EditorGrabtest>().ReleaseObjectPublic();
//        //gameObject.GetComponent<BoxCollider>().enabled = false;
//        gameObject.GetComponent<Rigidbody>().isKinematic = true;
//        Firearm weapon = other.gameObject.transform.parent.GetComponent<Firearm>();
//        inGun = true;
//        if (!weapon.hasMag)
//            weapon.InsertMagazine(this);
//    }
//}


//private void OnTriggerExit(Collider other)
//{
//    if (other.gameObject.tag == "WeaponMagazinePlace" && inGun)
//    {
//        Debug.Log("Exit");
//        inGun = false;
//    }
//}