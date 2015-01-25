using UnityEngine;
using System;
using System.Collections;

public class TemporaryDisplay : MonoBehaviour
{
	public float countdown = 3.0f;
	private float accumTime;
	void Start()
	{
		accumTime = 0.0f;
	}

	void FixedUpdate()
	{
		accumTime += Time.fixedDeltaTime;
		if (accumTime >= countdown)
		{
			// make our thing disappear
			gameObject.active = false;
		}
	}
}
