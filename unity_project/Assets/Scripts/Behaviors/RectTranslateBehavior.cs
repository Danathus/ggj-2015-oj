using UnityEngine;
using System.Collections;

public class RectTranslateBehavior : Behavior
{
	RectTransform m_Transform = null;
	Vector3 mOffset;
	public RectTranslateBehavior(string name, GameObject operand, Vector3 offset)
		: base(name, operand)
	{
		mOffset = offset;
		m_Transform = operand.GetComponent<RectTransform>() as RectTransform;
	}

	public override bool Operate(float signal)
	{
		if (signal > 0.0f && m_Transform != null)
		{
			m_Transform.position += mOffset * signal;
			return true;
		}
		return false;
	}
}
