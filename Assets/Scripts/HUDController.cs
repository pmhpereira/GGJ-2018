using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    [SerializeField]
    private Slider player1Life;
    [SerializeField]
    private Slider player2Life;

    private Data.Player[] m_Players;

    public void Init(float maxLife, Data.Player[] players){
        m_Players = players;
        player1Life.maxValue = player2Life.maxValue = maxLife;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(m_Players != null){

            foreach(var p in m_Players){

                if (p.playerIndex == 0)
                    player1Life.value = p.life;
                else
                    player2Life.value = p.life;
            }
        }

	}
}
