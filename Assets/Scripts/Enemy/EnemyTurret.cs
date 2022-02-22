using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] float projectileForce;
    [SerializeField] float projectileFireRate;

    float timeSinceLastFire;

    public Transform projectileSpawnPointRight;
    public Transform projectileSpawnPointLeft;

    public Projectile projectilePrefab;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (projectileForce <= 0)
            projectileForce = 7.0f;

        if (projectileFireRate <= 0)
            projectileFireRate = 2.0f;

        if (!projectilePrefab)
        {
            if (verbose)
                Debug.Log("Projectile Prefab has not be set on " + name);
        }
        if (!projectileSpawnPointRight)
        {
            if (verbose)
                Debug.Log("Projectile Spawn Point Right has not be set on " + name);
        }
        if (!projectileSpawnPointLeft)
        {
            if (verbose)
                Debug.Log("Projectile Spawn Point Left has not be set on " + name);
        }
    }

    public override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetBool("Fire"))
        {
            //HINT = THIS IS WHERE YOU WOULD CHECK DIRECTION/DISTANCE TO TARGET/ETC.
            if (Time.time >= timeSinceLastFire + projectileFireRate)
            {
                anim.SetBool("Fire", true);
            }
                
        }
        
    }

    public void Fire()
    {
        timeSinceLastFire = Time.time;

        Projectile temp = Instantiate(projectilePrefab, projectileSpawnPointRight.position, projectileSpawnPointRight.rotation);
        temp.speed = projectileForce;
    }

    public void ReturnToIdle()
    {
        anim.SetBool("Fire", false);
    }
}
