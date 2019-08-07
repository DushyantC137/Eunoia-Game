using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level1Design : MonoBehaviour {
    [SerializeField] private GameObject DestParticles;
    [SerializeField] private Vector3[] Nextpositions;
    [SerializeField]private float checkX, checkY;
    public LayerMask whatIsPlayer;
    private bool isPlayer;
    private int x = 1;
    private bool s;
    private ParticleSystem ps;
    private GameObject tempParticle;
    [SerializeField] private GameObject playerControl;
    [SerializeField] private TextMeshProUGUI helptxt;
  //  private Text ;
    // Use this for initialization
    void Start () {
        isPlayer = false;
        x = 1;
        s = true;
       // helptxt = gameObject.GetComponent<TextMeshProUGUI>();
        if (Nextpositions.Length > 0)
        {
            tempParticle = Instantiate(DestParticles, Nextpositions[0], Quaternion.identity);
            SetHelpTxt(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
     
        SetHelpTxt(x);
        isPlayer = Physics2D.OverlapBox(Nextpositions[x-1], new Vector2(checkX, checkY), 0, whatIsPlayer);        
        if (isPlayer && s && x<Nextpositions.Length)
        {
            
            if (tempParticle)
            {
                ps = tempParticle.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.loop = false;
                Destroy(tempParticle, 2f);
            }
            tempParticle = Instantiate(DestParticles, Nextpositions[x], Quaternion.identity);
            x++;
           s = false;
        }
       else
       {
           s = true;
       }
        

    }
    void SetHelpTxt(int n)
    {
        if (n == 0)
        {
            helptxt.text = "Hello Human, Press Space to jump";
        }
        if (n == 1)
        {
            helptxt.text = "Tap space in air to double jump";
        }
        if (n == 2)
        {
            helptxt.text = "Hold down key and press space to dash on objects";
        }
        if (n == 3)
        {
            helptxt.text = "Yes Human! This cube has magical powers...\n hold mouse1 to check";
        }
        else 
        {
            helptxt.text = "Hoomans shud be cannibals";
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(Nextpositions.Length>0)
            Gizmos.DrawWireCube(Nextpositions[0], new Vector3(checkX, checkY, 1));
    }
}
