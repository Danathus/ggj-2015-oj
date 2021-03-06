using UnityEngine;
using System;
using System.Collections;
  
//[RequireComponent(typeof(Animator))] 
public class IKControl : MonoBehaviour
{
	//protected Animator animator;

	//public bool ikActive = false;
	//public Transform rightHandObj = null;

	// inputs
	public Transform target;
	public Transform shoulderElbowPoint;
	public Transform wristElbowPoint;
	public Transform elbowWeight;

	// outputs
	public Transform handTransform;
	public Transform shoulderTransform;
	public Transform wristTransform;

	// constat state
	public float shoulderLength;
	public float wristLength;

	// dynamic state (exposed for debugging)
	public float elbowZ;
	public float distToTarget;
	public bool distanceFarEnough;
	public bool distanceCloseEnough;
	public bool distToTargetInRange;

	void Start()
	{
		//animator = GetComponent<Animator>();
		//Debug.Log("IKControl.Start() -- animator is ", animator);
	}

	void Awake()
	{
		shoulderLength = Vector3.Distance(shoulderTransform.position, shoulderElbowPoint.position);
		wristLength = Vector3.Distance(wristTransform.position, wristElbowPoint.position);
	}
	void Update()
	{
		handTransform.rotation = target.rotation;
		handTransform.position = wristTransform.position;

		transform.LookAt(target, transform.position - elbowWeight.position);
		distToTarget = Vector3.Distance(target.position, shoulderTransform.position);
		elbowZ = (Mathf.Pow(distToTarget, 2) - Mathf.Pow(wristLength, 2) + Mathf.Pow(shoulderLength,2))/(distToTarget * 2);

		Vector3 wristPos = wristTransform.localPosition;
		wristPos.z = Mathf.Clamp(distToTarget, 0, wristLength + shoulderLength);
		wristTransform.localPosition = wristPos;

		distanceFarEnough   = distToTarget > Mathf.Max (shoulderLength, wristLength) - Mathf.Min (shoulderLength, wristLength);
		distanceCloseEnough = distToTarget < shoulderLength + wristLength;
		distToTargetInRange = distanceFarEnough && distanceCloseEnough;
		if (distToTargetInRange){
			shoulderTransform.localRotation = Quaternion.Euler(Mathf.Acos(elbowZ/shoulderLength) * Mathf.Rad2Deg, 0, 0);
			wristTransform.localRotation = Quaternion.Euler( -(Mathf.Acos((distToTarget - elbowZ)/wristLength) * Mathf.Rad2Deg), 0, 0);
			distToTargetInRange = true;
		}
		
		if (!distanceCloseEnough)
		{
			shoulderTransform.localRotation = Quaternion.Euler(0,0,0);
			wristTransform.localRotation = Quaternion.Euler(0,0,0);
		}
		if (!distanceFarEnough)
		{
			shoulderTransform.localRotation = Quaternion.Euler(0,0,0);
			wristTransform.localRotation = Quaternion.Euler(180,0,0);
		}
		
	}

	/*
	// a callback for calculating IK
	void OnAnimatorIK(int layerIndex)
	{
		Debug.Log("IKControl.OnAnimatorIK()");
		if (animator)
		{
			// if the IK is active, set the position and rotation directly to the goal. 
			if (ikActive)
			{
				// weight = 1.0 for the right hand means position and rotation will be at the IK goal (the place the character wants to grab)
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

				// set the position and the rotation of the right hand where the external object is
				if (rightHandObj != null)
				{
					animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
				}                   
			}

			// if the IK is not active, set the position and rotation of the hand back to the original position
			else
			{          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);             
			}
		}
	}
	//*/
}

/////

