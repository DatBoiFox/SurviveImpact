using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    Melee meleeWeapon;

    public AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.GetComponent<IDestroyable>() != null)
        {

            if (collision.impulse.magnitude > meleeWeapon.attackSpeed)
            {
                collision.transform.gameObject.GetComponent<IDestroyable>().ApplyDamage(meleeWeapon.Damage);
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        else if (collision.transform.root.GetComponent<IDestroyable>() != null)
        {
            if (collision.impulse.magnitude > meleeWeapon.attackSpeed)
            {
                collision.transform.root.gameObject.GetComponent<IDestroyable>().ApplyDamage(meleeWeapon.Damage);
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
        if (collision.impulse.magnitude > meleeWeapon.attackSpeed)
        {
            audioSource.Play();
        }
    }

}
