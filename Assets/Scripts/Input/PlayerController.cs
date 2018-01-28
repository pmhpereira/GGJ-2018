using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public Data.Player player { get { return m_Player; } }

	private Data.Player m_Player; //get speed from here, theoretically, it returns the correct speed
	private PlatformerCharacter2D m_PlatformerCharacter;
	private Platformer2DUserControl m_UserControl;
	private Transform m_LastCheckpoint;
	private Handle m_Handle;
	private Ladder m_Ladder;
	private GameObject m_Step;

    [SerializeField]
    private InteractionInfo m_InteractionInfo;

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
        if (m_InteractionInfo != null)
            m_InteractionInfo.Init(m_Player);
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

        if (m_Ladder != null && !player.hasItem && !player.wantsSwitch)
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

        if (m_Player != null)
        {
            if (m_Player.hasItem)
                m_Player.life -= Time.deltaTime;

            if (m_PlatformerCharacter != null)
                m_PlatformerCharacter.maxRunningSpeed = m_Player.speed;

            //updating interaction info
            if (m_Player.wantsSwitch)
            {
                m_Player.currentInteractionInfo = Data.Player.Interaction.Trade;
            }
            else if (m_Handle != null)
            {
                var handleType = m_Handle.GetType();
                if(handleType == typeof(Lever))
                    m_Player.currentInteractionInfo = m_Player.hasItem ? Data.Player.Interaction.NotInteractable : Data.Player.Interaction.Interactable;
				else if (handleType == typeof(PressurePlate))
					m_Player.currentInteractionInfo = !m_Player.hasItem ? Data.Player.Interaction.NotInteractable : Data.Player.Interaction.Interactable;
				else if (handleType == typeof(Altar))
					m_Player.currentInteractionInfo = !m_Player.hasItem ? Data.Player.Interaction.NotInteractable : Data.Player.Interaction.None;
			}
            else if(m_Ladder != null){
                m_Player.currentInteractionInfo = !m_Player.hasItem ? Data.Player.Interaction.None : Data.Player.Interaction.NotInteractable;
            }
            else
                m_Player.currentInteractionInfo = Data.Player.Interaction.None;

        }

	}

	private void _HandleInput()
	{
		if (m_Handle != null && m_Handle.NeedsManualTrigger)
		{
            if (Input.GetButtonDown(string.Format("P{0}_Activate", PlayerIndex)))
            {
                var cena = m_Handle.GetType();

                if (cena == typeof(Lever))
                    AudioManager.Instance.PlaySFX("lever", false);

                if(!m_Player.hasItem)
                    m_Handle.ManualToggle();
            }
		}

        //some specific input..
        //if (Input.GetKeyDown(KeyCode.I))
        //m_Player.hasItem = !m_Player.hasItem;

        if (Input.GetButton(string.Format("P{0}_Switch", /*m_Player.playerIndex+1*/ PlayerIndex)) && m_PlatformerCharacter.isGrounded)
            m_Player.wantsSwitch = true;
        else
            m_Player.wantsSwitch = false;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
        if(collider.name.Contains("Idol"))
        {
            m_Player.hasItem = true;
            Destroy(collider.gameObject);
        }

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

        if(collider.name.Contains("End")){
            if(m_Player != null)
                m_Player.reachedEnd = true;
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

		if (collider.name.Contains("End"))
		{
			if (m_Player != null)
				m_Player.reachedEnd = false;
		}
	}
}
