using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerController : MonoBehaviour {

    private Data.Player m_Player; //get speed from here, theoretically, it returns the correct speed
    private PlatformerCharacter2D m_PlatformerCharacter;
    private Platformer2DUserControl m_UserControl;

    private void Awake()
    {
        m_UserControl = GetComponent<Platformer2DUserControl>();
        m_PlatformerCharacter = GetComponent<PlatformerCharacter2D>();
    }

    public void Init(Data.Player player){
        m_Player = player;
        m_UserControl.HorizontalBind = string.Format("P{0}_Horizontal");
        m_UserControl.JumpBind = string.Format("P{0}_Jump");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Player == null)
            return;
        
        _HandleInput();
        _UpdatePlayer();
	}

    private void _UpdatePlayer(){
        if (m_Player.hasItem)
            m_Player.life -= Time.deltaTime;
        m_PlatformerCharacter.maxSpeed = m_Player.speed;
    }

    private void _HandleInput(){

        //some specific input..

    }

}
