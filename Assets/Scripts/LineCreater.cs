using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineCreater : MonoBehaviour {
    public GameObject chalk;
    public GameObject brush;
    [SerializeField]
    private float MaxFluidslimit;
    [SerializeField]
    private float Fluidslimit;
    public GameObject linePrefab;
    public GameObject BouncelinePrefab;
    Line activeLine1;
    Line activeLine2;
    Rigidbody2D rb;
    [SerializeField]
    private bool isGrounded;
    public Transform groundCheck;
    public float checkX, checkY;
    public LayerMask whatIsGround;
    private GameObject lineGO;
    private bool isIcing;
    [SerializeField]private float decreaseFluidRate=2f;
    [SerializeField]private float increaseFluidRate=1f;
    [Header("Unity Stuff")]
    public Image fluidsBar;
    // Update is called once per frame
    public void Start()
    {
        Fluidslimit = MaxFluidslimit;
    }
    void Update()
    {
        fluidsBar.fillAmount = Fluidslimit / MaxFluidslimit;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(checkX, checkY), 0, whatIsGround);
        if (isGrounded)
        {
            activeLine1 = null;
            activeLine2 = null;
        }
        if (!isIcing)
        {
            IncreaseRateOfFluid();
        }
        if (!isGrounded)
        {
            ///Fluid Limit reached
            ///
           // Debug.Log(Fluidslimit);
            if (Fluidslimit >= 0)
            {
                //NormalLine
                if (Input.GetMouseButtonDown(0))
                {
                    lineGO = Instantiate(linePrefab);
                    activeLine1 = lineGO.GetComponent<Line>();
                    isIcing = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("yo");
                    activeLine1 = null;
                    isIcing = false;

                }
                if (activeLine1 != null)
                {

                    activeLine1.UpdateLine(brush.transform.position);
                    Fluidslimit -= decreaseFluidRate*Time.deltaTime;
                }

                //BounceLine();
            }
            else
            {
                activeLine1 = null;
                isIcing = false;
            }
        }
        else isIcing = false;
    }
    void IncreaseRateOfFluid()
    {
        Fluidslimit += increaseFluidRate*Time.deltaTime;
        Fluidslimit = Mathf.Clamp(Fluidslimit, 0f, MaxFluidslimit);
    }
    void BounceLine()
    {
        //Bounce
        if (Input.GetMouseButtonDown(1))
        {
            GameObject lineGO = Instantiate(BouncelinePrefab);
            activeLine2 = lineGO.GetComponent<Line>();
        }
        if (Input.GetMouseButtonUp(1))
        {
            activeLine2 = null;
        }
        if (activeLine2 != null)
        {
            activeLine2.UpdateLine(brush.transform.position);
            Fluidslimit -= Time.deltaTime;
        }
    }
    //GizmosForGroundCheck
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(checkX, checkY, 1));
    }

}
