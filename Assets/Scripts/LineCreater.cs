using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreater : MonoBehaviour {
    public GameObject chalk;
    public GameObject brush;
    [SerializeField]
    public float Fluidslimit;
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
  
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(checkX, checkY), 0, whatIsGround);
        if (isGrounded)
        {
            activeLine1 = null;
            activeLine2 = null;
        }
        if (!isGrounded)
        {
            ///Fluid Limit reached
            if (Fluidslimit >= 0)
            {
                //NormalLine
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject lineGO = Instantiate(linePrefab);
                    activeLine1 = lineGO.GetComponent<Line>();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    activeLine1 = null;
                }
                if (activeLine1 != null)
                {
                    activeLine1.UpdateLine(brush.transform.position);
                    Fluidslimit -= Time.deltaTime;
                }
                //BounceLine();
            }
        }
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
