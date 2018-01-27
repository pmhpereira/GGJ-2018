using UnityEngine;

public class Lever : Handle
{
	public override bool NeedsManualTrigger { get { return true; } }

	public bool Activated = false;

	public override void ManualToggle()
	{
		Activated = !Activated;

		if (Activated)
			OnActivated.Invoke();
		else
			OnDeactivated.Invoke();
	}
}
