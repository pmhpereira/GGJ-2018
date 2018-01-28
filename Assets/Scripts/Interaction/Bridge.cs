using UnityEngine;

public class Bridge : Interactee
{
	public float ActivationLength;

	public override void Activated()
	{
        AudioManager.Instance.PlaySFX("stonemechanism", true);
		transform.position += Vector3.right * ActivationLength;
	}

	public override void Deactivated()
	{
        AudioManager.Instance.PlaySFX("stonemechanism", true);
        transform.position -= Vector3.right * ActivationLength;
	}
}
