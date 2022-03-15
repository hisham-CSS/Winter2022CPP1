using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Player))]
public class PlayerFire : MonoBehaviour
{
    public bool verbose = false;

    SpriteRenderer sr;
    Animator anim;
    PlayerSounds ps;

    public AudioClip fireSound;
    public AudioMixerGroup soundFXGroup;

    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public float projectileSpeed;
    public Projectile projectilePrefab;

    


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ps = GetComponent<PlayerSounds>();

        if (projectileSpeed <= 0)
            projectileSpeed = 7.0f;

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
            if (verbose)
                Debug.Log("Inspector Values Not Set");

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
            //Debug.LogError("This is to pause the game");
        }
    }

    public void FireProjectile()
    {
        if (sr.flipX)
        {
            Projectile temp = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            temp.speed = -projectileSpeed;
        }   
        else
        {
            Projectile temp = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            temp.speed = projectileSpeed;
        }

        ps.Play(fireSound, soundFXGroup);
    }

}
