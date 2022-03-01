using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Player : MonoBehaviour
{
    public bool verbose = false;
    public bool isGrounded;


    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
        
    [SerializeField]
    float speed;

    [SerializeField]
    int jumpForce;

    [SerializeField]
    float groundCheckRadius;

    [SerializeField]
    LayerMask isGroundLayer;

    [SerializeField]
    Transform groundCheck;

    bool coroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 5.0f;
            if (verbose)
                Debug.Log("Speed changed to default value of 5");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 300;
            if (verbose)
                Debug.Log("Jump Force changed to default value of 300");
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.05f;
            if (verbose)
                Debug.Log("Ground Check Radius changed to default value of 0.05");
        }

        if (!groundCheck)
        {
            groundCheck = transform.GetChild(0);
            if (verbose)
            {
                if (groundCheck.name == "GroundCheck")
                    Debug.Log("Ground Check Found and Assigned");
                else
                    Debug.Log("Manually set ground check as it could not be found!");
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClip = anim.GetCurrentAnimatorClipInfo(0);
        float hInput = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }
        

        if (curPlayingClip[0].clip.name != "Fire")
        {
            Vector2 moveDir = new Vector2(hInput * speed, rb.velocity.y);
            rb.velocity = moveDir;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        anim.SetFloat("xVel", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);

        if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX)
            sr.flipX = !sr.flipX;
        
    }

    public void StartJumpForceChange()
    {
        if (!coroutineRunning)
            StartCoroutine("JumpForceChange");
        else
        {
            StopCoroutine("JumpForceChange");
            jumpForce /= 2;
            StartCoroutine("JumpForceChange");
        }

    }

    IEnumerator JumpForceChange()
    {
        coroutineRunning = true;
        jumpForce *= 2;

        yield return new WaitForSeconds(5.0f);

        jumpForce /= 2;
        coroutineRunning = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Squish")
        {
            col.gameObject.GetComponentInParent<EnemyWalker>().IsSquished();
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            Destroy(col.gameObject);

        }
    }
}
