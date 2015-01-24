using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// PLAYER CONTROL SCHEME ///////////////////////////////////////////////////////

// a behavior is a thing a player can do in the scenario
//   does not define how this behavior is controlled
public abstract class Behavior
{
	public string mName;
	protected GameObject mOperand;

	public Behavior() {}
	public Behavior(string name, GameObject operand)
	{
		mName = name;
		mOperand = operand;
	}

	// input: signal -- degree [0.0, 1.0] to which signal should be operated (0.0 means do nothing, generally)
	// output: true iff should record
	public abstract bool Operate(float signal);
	public virtual Behavior GenerateRecordedBehavior()
	{
		return this;
	}
}

public class TranslateBehavior : Behavior
{
	Vector3 mOffset;
	public TranslateBehavior(string name, GameObject operand, Vector3 offset)
		: base(name, operand)
	{
		mOffset = offset;
	}

	public override bool Operate(float signal)
	{
		if (signal > 0.0f)
		{
			mOperand.transform.position += mOffset * signal;
			return true;
		}
		return false;
	}
}

// a scenario includes an ordered set of behaviors that can be done
class Scenario
{
	List<Behavior> mBehaviors = new List<Behavior>();
	Dictionary<string, Behavior> mNameToBehavior = new Dictionary<string, Behavior>();

	public void AddBehavior(Behavior behavior)
	{
		mBehaviors.Add(behavior);
		mNameToBehavior[behavior.mName] = behavior;
	}
	public Behavior GetBehavior(string name)
	{
		return mNameToBehavior[name];
	}
}

public abstract class ControlSignal
{
	public ControlSignal() {}
	public abstract float PollSignal();
}

public class KeyCodeControlSignal : ControlSignal
{
	KeyCode mKeyCode;

	public KeyCodeControlSignal(KeyCode keyCode)
		: base()
	{
		mKeyCode = keyCode;
	}

	public override float PollSignal()
	{
		bool keyDown = Input.GetKey(mKeyCode);
		return keyDown ? 1.0f : 0.0f;
	}
}

class ControlScheme
{
	List< KeyValuePair<ControlSignal, Behavior> > mBehaviors = new List< KeyValuePair<ControlSignal, Behavior> >();

	public void AddControl(ControlSignal controlSignal, Behavior behavior)
	{
		mBehaviors.Add(new KeyValuePair<ControlSignal, Behavior>(controlSignal, behavior));
	}

	public void Update()
	{
		foreach (KeyValuePair<ControlSignal, Behavior> key_behavior in mBehaviors)
		{
			ControlSignal controlSignal = key_behavior.Key;
			float signal = controlSignal.PollSignal();

			Behavior behavior = key_behavior.Value;
			bool shouldRecord = behavior.Operate(signal);

			if (shouldRecord)
			{
				BehaviorEvent behavior_event = new BehaviorEvent(behavior.GenerateRecordedBehavior(), signal);
				ReplayManager.Instance.AddEvent(behavior_event);
			}
		}
	}
}

// ADAPT REPLAY EVENT PATTERN //////////////////////////////////////////////////

public class BehaviorEvent : ReplayEvent
{
	Behavior mBehavior;
	float mSignal;

	private BehaviorEvent() {}
	public BehaviorEvent(Behavior behavior, float signal)
		: base()
	{
		mBehavior = behavior;
		mSignal = signal;
	}

	public override void Activate()
	{
		mBehavior.Operate(mSignal);
	}
}

// BEHAVIOR ENTRY POINT ////////////////////////////////////////////////////////

public class Main : MonoBehaviour
{
	private GameObject mPlayer1, mPlayer2;
	private ControlScheme mControls;

	// Use this for initialization
	void Start () {
		// create mock scene with two player objects
		mPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer1.transform.position = new Vector3(-1, 0, 0);
		mPlayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		mPlayer2.transform.position = new Vector3( 1, 0, 0);

		// create the behaviors in this scene
		Scenario scenario = new Scenario();
		float speed = 0.1f;
		scenario.AddBehavior(new TranslateBehavior("player1 move up",    mPlayer1, new Vector3( 0,  1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move down",  mPlayer1, new Vector3( 0, -1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move left",  mPlayer1, new Vector3(-1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player1 move right", mPlayer1, new Vector3( 1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move up",    mPlayer2, new Vector3( 0,  1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move down",  mPlayer2, new Vector3( 0, -1, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move left",  mPlayer2, new Vector3(-1,  0, 0) * speed));
		scenario.AddBehavior(new TranslateBehavior("player2 move right", mPlayer2, new Vector3( 1,  0, 0) * speed));

		// create the control scheme that maps inputs to these behaviors
		mControls = new ControlScheme();
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.W),          scenario.GetBehavior("player1 move up"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.S),          scenario.GetBehavior("player1 move down"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.A),          scenario.GetBehavior("player1 move left"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.D),          scenario.GetBehavior("player1 move right"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    scenario.GetBehavior("player2 move up"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  scenario.GetBehavior("player2 move down"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  scenario.GetBehavior("player2 move left"));
		mControls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), scenario.GetBehavior("player2 move right"));
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	// called once per timestep update (critical: do game state updates here!!!)
	void FixedUpdate()
	{
		// update all the controls
		mControls.Update();

		if (Input.GetKey(KeyCode.Space))
		{
			// start the playback
			ReplayManager.Instance.PlayReplay();
		}
	}

	// helper functions
}

// END OF FILE /////////////////////////////////////////////////////////////////
