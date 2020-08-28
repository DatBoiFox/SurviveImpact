using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Firearm : Weapon
{
    public Magazine magazine;
    public MagazineSlot magazineSlot;
    public Transform pipe;
    public GameObject shootFX;

    // Audio
    public AudioSource audioSource;
    public AudioClip ShotSound;
    public AudioClip EmptySound;

    // The amount of force added to the item that is hit
    public float inpactForce = 10;

    public abstract void InsertMagazine(Magazine magazine);
    public abstract void EjectMagazine();
    public abstract void Shoot();
    public bool canShoot = true;
    public IEnumerator PlacementCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        this.gameObject.layer = 0;
    }
}
