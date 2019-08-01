using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayer : MonoBehaviour {
    [SerializeField]
    private GameObject player1Control;
    [SerializeField]
    private GameObject player2Control;
    [SerializeField]
    private TriangleStatus TPS;
    [SerializeField]
    private BoxStatus BPS;
    // Use this for initialization
    void Start () {
        TogglePlayer1(true);
        TogglePlayer2(false);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            if (player1Control.activeSelf)
            {
                TogglePlayer1(false);
                TogglePlayer2(true);

            }
            else
            {
                TogglePlayer1(true);
                TogglePlayer2(false);

            }
        }
	}
    void TogglePlayer1(bool tog)
    {
        BPS.Freeze(!tog);
        player1Control.SetActive(tog);
    }
    void TogglePlayer2(bool tog)
    {
        TPS.Freeze(!tog);
        player2Control.SetActive(tog);
        
    }

}
