using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    public enum GameState { Start }

    [SerializeField]
    private PlayerController m_PlayerPrefab;

    private Data.Player[] m_Players = new Data.Player[2]; //well, were only gonna hasve 2 :p

	// Use this for initialization
	void Start () {

        Data.Player p1 = new Data.Player(0, 1f, 5f);
        m_Players[0] = p1;
        Data.Player p2 = new Data.Player(1, 1f, 5f);
        m_Players[1] = p2;

        var player1gameObject = Instantiate(m_PlayerPrefab) as PlayerController;
        player1gameObject.Init(p1);
        var player2gameObject = Instantiate(m_PlayerPrefab) as PlayerController;
        player1gameObject.Init(p2);

	}
	
	// Update is called once per frame
	void Update () {
	}
}
