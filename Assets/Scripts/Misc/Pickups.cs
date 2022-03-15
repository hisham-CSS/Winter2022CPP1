using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Pickups : MonoBehaviour
{
    enum CollectibleType
    {
        POWERUP,
        SCORE,
        LIFE
    }

    [SerializeField] CollectibleType curCollectible;
    [SerializeField] AudioClip pickupSound;
    public AudioMixerGroup soundFXGroup;

    AudioSource myAudioSource;
    public int ScoreValue;

    private void Start()
    {
        if (!myAudioSource)
            myAudioSource = GetComponent<AudioSource>();

        if (curCollectible == CollectibleType.LIFE)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-3, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerSounds ps = collision.gameObject.GetComponent<PlayerSounds>();
            ps.Play(pickupSound, soundFXGroup);

            switch (curCollectible)
            {
                case CollectibleType.POWERUP:
                    collision.gameObject.GetComponent<Player>().StartJumpForceChange();
                    //curPlayerScript.score += ScoreValue;
                    GameManager.instance.score += ScoreValue;
                    break;
                case CollectibleType.LIFE:
                    //curPlayerScript.lives++;
                    //curPlayerScript.score += ScoreValue;
                    GameManager.instance.lives++;
                    break;
                case CollectibleType.SCORE:
                    GameManager.instance.score += ScoreValue;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
