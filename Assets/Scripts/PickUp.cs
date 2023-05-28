using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    Health, Ammo
}

public class PickUp : MonoBehaviour
{
    public PickUpType type;
    public int value;

    public AudioClip pickUpSFX;

    [Header("Bobbing")]
    public float rotateSpeed;
    public float bobSpeed;
    public float bobHeight;


    private Vector3 startPos;
    private bool bobbingUp;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //rotate
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // bob up and down
        Vector3 offSet = (bobbingUp == true ? offSet = new Vector3(0, bobHeight / 2, 0) : new Vector3(0, -bobHeight / 2, 0));
        transform.position = Vector3.MoveTowards(transform.position, startPos + offSet, bobSpeed * Time.deltaTime);

        if (transform.position == startPos + offSet)
            bobbingUp = !bobbingUp;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch (type)
            {
                case PickUpType.Health:
                    player.GiveHealth(value);
                    break;
                case PickUpType.Ammo:
                    player.GiveAmmo(value);
                    break;
            }

            other.GetComponent<AudioSource>().PlayOneShot(pickUpSFX);

            Destroy(gameObject);
        }
    }
}
