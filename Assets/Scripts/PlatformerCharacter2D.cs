﻿using UnityEngine;

// Adapted from UnityStandardAssets._2D.PlatformerCharacter2D
public class PlatformerCharacter2D : MonoBehaviour
{
    public float maxRunningSpeed { get { return m_MaxRunningSpeed; } set { m_MaxRunningSpeed = value; } }
    public float maxClimbingSpeed { get { return m_MaxClimbingSpeed; } set { m_MaxClimbingSpeed = value; } }

    public bool canClimb;
    public bool isGrounded { get { return m_Grounded; } }

    [SerializeField] private float m_MaxRunningSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_MaxClimbingSpeed = 3f;                    // The fastest the player can travel in the ys axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    [SerializeField]
    private AudioSource m_Footsteps;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	private bool m_Grounded;            // Whether or not the player is grounded.
	private bool m_Climbing;            // Whether or not the player is climbing.
	const float k_GroundedRadius = .01f; // Radius of the overlap circle to determine if grounded
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private bool canGround;

	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}

		canGround = true;
		m_Grounded = m_Grounded && !m_Climbing;
		m_Anim.SetBool("Ground", m_Grounded);
	}

	public void Move(float hMove, float vMove, bool crouch, bool jump, bool climb)
	{
		/** /
		// If crouching, check to see if the character can stand up
		if (!crouch && m_Anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		crouch = false;

		// Set whether or not the character is crouching in the animator
		m_Anim.SetBool("Crouch", crouch);
		/**/

		m_Anim.SetFloat("vSpeed", 0);

		if (climb)
		{
			if (!canClimb)
			{
				m_Grounded = true;
				m_Climbing = false;
				m_Rigidbody2D.isKinematic = false;
				m_Anim.SetBool("Ground", true);
				m_Anim.SetBool("Climbing", false);
				m_Anim.SetFloat("vSpeed", 0);
			}
			else if (m_Grounded && !m_Climbing && canClimb)
			{
				m_Grounded = false;
				m_Climbing = true;
				m_Rigidbody2D.isKinematic = true;

				m_Rigidbody2D.velocity = Vector2.zero;
				m_Anim.SetBool("Ground", false);
				m_Anim.SetBool("Climbing", true);
				m_Anim.SetFloat("vSpeed", (int) Mathf.Abs(vMove));

				if (vMove == 0)
					return;

				m_Rigidbody2D.transform.position += Vector3.up * m_MaxClimbingSpeed * vMove / Mathf.Abs(vMove) * Time.deltaTime;
				return;
			}
			else if (m_Climbing)
			{
				m_Rigidbody2D.velocity = Vector2.zero;
				m_Anim.SetBool("Ground", false);
				m_Anim.SetBool("Climbing", true);
				m_Anim.SetFloat("vSpeed", (int) Mathf.Abs(vMove));
					
				if (vMove == 0)
					return;

				m_Rigidbody2D.transform.position += Vector3.up * m_MaxClimbingSpeed * vMove / Mathf.Abs(vMove) * Time.deltaTime;
				return;
			}
			else if (canGround)
			{
				m_Grounded = true;
				m_Climbing = false;
				m_Rigidbody2D.isKinematic = false;
				m_Anim.SetBool("Ground", true);
				m_Anim.SetBool("Climbing", false);
				m_Anim.SetFloat("vSpeed", 0);
			}
		}

		//only control the player if grounded or airControl is turned on
		if (!m_Climbing && (m_Grounded || m_AirControl))
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			hMove = (crouch ? hMove * m_CrouchSpeed : hMove);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat("Speed", Mathf.Abs(hMove));

            bool stuck = hMove != 0f && m_Rigidbody2D.velocity.x == 0f && !isGrounded;

            // Move the character
            m_Rigidbody2D.velocity = new Vector2((hMove * m_MaxRunningSpeed), m_Rigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (hMove > 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (hMove < 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (m_Grounded && jump && m_Anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Anim.SetBool("Ground", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
        if(m_Grounded)
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                if (!m_Footsteps.isPlaying)
                    m_Footsteps.Play();
            }
            else
            {
                if (m_Footsteps.isPlaying)
                    m_Footsteps.Stop();
            }
        }
        else
        {
            if (m_Footsteps.isPlaying)
                m_Footsteps.Stop();
        }
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
