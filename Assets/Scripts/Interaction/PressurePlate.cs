using UnityEngine;

public class PressurePlate : Handle
{
	public override bool NeedsManualTrigger { get { return false; } }

	public bool Activated = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (Activated)
			return;

        if (!other.GetComponent<PlayerController>().player.hasItem)
            return;

		Activated = true;
		OnActivated.Invoke();
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (!Activated)
			return;

		Activated = false;
		OnDeactivated.Invoke();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.GetComponent<PlayerController>().player.hasItem && !Activated)
		{
			Activated = true;
			OnActivated.Invoke();
		}
		else if(!other.GetComponent<PlayerController>().player.hasItem && Activated)
		{
			Activated = false;
			OnDeactivated.Invoke();
		}
	}
}
