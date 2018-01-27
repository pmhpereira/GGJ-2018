using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    public enum GameState { None, Start, Playing, GameOver }
    private GameState m_CurrentGameState = GameState.None;
    private GameState m_PreviousGameState = GameState.None;

    private static GameController s_Instance;
    public static GameController Instance { get { return s_Instance; }}

    public float playerSpeed = 5f;
    public float levelTime = 120f;

    [SerializeField]
    private PlayerController m_PlayerPrefab;

    private Data.Player[] m_Players = new Data.Player[2]; //well, were only gonna hasve 2 :p

    private void Awake()
    {
        s_Instance = this;
    }

    // Use this for initialization
    void Start () {

        Data.Player p1 = new Data.Player(0, levelTime / 2f, playerSpeed);
        m_Players[0] = p1;
        Data.Player p2 = new Data.Player(1, levelTime / 2f, playerSpeed);
        m_Players[1] = p2;

        //instantiate first level?

        var player1gameObject = Instantiate(m_PlayerPrefab) as PlayerController;
        player1gameObject.Init(p1);
        var player2gameObject = Instantiate(m_PlayerPrefab) as PlayerController;
        player2gameObject.Init(p2);

        //load menu?


        //change this to on start game click?
        _ChangeState(GameState.Start);

	}
	
	// Update is called once per frame
	void Update () {

        if(m_CurrentGameState != m_PreviousGameState){
            m_PreviousGameState = m_CurrentGameState;
            //handle game state changes here
            switch (m_CurrentGameState)
            {
                case GameState.Start:

                    // on start event
                    Debug.Log("Game has started");

                    //show ready message??
                    _ChangeState(GameState.Playing);

                    break;
                case GameState.Playing:

                    // setup right before  playing

                    break;
                case GameState.GameOver:

                    // on game over
                    Debug.Log("Game Over!");

                    break;
                default:
                    break;
            }
        }

        switch (m_CurrentGameState)
        {
            case GameState.Playing:

                // setup right before  playing
                Debug.LogFormat("Player 1 life: {0}; Player 2 life: {1}", m_Players[0].life, m_Players[1].life);
                _CheckGameOver();

                break;
            default:
                break;
        }


	}

    private void _CheckGameOver(){
        foreach (var player in m_Players)
        {
            if (player.life <= 0f)
                _ChangeState(GameState.GameOver);
        }
    }

    private void _ChangeState(GameState newState){
        m_PreviousGameState = m_CurrentGameState;
        m_CurrentGameState = newState;
    }
}
