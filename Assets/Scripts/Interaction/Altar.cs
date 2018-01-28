using UnityEngine;

public class Altar : Handle
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
		other.GetComponent<PlayerController>().player.hasItem = false;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.GetComponent<PlayerController>().player.hasItem && !Activated)
		{
			Activated = true;
			OnActivated.Invoke();
			other.GetComponent<PlayerController>().player.hasItem = false; 
		}
	}
}
