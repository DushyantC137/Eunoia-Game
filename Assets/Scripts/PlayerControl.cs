using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerControl : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float moveInputHorrizontal;
    private float moveInputVertical;
    public float jumpForce;

    //minor Detail
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private bool inAir;
    [Range(0, 3f)] [SerializeField] private float fallingGravity;
    private float startGravity;
   // private bool isdoubleJump = false;
    private bool jumpRequest = false;
    [SerializeField] private float maxJumpForce;
    private float moveV;
    private float yspeed;
    private bool facingRight;


    private Rigidbody2D rb;
    [SerializeField]
    public bool isGrounded;
    public Transform groundCheck;
    public float checkX, checkY;
    public LayerMask whatIsGround;
    private Collider2D groundObjectCollider;
    private int extraJump;
    public int extrajumpinput;

    private int extraDash;
    [SerializeField]private int extraDashInput=1;

    public bool moving;
    public Animator animP;
    public bool isDrop;


    [SerializeField]
    private AudioSource source;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    //trail
    public GameObject jumpspark;
    [SerializeField]
    private GameObject trailJump;
    private boxtrail trailscript;

    //check
    public bool isSparkEnable = false;
    public bool isTrailEnable = false;
    [SerializeField] private float extraForce = 0.42f;

    //Dash
    [SerializeField] private float dashForce;
    [SerializeField] private float startDashTime;
    private float dashTime;
    private bool isDashing = false;
    private bool dashRequest;
    [SerializeField]private float restTimeForDashInput;
    private float restTimeForDash;

    //icing
    bool isIcing;

    // Use this for initialization
    void Start()
    {
        facingRight = true;
        moving = true;
        rb = player.GetComponent<Rigidbody2D>();
        extraJump = extrajumpinput;
        extraDash = extraDashInput;
        startGravity = rb.gravityScale;
        trailscript = trailJump.GetComponent<boxtrail>();
        trailscript.enabled = false;
        dashTime = startDashTime;
        restTimeForDash = restTimeForDashInput;
    }

    void Update()
    {

        //AirControls
        inAir = !isGrounded;
        if (inAir)
        {
            //Falling
            if (yspeed < 0.5f)
            {
                rb.gravityScale = startGravity + fallingGravity;
            }

        }
        else
        {
            rb.gravityScale = startGravity;
        }
        
        //Dropping Effects
        DroppingEffects();


        //Is is moving
        if (rb.velocity.x != 0 || yspeed != 0)
        {
            moving = true;
        }
        else
            moving = false;

        //NormalJump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (extraJump > 0)
            {
                jumpRequest = true;
            }
            animP.SetTrigger("jump");
            //Debug.Log("Jump");
            
        }
        //IS THE OBJECT GROUNDED
        groundObjectCollider = Physics2D.OverlapBox(groundCheck.position, new Vector2(checkX, checkY), 0, whatIsGround);
        if (groundObjectCollider)
        {
            isGrounded = true;
            if (groundObjectCollider.gameObject.layer == 8) isIcing = true;
            else isIcing = false;
        }
        else { isIcing = false; isGrounded = false; }
        // Debug.Log(isIcing);
        
        //Left Right movement
        moveInputHorrizontal = Input.GetAxis("Horizontal");
        moveV = moveInputHorrizontal * speed * Time.deltaTime;
        //Facing Right
       if(!facingRight && moveInputHorrizontal > 0)
        {
            Flip();
        }
        else
        {
            if (facingRight && moveInputHorrizontal < 0)
                Flip();
        }

        moveInputVertical = Input.GetAxis("Vertical");
        //Down/Jump////Dash>
        //DashRequest
        

        if (restTimeForDash <= 0)
        {
            if (moveInputVertical < 0 && Input.GetKeyDown(KeyCode.Space) && extraDash > 0)
            {
                
                dashRequest = true;
                restTimeForDash = restTimeForDashInput;
            }
            
        }
        else
        {
            restTimeForDash -= Time.deltaTime;
        }
    
  
       
    }


    //Dealing with physics
    void FixedUpdate()
    {
        yspeed = rb.velocity.y;
        //LeftRight Movement
        
        Vector3 targetVelocity = new Vector2(moveV * 10f, rb.velocity.y);
       
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        
        //NormalJump
        if (jumpRequest && isGrounded &&!isDashing)
        {
            if (!isIcing)
            {
                source.clip = jumpSound;
                source.Play();
            }

            if (yspeed < 0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce * 1.1f, ForceMode2D.Impulse);
            }else
                rb.AddForce( Vector2.up * jumpForce,ForceMode2D.Impulse);
            jumpRequest = false;
        }

        //Extra Jump
        if (jumpRequest && extraJump > 0 && inAir && !isDashing)
        {
            source.clip = jumpSound;
            source.Play();
            //Debug.Log(yspeed);
            if (isTrailEnable)
                trailscript.enabled = true;
         //   isdoubleJump = true;
            if (isSparkEnable)
            {
                GameObject jumpSparkT = Instantiate(jumpspark, player.transform.position, Quaternion.identity);
                Destroy(jumpSparkT, 1f);
            }
            
            
            if (yspeed > maxJumpForce)
                rb.AddForce(Vector2.up * jumpForce * extraForce, ForceMode2D.Impulse);
            else
            {
                if (yspeed < 0.4f && yspeed > -0.3f&& inAir)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * jumpForce * 1.3f, ForceMode2D.Impulse);
                }
                if (yspeed > 0.4f)
                {
                    rb.AddForce(Vector2.up * jumpForce * 1.3f, ForceMode2D.Impulse);
                    
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * jumpForce * 1.3f, ForceMode2D.Impulse);
                }
                
            }
                extraJump--;
            jumpRequest = false;
        }
        //DashRequest
        if (dashRequest)
        {
            if (dashTime <= 0)
            {
                rb.velocity = new Vector2(0,Mathf.Min(yspeed,12f));
                dashTime = startDashTime;
                isDashing = false;
                trailscript.enabled = false;
                dashRequest = false;
                extraDash--;
            }
            else
            {
                trailscript.enabled = true;
                dashTime -= Time.fixedDeltaTime ;
                isDashing = true;
                if(facingRight)
                    rb.velocity = Vector2.right * dashForce;
                else
                    rb.velocity = Vector2.left * dashForce;

            }
        }
    }

  

   void DroppingEffects()
    {
        if (isGrounded)
        {
            extraJump = extrajumpinput;
            
             extraDash = extraDashInput;
          //  isdoubleJump = false;
            trailscript.enabled = false;

            if (isDrop)
            {
                if (!isIcing)
                {
                    source.clip = landingSound;
                    source.Play();
                }

               // Debug.Log("drop");
                
                float shakeImpulse = Mathf.Min((Mathf.Abs(yspeed) / 10f), 4f);
               // Debug.Log(yspeed);
                if(yspeed==0)
                    CameraShaker.Instance.ShakeOnce(4f, 7f, 0.2f, 0.4f);
                else
                CameraShaker.Instance.ShakeOnce(shakeImpulse, 7f, 0.2f, 0.4f);
                if (yspeed < -6f)
                {
                    animP.SetTrigger("hardDrop");
                }
                else
                    animP.SetTrigger("drop");
                isDrop = false;
            }
        }
        else
            isDrop = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkX, checkY, 1));
    }
   
}
