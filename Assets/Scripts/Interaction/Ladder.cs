using System;
using UnityEngine;

public class Ladder : Interactee
{
	public GameObject topStep;
	public GameObject bottomStep;

	public float ActivationLength;

	public override void Activated()
	{
        AudioManager.Instance.PlaySFX("ladderopen", false);
		transform.position += Vector3.up * ActivationLength;
	}

	public override void Deactivated()
	{
        AudioManager.Instance.PlaySFX("ladderopen", false);
        transform.position -= Vector3.up * ActivationLength;
	}
}
