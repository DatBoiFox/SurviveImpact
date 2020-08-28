using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour, IDestroyable
{
    public float Health;
    // Wood prefab that will be dropped after the tree is chopped down
    public GameObject wood;
    // Sapling prefab that will be dropped after the tree is chopped down.
    public GameObject sapling;

    // Tree Falling
    private bool started = false;
    private float fallTimer = 2;
    private float tempTimer = 0;


    public void ApplyDamage(float damage)
    {
        Health -= damage;
    }

    private void Update()
    {
        if (Health <= 0)
            Fall();
    }
    private void Start()
    {
        FindObjectOfType<SaveManager>().SaveItem(Application.loadedLevelName, this.gameObject);
    }

    public void Destroy()
    {
        this.GetComponent<Collider>().enabled = false;
        Instantiate(wood, this.transform.position + Vector3.up, this.transform.localRotation);
        Instantiate(wood, this.transform.position + Vector3.up, this.transform.localRotation);
        Instantiate(wood, this.transform.position + Vector3.up, this.transform.localRotation);

        if (Random.Range(0,5) >= 2)
            Instantiate(sapling, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + .5f, this.transform.localPosition.z), Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void Fall()
    {
        if (!started)
        {
            started = true;
            Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, FindObjectOfType<BoddyFollow>().gameObject.transform.eulerAngles.y, transform.eulerAngles.z);

            transform.rotation = Quaternion.Euler(eulerRotation);
        }

        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }

        if (transform.eulerAngles.x < 85f)
        {
            Vector3 eulerRotation = new Vector3(transform.eulerAngles.x + .45f, transform.eulerAngles.y, transform.eulerAngles.z);

            transform.rotation = Quaternion.Euler(eulerRotation);

        }
        else
        {
            Destroy();
        }
    }
}
