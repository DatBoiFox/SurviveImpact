using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public Chainsaw chainsaw;
    private bool onCooldown;

    private void OnCollisionStay(Collision collision)
    {
        if (chainsaw.active)
        {
            if (collision.gameObject.GetComponent<IDestroyable>() != null && !onCooldown)
            {
                collision.gameObject.GetComponent<IDestroyable>().ApplyDamage(chainsaw.Damage);
                onCooldown = true;
                StartCoroutine(EffectCooldown());
            }
        }
    }

    IEnumerator EffectCooldown()
    {
        yield return new WaitForSeconds(1f);
        onCooldown = false;
    }

}
