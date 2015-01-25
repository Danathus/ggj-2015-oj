using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameObjectReverter
{
	public GameObjectReverter (GameObject gameObject)
	{
		_gameObject = gameObject;
		_position = _gameObject.transform.position;
		_rotation = _gameObject.transform.rotation;
	}

	public void Revert()
	{
		_gameObject.transform.position = _gameObject.rigidbody.position = _position;
		_gameObject.transform.rotation = _gameObject.rigidbody.rotation = _rotation;
		_gameObject.rigidbody.velocity = _gameObject.rigidbody.angularVelocity = new Vector3();
	}

	private GameObject _gameObject;
	private Vector3 _position;
	private Quaternion _rotation;
}

