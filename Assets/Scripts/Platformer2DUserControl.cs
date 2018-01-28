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
	private bool m_Jump;
	private bool m_Climb;

	private void Awake()
	{
		m_Character = GetComponent<PlatformerCharacter2D>();
	}

	private void Update()
	{
		if (!m_Jump)
			m_Jump = CrossPlatformInputManager.GetButtonDown(JumpBind);
	}

	private void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis(HorizontalBind);
		float v = CrossPlatformInputManager.GetAxis(VerticalBind);

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
