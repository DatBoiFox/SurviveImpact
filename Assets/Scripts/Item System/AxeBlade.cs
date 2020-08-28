using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBlade : MonoBehaviour
{
    public float Damage;
    [SerializeField]
    private WoodCutingAxe axe;
    [Range(0, 5)]
    public float attackSpeed;
    [SerializeField]
    private AudioSource audio;

    //public AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.GetComponent<IDestroyable>() != null)
        {

            if (collision.impulse.magnitude > attackSpeed)
            {
                collision.transform.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
                audio.PlayOneShot(audio.clip);
            }
        }
        else if (collision.transform.root.GetComponent<IDestroyable>() != null)
        {
            if (collision.impulse.magnitude > attackSpeed)
            {
                collision.transform.root.gameObject.GetComponent<IDestroyable>().ApplyDamage(Damage);
                audio.PlayOneShot(audio.clip);
            }
        }
        if (collision.impulse.magnitude > attackSpeed)
        {
            audio.Play();
        }
    }

}
