using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int HP;
    public int maxHP;
    public int scoreToGive;

    [Header("Movement")]
    public float moveSpeed;
    public float attackRange;
    public float yPathOffset;

    
    public Slider slider;

    private List<Vector3> path;
    private Weapon weapon;
    private GameObject target;

    void Start()
    {
        // gets the component for the weapon
        weapon = GetComponent<Weapon>();
        // to be able to set the target
        target = FindObjectOfType<Player>().gameObject;
        InvokeRepeating("UpdatePath", 0.0f, 0.5f);
        
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= attackRange)
        {
            if (weapon.CanShoot())
                weapon.Shoot();

        }
        else { ChaseTarget(); }

        // look at the target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle =  Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        transform.eulerAngles = Vector3.up * angle;
    }

    void ChaseTarget()
    {
        if (path.Count == 0) return;
        // move towards the closest path
        transform.position = Vector3.MoveTowards(transform.position, path[0] + new Vector3(0, yPathOffset, 0), moveSpeed * Time.deltaTime);

        if (transform.position == path[0] + new Vector3(0, yPathOffset, 0))
        {
            path.RemoveAt(0);
        }
    }

    void UpdatePath()
    {
        //calculate a path to the target
        NavMeshPath navMeshpath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshpath);

        // save corners as a list
        path = navMeshpath.corners.ToList();
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        slider.value = (float)HP/maxHP;
        if (HP <= 0) Die();
    }

    void Die()
    {
        GameManager.instance.AddScore(scoreToGive);
        Destroy(gameObject);
    }

}
