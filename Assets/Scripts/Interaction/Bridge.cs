using UnityEngine;

public class Bridge : Interactee
{
	public float ActivationLength;

	public override void Activated()
	{
		transform.position += Vector3.right * ActivationLength;
	}

	public override void Deactivated()
	{
		transform.position -= Vector3.right * ActivationLength;
	}
}
