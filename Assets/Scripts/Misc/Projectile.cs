using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public int damageValue;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
            lifetime = 2.0f;

        if (damageValue <= 0)
            damageValue = 2;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (rb.velocity.x != speed)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (gameObject.tag == "PlayerProjectile")
            {
                Enemy e = col.gameObject.GetComponent<Enemy>();

                if (e)
                    e.TakeDamage(damageValue);

                Destroy(gameObject);
            }
        }
    }
}
