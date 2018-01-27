using UnityEngine;
using UnityEngine.Events;

public abstract class Handle : MonoBehaviour {

	public UnityEvent OnActivated;
	public UnityEvent OnDeactivated;

	public virtual bool NeedsManualTrigger { get { return true; } }

	protected Handle()
	{

	}

	public virtual void ManualToggle() { }
}
