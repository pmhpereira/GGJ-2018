using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
	private Data.Player m_Player; //get speed from here, theoretically, it returns the correct speed
	private PlatformerCharacter2D m_PlatformerCharacter;
	private Platformer2DUserControl m_UserControl;
	private Vector3 m_LastCheckpoint;
	private Handle m_Handle;
	private Ladder m_Ladder;
	private GameObject m_Step;

	public int PlayerIndex;

	private void Awake()
	{
		m_UserControl = GetComponent<Platformer2DUserControl>();
		m_PlatformerCharacter = GetComponent<PlatformerCharacter2D>();
	}

	public void Init(Data.Player player)
	{
		m_Player = player;
		m_UserControl.HorizontalBind = string.Format("P{0}_Horizontal", m_Player.playerIndex + 1);
		m_UserControl.JumpBind = string.Format("P{0}_Jump", m_Player.playerIndex + 1);
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		//if (m_Player == null)
		//	return;
		_HandleInput();
		_UpdatePlayer();
	}

	private void _UpdatePlayer()
	{
		if (m_Ladder != null)
		{
			float v = CrossPlatformInputManager.GetAxis(string.Format("P{0}_Vertical", PlayerIndex));
			if (m_Step == m_Ladder.bottomStep && v > 0)
				m_PlatformerCharacter.canClimb = true;
			else if (m_Step == m_Ladder.bottomStep && v < 0)
				m_PlatformerCharacter.canClimb = false;
			else if (m_Step == m_Ladder.topStep && v > 0)
				m_PlatformerCharacter.canClimb = false;
			else if (m_Step == m_Ladder.topStep && v < 0)
				m_PlatformerCharacter.canClimb = true;
		}

		return;

		if (m_Player.hasItem)
			m_Player.life -= Time.deltaTime;

		m_PlatformerCharacter.maxRunningSpeed = m_Player.speed;
	}

	private void _HandleInput()
	{
		if (m_Handle != null && m_Handle.NeedsManualTrigger)
		{
			if (Input.GetButtonDown(string.Format("P{0}_Activate", PlayerIndex)))
				m_Handle.ManualToggle();
		}

		return;

		//some specific input..
		if (Input.GetKeyDown(KeyCode.I))
			m_Player.hasItem = !m_Player.hasItem;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		Handle handle = collider.gameObject.GetComponent<Handle>();
		if (handle != null)
		{
			m_Handle = handle;
			return;
		}

		Ladder ladder = collider.transform.parent.GetComponent<Ladder>();
		if (ladder != null)
		{
			m_Ladder = ladder;
			m_Step = collider.gameObject;
			return;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		Handle handle = collider.gameObject.GetComponent<Handle>();
		if (handle != null)
		{
			m_Handle = null;
			return;
		}

		Ladder ladder = collider.transform.parent.GetComponent<Ladder>();
		if (ladder != null)
		{
			m_Ladder = null;
			m_Step = null;
			return;
		}
	}
}
