using System;
using UnityEngine;

public class Elevator : Interactee
{
	public int minLength;
	public int maxLength;
	public float speed;
	public int yDirection = 1; // up

	public float currentLength;

	private float startingHeight;
	private Vector3 startingTransform;

	private Transform cart;

	public override void Activated()
	{
        AudioManager.Instance.PlaySFX("elevatormusic", true);
		yDirection = -yDirection;
	}

	public override void Deactivated()
	{
        AudioManager.Instance.PlaySFX("elevatormusic", true);
        yDirection = -yDirection;
	}

	public void Start()
	{
		currentLength = Mathf.Clamp(currentLength, minLength, maxLength);
		cart = transform.Find("Cart");
		startingHeight = currentLength;
		startingTransform = cart.localPosition;
	}

	public void Update()
	{
		currentLength += -yDirection * speed * Time.deltaTime;
        if (currentLength > maxLength || currentLength < minLength)
            AudioManager.Instance.PlaySFX("ding", true);
		currentLength = Mathf.Clamp(currentLength, minLength, maxLength);

		cart.localPosition = startingTransform + Vector3.up * (startingHeight - currentLength);
	}
}
