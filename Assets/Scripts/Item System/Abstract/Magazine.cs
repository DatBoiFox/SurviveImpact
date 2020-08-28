using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magazine : Item
{
    public MagazineType magazineType;
    // The size of the magazine
    public int size;
    // Current bullet count
    public int bulletsCount;
    // Weapon slot that attaches magazines.
    public MagazineSlot slot;
    public AmmunitionType ammunitionType;

    public abstract void AddBullets(Ammunition ammo);
    
    // Enables object's collider
    public IEnumerator PlacementCooldown()
    {
        yield return new WaitForSeconds(1);
        this.GetComponent<Collider>().enabled = true;
    }

}
