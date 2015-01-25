using UnityEngine;
using System.Collections;

public class MagneticBehavior: Behavior
{
	private RectTransform m_Transform = null;
	private float   m_Strength = 1.0f;
	private Vector3 m_Target;
	private Vector3 m_Delta;
	
	public MagneticBehavior(string name, GameObject operand, float strength)
		: base(name, operand)
	{
		m_Strength = strength;
		m_Transform = operand.GetComponent<RectTransform>() as RectTransform;

		if (m_Transform != null) {
			m_Target = m_Transform.position;
		}
		m_Delta = new Vector3(0, 0, 0);
	}
	
	public override bool Operate(float signal)
	{
		if (signal > 0.0f && m_Transform != null)
		{
			Vector3 offset = m_Target - m_Transform.position;
			offset *= m_Strength * Time.fixedDeltaTime;
			m_Delta = offset;

			m_Transform.position += offset;
			return true;
		}
		return false;
	}
	
	public override Behavior GenerateRecordedBehavior()
	{
		TranslateBehavior generated_behavior = new TranslateBehavior("auto generate magnetic behavior", mOperand, m_Delta);
		return generated_behavior;
	}
}

public class TitleScreen : MonoBehaviour {

	public GameObject m_LeftPart = null;
	public GameObject m_RightPart = null;

	private RectTransform m_LeftTransform = null;
	private RectTransform m_RightTransform = null;

	private ControlScheme m_Controls;
	private bool m_IsFinished = false;
	private float m_FinishedTimer = 5.0f;

	private Vector3 m_FinishedCenter;

	// Use this for initialization
	void Start () {
		m_LeftTransform  = m_LeftPart.GetComponent<RectTransform>() as RectTransform;
		m_RightTransform = m_RightPart.GetComponent<RectTransform>() as RectTransform;

		float speed = 20.0f;
		Behavior p1Mag   = new MagneticBehavior("player1 magnetic", 	   m_LeftPart, 3.0f);
		Behavior p1Up    = new RectTranslateBehavior("player1 move up",    m_LeftPart, new Vector3( 0,  1, 0) * speed);
		Behavior p1Down  = new RectTranslateBehavior("player1 move down",  m_LeftPart, new Vector3( 0, -1, 0) * speed);
		Behavior p1Left  = new RectTranslateBehavior("player1 move left",  m_LeftPart, new Vector3(-1,  0, 0) * speed);
		Behavior p1Right = new RectTranslateBehavior("player1 move right", m_LeftPart, new Vector3( 1,  0, 0) * speed);
		Behavior p2Mag   = new MagneticBehavior("player2 magnetic", 	   m_RightPart, 3.0f);
		Behavior p2Up    = new RectTranslateBehavior("player2 move up",    m_RightPart, new Vector3( 0,  1, 0) * speed);
		Behavior p2Down  = new RectTranslateBehavior("player2 move down",  m_RightPart, new Vector3( 0, -1, 0) * speed);
		Behavior p2Left  = new RectTranslateBehavior("player2 move left",  m_RightPart, new Vector3(-1,  0, 0) * speed);
		Behavior p2Right = new RectTranslateBehavior("player2 move right", m_RightPart, new Vector3( 1,  0, 0) * speed);

		m_Controls = new ControlScheme();
		m_Controls.AddControl(new TrueSignal(), 							p1Mag  );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.W),          p1Up   );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.S),          p1Down );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.A),          p1Left );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.D),          p1Right);
		m_Controls.AddControl(new TrueSignal(), 							p2Mag  );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow),    p2Up   );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow),  p2Down );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow),  p2Left );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow), p2Right);

		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p1Up    );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p1Down  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p1Left  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p1Right );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f), p2Up    );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f), p2Down  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f), p2Left  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f), p2Right );
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_IsFinished) {
			if (m_Controls != null) {
				m_Controls.Update();
			}

			if (m_LeftTransform != null && m_RightTransform != null) {
				Vector3 offset = (m_LeftTransform.position + new Vector3(30, 0, 0)) - (m_RightTransform.position - new Vector3(30, 0, 0));
				if (offset.magnitude < 5) {
					m_IsFinished = true;
					m_FinishedCenter = (m_LeftTransform.position + new Vector3(30, 0, 0)) - (offset * 0.5f);

					m_LeftPart.GetComponent<PulseImage>().enabled = false;
					m_RightPart.GetComponent<PulseImage>().enabled = false;
				}
			} else {
				m_IsFinished = true;
			}
		} else {
			if (m_LeftTransform != null && m_RightTransform != null) {
				Vector3 offset = (m_FinishedCenter - new Vector3(17, 0, 0)) - m_LeftTransform.position;
				offset *= 3.0f * Time.fixedDeltaTime;
				m_LeftTransform.position += offset;

				offset = (m_FinishedCenter + new Vector3(17, 0, 0)) - m_RightTransform.position;
				offset *= 3.0f * Time.fixedDeltaTime;
				m_RightTransform.position += offset;
			}

			m_FinishedTimer -= Time.fixedDeltaTime;
			if (m_FinishedTimer <= 0.0f) {
				ScenarioManager.Instance.ActivateState("Play");
			}
		}
	}
}
