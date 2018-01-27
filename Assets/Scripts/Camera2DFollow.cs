using System;
using UnityEngine;

// Adapted from UnityStandardAssets._2D.Camera2DFollow
public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
	public float yDelta;

	private float m_OffsetY;
	private float m_OffsetZ;
    private Vector3 m_CurrentVelocity;

    // Use this for initialization
    private void Start()
	{ 
		if(target != null)
	        m_OffsetZ = (transform.position - target.position).z;
    }

	private bool _repositioning = false;

    // Update is called once per frame
    private void Update()
    {
		if (_repositioning == false && Math.Abs(transform.position.y - target.position.y) < yDelta)
			return;

		_repositioning = true;

		Vector3 aheadTargetPos = target.position + Vector3.forward * m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
		newPos.x = 0;

		if (Math.Abs(newPos.y - target.position.y) < 0.1f)
			_repositioning = false;

		transform.localPosition = newPos;
    }
}
