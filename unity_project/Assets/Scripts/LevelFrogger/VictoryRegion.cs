using UnityEngine;
using System.Collections;

public class VictoryRegion : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Object Entered the trigger");
		FroggerMan froggerMan = other.gameObject.GetComponent<FroggerMan>();
		froggerMan.Victory();
	}
}
