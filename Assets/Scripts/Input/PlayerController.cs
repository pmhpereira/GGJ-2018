using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private Data.Player m_Player; //get speed from here, theoretically, it returns the correct speed

    public void Init(Data.Player player){
        m_Player = player;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Player == null)
            return;
        
        _HandleInput();
	}

    private void _DebugInput(KeyCode k){
        Debug.LogFormat("Player {0} pressed {1}", k.ToString());   
    }

    private void _HandleInput(){
        if (m_Player.playerIndex == 0)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _DebugInput(KeyCode.LeftArrow);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _DebugInput(KeyCode.RightArrow);

            }
            if (Input.GetKey(KeyCode.A))
            {
                _DebugInput(KeyCode.A);

            }
            if (Input.GetKey(KeyCode.S))
            {
                _DebugInput(KeyCode.S);

            }
        }
        else if (m_Player.playerIndex == 1)
        {

            //BUTTONS
            if (Input.GetButtonDown("PS4_X"))
            {
                print("X");
            }

            if (Input.GetButtonDown("PS4_O"))
            {
                print("O");
            }

            if (Input.GetButtonDown("PS4_Triangle"))
            {
                print("Triangle");
            }

            if (Input.GetButtonDown("PS4_Square"))
            {
                print("Square");
            }

            //LEFT STICK
            if (Input.GetAxis("PS4_L_Horizontal") > 0.1)
            {
                print("Left Stick Right");
            }

            if (Input.GetAxis("PS4_L_Horizontal") < -0.1)
            {
                print("Left Stick Left");
            }

            if (Input.GetAxis("PS4_L_Vertical") < -0.1)
            {
                print("Left Stick Up");
            }

            if (Input.GetAxis("PS4_L_Vertical") > 0.1)
            {
                print("Left Stick Down");
            }

            //D-PAD
            if (Input.GetAxis("PS4_D_Y") == 1)
            {
                print("D-Pad Up");
            }

            if (Input.GetAxis("PS4_D_Y") == -1)
            {
                print("D-Pad Down");
            }
            if (Input.GetAxis("PS4_D_X") == 1)
            {
                print("D-Pad Right");
            }

            if (Input.GetAxis("PS4_D_X") == -1)
            {
                print("D-Pad Left");
            }

        }
    }

}
