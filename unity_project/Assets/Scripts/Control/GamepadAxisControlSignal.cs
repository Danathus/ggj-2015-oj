using UnityEngine;
using System.Collections;

public class GamepadAxisControlSignal : ControlSignal
{
	public enum Dimension { X, Y };

	GamepadInput.GamePad.Index mGamepadIndex;
	GamepadInput.GamePad.Axis mAxis;
	Dimension mDimension;
	float mFactor;
	bool mCircularBoundary; // eliminates "square root of 2" advantage

	public GamepadAxisControlSignal(
		GamepadInput.GamePad.Index gamepadIndex,
		GamepadInput.GamePad.Axis axis,
		Dimension dimension,
		float factor,
		bool circularBoundary = false)
		: base()
	{
		mGamepadIndex = gamepadIndex;
		mAxis = axis;
		mDimension = dimension;
		mFactor = factor;
		mCircularBoundary = circularBoundary;
	}

	public override float PollSignal()
	{
		Vector2 axis = GamepadInput.GamePad.GetAxis(mAxis, mGamepadIndex);
		float signal = (mDimension == Dimension.X ? axis.x : axis.y) * mFactor;
		if (mCircularBoundary)
		{
			if (axis.magnitude > 1.0f)
			{
				signal /= axis.magnitude;
			}
		}
		return signal;
	}
}
