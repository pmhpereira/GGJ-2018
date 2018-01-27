using UnityEngine;
using UnityEngine.Events;

public abstract class Interactee : MonoBehaviour
{
	protected Interactee()
	{

	}

	public abstract void Activated();
	public abstract void Deactivated();
}
