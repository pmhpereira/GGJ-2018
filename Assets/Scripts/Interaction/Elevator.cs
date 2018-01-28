using System;
using UnityEngine;

public class Elevator : Interactee
{
	public int minLength;
	public int maxLength;
	public float speed;
	public int yDirection = 1; // up

    public float prevlength = 0f;
	public float currentLength = 1f;

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
        currentLength = maxLength;
		cart = transform.Find("Cart");
		startingHeight = currentLength;
		startingTransform = cart.localPosition;
	}

	public void Update()
	{
		currentLength += -yDirection * speed * Time.deltaTime;
        if ( prevlength == currentLength && (currentLength > maxLength || currentLength < minLength))
            AudioManager.Instance.PlaySFX("ding", true);
		currentLength = Mathf.Clamp(currentLength, minLength, maxLength);
        prevlength = currentLength;
		cart.localPosition = startingTransform + Vector3.up * (startingHeight - currentLength);
	}
}
