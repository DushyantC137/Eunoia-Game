using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using EZCameraShake;

public class PlayerControl2 : MonoBehaviour
{

    //extra
    private bool jumpRequest;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    private float moveV;

    public GameObject player;
    public float speed;
    private float moveInputHorrizontal;
    private float moveInputVertical;
    public float jumpForce;
    private Rigidbody2D rb;

    [SerializeField]
    public bool isGrounded;
    public Transform groundCheck;
    public float checkX, checkY;
    public LayerMask whatIsGround;
    // private int extraJump;
    // public int extrajumpinput;
    private float yspeed;
    public bool moving;
    public Animator animP;
    public bool isDrop;


    [SerializeField]
    private AudioSource source;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    private Collider2D groundObjectCollider;
    private bool isIcing;


    // Use this for initialization
    void Start()
    {

        moving = true;
        rb = player.GetComponent<Rigidbody2D>();
        
        //extraJump = extrajumpinput;
    }

    void Update()
    {

        if (isGrounded)
        {
            //extraJump = extrajumpinput;
            if (isDrop)
            {
                
                if (!isIcing)
                {
                     source.clip = landingSound;
                     source.Play();
                }
                
                ///animP.SetTrigger("drop");
              //  float shakeImpulse = Mathf.Min((Mathf.Abs(yspeed) / 12f), 8f);
               // CameraShaker.Instance.ShakeOnce(shakeImpulse, 7f, 0.2f, 0.4f);////////////////////////////////////////////////////
                isDrop = false;
            }
        }
        else
            isDrop = true;
        //Is is moving
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            moving = true;
        }
        else
            moving = false;
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            ///animP.SetTrigger("jump");
            jumpRequest = true;
           // extraJump--;
        }
        //Here WE CHECK IF IT IS ON GROUND OR NOT
        groundObjectCollider = Physics2D.OverlapBox(groundCheck.position, new Vector2(checkX, checkY), 0, whatIsGround);
        if (groundObjectCollider)
        {
            isGrounded = true;
            if (groundObjectCollider.gameObject.layer == 8) isIcing = true;
            else isIcing = false;
        }
        else { isIcing = false; isGrounded = false; }

        //Down
        if (!isGrounded)
        {
            moveInputVertical = Input.GetAxis("Vertical");
            //Fast Drop--
            if (moveInputVertical < 0)
            {
               // rb.velocity = Vector2.down * jumpForce * 1.4f;
                
            }
        }
        //Left Right movement
        moveInputHorrizontal = Input.GetAxis("Horizontal");
        moveV = moveInputHorrizontal * speed * Time.deltaTime;

        
    }

    void FixedUpdate()
    {
        yspeed = rb.velocity.y;
        Vector3 targetVelocity = new Vector2(moveV * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        //Jump
        if (jumpRequest)
        {
            if (!isIcing)
            {
                source.clip = jumpSound;
                source.Play();
            }
            if (yspeed < 0.1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (yspeed > 12f)
            {
                rb.velocity = new Vector2(0.6f * rb.velocity.y, rb.velocity.x);
            }
            jumpRequest = false;
        }

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkX, checkY, 1));
    }
 
}
