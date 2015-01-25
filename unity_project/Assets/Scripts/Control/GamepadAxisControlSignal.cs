using UnityEngine;
using System.Collections;

public class GamepadAxisControlSignal : ControlSignal
{
	public enum Dimension { X, Y };

	GamepadInput.GamePad.Index mGamepadIndex;
	GamepadInput.GamePad.Axis mAxis;
	Dimension mDimension;
	float mFactor;

	public GamepadAxisControlSignal(
		GamepadInput.GamePad.Index gamepadIndex,
		GamepadInput.GamePad.Axis axis,
		Dimension dimension,
		float factor)
		: base()
	{
		mGamepadIndex = gamepadIndex;
		mAxis = axis;
		mDimension = dimension;
		mFactor = factor;
	}

	public override float PollSignal()
	{
		Vector2 axis = GamepadInput.GamePad.GetAxis(mAxis, mGamepadIndex);
		float signal = (mDimension == Dimension.X ? axis.x : axis.y) * mFactor;
		return signal;
	}
}
