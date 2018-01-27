using UnityEngine;

public class PressurePlate : Handle
{
	public override bool NeedsManualTrigger { get { return false; } }

	public bool Activated = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		Activated = true;
		OnActivated.Invoke();
	}

	void OnTriggerExit2D(Collider2D other)
	{
		Activated = false;
		OnDeactivated.Invoke();
	}
}
