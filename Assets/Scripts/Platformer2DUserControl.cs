using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// Adapted from UnityStandardAssets._2D.Platformer2DUserControl
[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
	public string JumpBind = "Jump";
	public string HorizontalBind = "Horizontal";
	public string VerticalBind = "Vertical";

	private PlatformerCharacter2D m_Character;
    private PlayerController m_PlayerController;
	private bool m_Jump;
	private bool m_Climb;

    //private Rigidbody2D m_Rigidbody2D;

	private void Awake()
	{
        m_PlayerController = GetComponent<PlayerController>();
		m_Character = GetComponent<PlatformerCharacter2D>();
        //m_Rigidbody2D = GetComponent<Rigidbody2D>();

	}

    Vector3 m_CurrVelocity;

	private void Update()
	{
        //m_CurrVelocity = m_Rigidbody2D.velocity;

        ////if(m_PlayerController.PlayerIndex == 1)
        //Debug.Log(m_CurrVelocity);
        //bool jumpInput = CrossPlatformInputManager.GetButtonDown(JumpBind);

        //if (jumpInput)
        //    Debug.Log("jump");

        //if(jumpInput){
        //    if (!m_Jump)
        //        m_Jump = jumpInput;
        //}

        if (!m_Jump && v == 0f)
			m_Jump = CrossPlatformInputManager.GetButtonDown(JumpBind);
	}

    float v, h;

	private void FixedUpdate()
	{
        if (m_PlayerController.player.wantsSwitch && m_Character.isGrounded)
            return;

		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		h = CrossPlatformInputManager.GetAxis(HorizontalBind);
		v = CrossPlatformInputManager.GetAxis(VerticalBind);

		m_Climb = (v != 0);

		if (h != 0)
			m_Climb = false;

		// Pass all parameters to the character control script.
		if(m_Character != null)
			m_Character.Move(h, v, crouch, m_Jump, m_Climb);
		m_Jump = false;
		m_Climb = false;
	}
}
