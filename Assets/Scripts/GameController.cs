using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public enum GameState { None, Start, Playing, Paused, GameOver, Finished }
    public GameState gameState { get { return m_CurrentGameState; }}
    private GameState m_CurrentGameState = GameState.None;
    private GameState m_PreviousGameState = GameState.None;

    private static GameController s_Instance;
    public static GameController Instance { get { return s_Instance; } }

    [Header("Player Containers")]
    [SerializeField]
    private GameObject m_Player1Container;
    [SerializeField]
    private GameObject m_Player2Container;

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
    private Transform m_GameOverMenu;
    [SerializeField]
    private Transform m_GameFinishedMenu;
    [SerializeField]
    private HUDController m_Hud;
    [SerializeField]
    private PlayerController m_Player1Prefab;
    [SerializeField]
    private PlayerController m_Player2Prefab;
    [SerializeField]
    private Data.LevelPair[] m_LevelsPrefabs;

    private Transform m_LastMenu;

    private int m_CurrentLevelIndex = 0;
    private Data.LevelPair m_CurrentLevel;
    private Data.Player[] m_Players = new Data.Player[2]; //well, were only gonna have 2 :p
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

    private void _SetMenuState(Transform menu, bool state){
        menu.gameObject.SetActive(state);
        m_LastMenu = state ? menu : null;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks state transitions
        if (m_CurrentGameState != m_PreviousGameState)
        {
            m_PreviousGameState = m_CurrentGameState;
            //handle game state changes here
            switch (m_CurrentGameState)
            {
                case GameState.Start:
                    // Game is loaded, showing start screen
                    Debug.Log("Game has started");

                    m_Hud.gameObject.SetActive(false);
                    _SetMenuState(m_MainMenu, true);
                    Time.timeScale = 0f;

                    AudioManager.Instance.PlayBGM("menu", true, true);

                    break;
                case GameState.Playing:
                    // setup right before  playing
                    m_Hud.gameObject.SetActive(true);
                    _SetMenuState(m_MainMenu, false);
                    Time.timeScale = 1f;

                    AudioManager.Instance.PlayLevelStart();

                    break;
                case GameState.Paused:
                    _SetMenuState(m_PauseMenu, true);
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    // on game over
                    Debug.Log("Game Over!");
                    _SetMenuState(m_GameOverMenu, true);
                    Time.timeScale = 0f;

                    break;
                case GameState.Finished:
                    // on game over
                    Debug.Log("Game finished!");
                    _SetMenuState(m_GameFinishedMenu, true);
                    Time.timeScale = 0f;

                    break;
                default:
                    break;
            }
        }

        // Checks game over / finish conditions
        switch (m_CurrentGameState)
        {
            case GameState.Playing:

                // setup right before  playing
                //Debug.LogFormat("Player 1 life: {0}; Player 2 life: {1}", m_Players[0].life, m_Players[1].life);
                _CheckGameOver();
                _CheckTrade();

                break;
            default:
                break;
        }


        if (Input.GetKeyDown(KeyCode.P))
            OnPause();
        if (Input.GetKeyDown(KeyCode.L))
            _ChangeState(GameState.GameOver);

        if (Input.GetKeyDown(KeyCode.K))
            _ChangeState(GameState.Finished);
        if (Input.GetKeyDown(KeyCode.T))
            TransferIdol();
    }

    private void _CheckTrade()
    {
        Data.Player p1 = m_Players[0];
        Data.Player p2 = m_Players[1];
        if(p1.wantsSwitch && p2.wantsSwitch){
            
   //         p2.hasItem = !p2.hasItem;
   //         p1.hasItem = !p1.hasItem;
			//p1.wantsSwitch = false;
			//p2.wantsSwitch = false;
            TransferIdol();
		}
    }

    private void _Init()
    {
        //instantiate first level?
        m_CurrentLevelIndex = 0;
        _LoadLevel(m_CurrentLevelIndex);

        float playerLife = levelTime / 2f;

        //instantiate players
        _InitPlayers(playerLife);

        //load hud
        m_Hud.Init(playerLife, m_Players);
    }

    private void _InitPlayers(float playerLife)
    {
        Data.Player p1 = new Data.Player(0, playerLife, playerSpeed);
		p1.hasItem = true;
        m_Players[0] = p1;
        Data.Player p2 = new Data.Player(1, playerLife, playerSpeed);
        m_Players[1] = p2;

        var player1gameObject = Instantiate(m_Player1Prefab, m_Player1Container.transform) as PlayerController;
        player1gameObject.Init(p1);
        var player2gameObject = Instantiate(m_Player2Prefab, m_Player2Container.transform) as PlayerController;
        player2gameObject.Init(p2);

        m_PlayerGameObjects[0] = player1gameObject;
        m_PlayerGameObjects[1] = player2gameObject;

        m_P1Cam.target = player1gameObject.transform;
        m_P2Cam.target = player2gameObject.transform;
    }

    private void _LoadLevel(int levelIndex, bool force = false)
    {
        if (m_CurrentLevelIndex != levelIndex)
        {
            m_CurrentLevel.DestroyLevels();
        }
        var newLevelsPair = m_LevelsPrefabs[m_CurrentLevelIndex];
        var p1LevelInstance = Instantiate(newLevelsPair.p1Level, m_Player1Container.transform);
        var p2LevelInstance = Instantiate(newLevelsPair.p2Level, m_Player2Container.transform);
        m_CurrentLevel = new Data.LevelPair();
        m_CurrentLevel.p1Level = p1LevelInstance;
        m_CurrentLevel.p2Level = p2LevelInstance;

        m_CurrentLevel.p1Level.transform.localPosition = Vector3.one * -10f;
        m_CurrentLevel.p2Level.transform.localPosition = Vector3.one * -10f;
    }

    private void _ResetLevel()
    {
        m_CurrentLevelIndex = 0;
        _LoadLevel(m_CurrentLevelIndex);
    }

    private void _CheckGameOver()
    {
        int reachedEnd = 0;
        foreach (var player in m_Players)
        {
            if (player.life <= 0f)
            {
                _ChangeState(GameState.GameOver);
                return;
            }
            if (player.reachedEnd)
                reachedEnd++;
        }
        if (reachedEnd == m_PlayerGameObjects.Length) //both reached end
        { 
            _ChangeState(GameState.Finished);
            return;
        }
    }

    private void _ChangeState(GameState newState)
    {
        if(m_LastMenu != null)
        {
            m_LastMenu.gameObject.SetActive(false);
            m_LastMenu = null;
        }

        m_PreviousGameState = m_CurrentGameState;
        m_CurrentGameState = newState;
    }

    private void _Cleanup()
    {
        if (m_PlayerGameObjects != null)
        {
            foreach (var p in m_PlayerGameObjects)
                Destroy(p.gameObject);
        }
        if (m_CurrentLevel != null)
        {
            Destroy(m_CurrentLevel.p1Level.gameObject);
            Destroy(m_CurrentLevel.p2Level.gameObject);
        }
    }

    public bool isTransferingIdol = false;

    public void TransferIdol(){

        Camera p1camera = m_P1Cam.gameObject.GetComponent<Camera>();
        Camera p2camera = m_P2Cam.gameObject.GetComponent<Camera>();

        Vector3 p1WorldPos = m_PlayerGameObjects[0].transform.position;
        Vector3 p1ScreenPos = p1camera.WorldToScreenPoint(p1WorldPos);

        Vector3 p2WorldPos = m_PlayerGameObjects[1].transform.position;
        Vector3 p2ScreenPos = p2camera.WorldToScreenPoint(p2WorldPos);

        Vector3 orig = m_Players[0].hasItem ? p1ScreenPos : p2ScreenPos;
        Vector3 dest = orig == p1ScreenPos ? p2ScreenPos : p1ScreenPos;

        Time.timeScale = 0f;
        m_Hud.AnimateIdolTransfer(orig, dest, ()=> {

            m_Players[0].hasItem = !m_Players[0].hasItem;
            m_Players[1].hasItem = !m_Players[1].hasItem;
            m_Players[0].wantsSwitch = false;
            m_Players[1].wantsSwitch = false;

            Time.timeScale = 1f;
        });
    }

#region BUTTON_CALLBACKS
    public void OnNewGame()
    {
        _ChangeState(GameState.Playing);
    }

    public void OnResume(){
        m_LastMenu.gameObject.SetActive(false);
        _ChangeState(GameState.Playing);
    }

    public void OnGoToMainMenu(){
        _Cleanup();
        _Init();
        _ChangeState(GameState.Start);
    }

    public void OnRetry(){
        if(gameState == GameState.GameOver){
            _Cleanup();
            _Init();
            _ChangeState(GameState.Playing);
        }
    }

    public void OnPause(){
        if (gameState == GameState.Playing)
            _ChangeState(GameState.Paused);
    }
#endregion

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }

}
