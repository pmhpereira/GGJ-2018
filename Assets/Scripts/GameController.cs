﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{

    public enum GameState { None, Start, Playing, Paused, GameOver }
    public GameState gameState { get { return m_CurrentGameState; }}
    private GameState m_CurrentGameState = GameState.None;
    private GameState m_PreviousGameState = GameState.None;

    private static GameController s_Instance;
    public static GameController Instance { get { return s_Instance; } }

    [Header("Player Maps")]
    [SerializeField]
    private GameObject m_Player1Map;
    [SerializeField]
    private GameObject m_Player2Map;

    [Header("Cameras")]
    [SerializeField]
    private Camera2DFollow m_P1Cam;
    [SerializeField]
    private Camera2DFollow m_P2Cam;

    [Header("PlayerSettings")]
    public float playerSpeed = 1f;
    public float levelTime = 120f;

    [Header("References")]
    [SerializeField]
    private Transform m_MainMenu;
    [SerializeField]
    private Transform m_PauseMenu;
    [SerializeField]
    private HUDController m_Hud;
    [SerializeField]
    private PlayerController m_Player1Prefab;
    [SerializeField]
    private PlayerController m_Player2Prefab;
    [SerializeField]
    private Data.LevelPair[] m_LevelsPrefabs;

    private int m_CurrentLevelIndex = 0;
    private Data.LevelPair m_CurrentLevel;
    private Data.Player[] m_Players = new Data.Player[2]; //well, were only gonna hasve 2 :p
    private PlayerController[] m_PlayerGameObjects = new PlayerController[2];

    private void Awake()
    {
        s_Instance = this;

    }

    // Use this for initialization
    void Start()
    {
        _Init();
        //change this to on start game click?
        _ChangeState(GameState.Start);
    }

    private void _Init(){
        //instantiate first level?
        m_CurrentLevelIndex = 0;
        LoadLevel(m_CurrentLevelIndex);

        float playerLife = levelTime / 2f;

        //instantiate players
        _InitPlayers(playerLife);

        //load hud
        m_Hud.Init(playerLife, m_Players);
    }

    private void _InitPlayers(float playerLife)
    {

        Data.Player p1 = new Data.Player(0, playerLife, playerSpeed);
        m_Players[0] = p1;
        Data.Player p2 = new Data.Player(1, playerLife, playerSpeed);
        m_Players[1] = p2;

        var player1gameObject = Instantiate(m_Player1Prefab, m_Player1Map.transform) as PlayerController;
        player1gameObject.Init(p1);
        var player2gameObject = Instantiate(m_Player2Prefab, m_Player2Map.transform) as PlayerController;
        player2gameObject.Init(p2);

        m_PlayerGameObjects[0] = player1gameObject;
        m_PlayerGameObjects[1] = player2gameObject;

        m_P1Cam.target = player1gameObject.transform;
        m_P2Cam.target = player2gameObject.transform;

    }

    public void LoadLevel(int levelIndex, bool force = false)
    {
        if (m_CurrentLevelIndex != levelIndex)
        {
            m_CurrentLevel.DestroyLevels();
        }
        var newLevelsPair = m_LevelsPrefabs[m_CurrentLevelIndex];
        var p1LevelInstance = Instantiate(newLevelsPair.p1Level, m_Player1Map.transform);
        var p2LevelInstance = Instantiate(newLevelsPair.p2Level, m_Player2Map.transform);
        m_CurrentLevel = new Data.LevelPair();
        m_CurrentLevel.p1Level = p1LevelInstance;
        m_CurrentLevel.p2Level = p2LevelInstance;

        m_CurrentLevel.p1Level.transform.localPosition = Vector3.one * -10f;
        m_CurrentLevel.p2Level.transform.localPosition = Vector3.one * -10f;
    }

    private void _ResetLevel()
    {
        m_CurrentLevelIndex = 0;
        LoadLevel(m_CurrentLevelIndex);
    }

    // Update is called once per frame
    void Update()
    {

        if (m_CurrentGameState != m_PreviousGameState)
        {
            m_PreviousGameState = m_CurrentGameState;
            //handle game state changes here
            switch (m_CurrentGameState)
            {
                case GameState.Start:
                    // Game is loaded, showing start screen
                    Debug.Log("Game has started");

                    _SetHudState(false);
                    _SetMainMenuState(true);
                    Time.timeScale = 0f;

                    break;
                case GameState.Playing:
                    // setup right before  playing
                    _SetHudState(true);
                    _SetMainMenuState(false);
                    Time.timeScale = 1f;

                    break;
                case GameState.Paused:
                    m_PauseMenu.gameObject.SetActive(true);
                    Time.timeScale = 0f;
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


        if (Input.GetKeyDown(KeyCode.P))
            OnPause();


    }

    private void _SetHudState(bool state)
    {
        m_Hud.gameObject.SetActive(state);
    }

    private void _SetMainMenuState(bool state)
    {
        //load start menu?
        if (m_MainMenu != null)
            m_MainMenu.gameObject.SetActive(state);
    }

    private void _CheckGameOver()
    {
        foreach (var player in m_Players)
        {
            if (player.life <= 0f)
                _ChangeState(GameState.GameOver);
        }
    }

    private void _ChangeState(GameState newState)
    {
        m_PreviousGameState = m_CurrentGameState;
        m_CurrentGameState = newState;
    }

    public void OnNewGame()
    {
        _ChangeState(GameState.Playing);
    }

    public void OnResume(){
        if (gameState == GameState.Paused){
            _ChangeState(GameState.Playing);
            m_PauseMenu.gameObject.SetActive(false);
        }
    }

    public void OnGoToMainMenu(){
        _Cleanup();
        _Init();
        m_PauseMenu.gameObject.SetActive(false);
        _ChangeState(GameState.Start);
    }

    public void OnPause(){
        if (gameState == GameState.Playing)
            _ChangeState(GameState.Paused);
    }

    private void _Cleanup(){
        if(m_PlayerGameObjects != null){
            foreach (var p in m_PlayerGameObjects)
                Destroy(p.gameObject);
        }
        if(m_CurrentLevel != null){
            Destroy(m_CurrentLevel.p1Level.gameObject);
            Destroy(m_CurrentLevel.p2Level.gameObject);
        }
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }

}
