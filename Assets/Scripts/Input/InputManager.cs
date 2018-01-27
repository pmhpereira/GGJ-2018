using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public struct PlayerController{
        public int playerIndex;
        public string controlerId;
    }

    public string[] Inputs { get { return m_Inputs; }}
    private string[] m_Inputs;
    
    public enum State { Waiting, Mapping, Done }
    public State currState = State.Waiting;
    public State prevState = State.Waiting;

    public bool p1 = false;
    public bool p2 = false;



	// Use this for initialization
	void Start () {
        
        m_Inputs = Input.GetJoystickNames();
        if (m_Inputs.Length == 0)
            Debug.LogWarning("No Controllers connected");
        else{
            foreach (var i in m_Inputs)
            {
                Debug.Log(i);
            }
            _ChangeState(State.Mapping);
            StartCoroutine(WaitForInput());
        }

	}

    IEnumerator WaitForInput(){
        Debug.Log("WAITING FOR INPUT!");

        while(true){
            if(Input.GetButton("X360_A")){
                Debug.Log("I GOT THAT!");
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(currState == State.Done && prevState == State.Mapping){

            if (Input.GetButton("j1Jump")){
                Debug.Log("Player 1 jumped");
            }

        }
        else if(currState == State.Mapping && prevState == State.Waiting){
        }
	}

    private void _ChangeState(State newState){
        prevState = currState;
        currState = newState;
    }
}
