using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int HP;
    public int MaxHP;

    [Header("Movement")]
    public float moveSpeed;     // movement speed in units per second
    public float jumpForce;     // force applied upwards

    [Header("Camera")]
    public float lookSensitivity;   // mouse look sensitivity
    public float maxLookX;          // highest up we can look
    public float minLookX;          // lowest down we can look
    private float rotX;              // current x rotation of the camera

    private Camera cam;
    private Rigidbody rig;
    private Weapon weapon;

    void Awake()
    {
        // get the components
        cam = Camera.main;
        rig = GetComponent<Rigidbody>();
        weapon = GetComponent<Weapon>();

        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Start()
    {
        GameUi.instance.UpdateHealthBar(HP, MaxHP);
        GameUi.instance.UpdateScoreText(0);
        GameUi.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
    }

    void Update()
    {
        //don't do anything if paused
        if (GameManager.instance.gamePaused == true)
            return;

        Move();
        if (Input.GetButtonDown("Jump"))
            TryJump();
        if (Input.GetButton("Fire1"))
        {
            if (weapon.CanShoot())
                weapon.Shoot();
        }
        CamLook();
    }

  

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 dir = transform.right * x + transform.forward * z;
        dir.y = rig.velocity.y;
        rig.velocity = dir;
    }

    void CamLook()
    {
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        transform.eulerAngles += Vector3.up * y;

    }

    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 1.1f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        GameUi.instance.UpdateHealthBar(HP, MaxHP);

        if (HP <= 0)
            Die();
    }

    void Die()
    {
        GameManager.instance.LoseGame();
    }

    public void GiveHealth(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);

        GameUi.instance.UpdateHealthBar(HP, MaxHP);
        
    }

    public void GiveAmmo(int amount) {
        weapon.curAmmo = Mathf.Clamp(weapon.curAmmo + amount, 0, weapon.maxAmmo);
        GameUi.instance.UpdateAmmoText(weapon.curAmmo, weapon.maxAmmo);
    }

}
