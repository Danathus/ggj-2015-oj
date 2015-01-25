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
			m_Delta = m_Target - m_Transform.position;
			m_Delta *= m_Strength * Time.fixedDeltaTime;
			Debug.Log("" + (m_Transform.position - m_Target) + ", " + m_Delta);

			m_Transform.position += m_Delta;
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
	public float m_FinishedTimer = 5.0f;

	private Vector3 m_FinishedCenter;
	public float m_holdAccum = 0.0f;
	public float m_holdTimeRequirement = 1.0f;

	// Use this for initialization
	void Start () {
		m_holdAccum = 0.0f;

		m_LeftTransform  = m_LeftPart.GetComponent<RectTransform>() as RectTransform;
		m_RightTransform = m_RightPart.GetComponent<RectTransform>() as RectTransform;

		float speed = 20.0f;
		float magnetForce = 15.0f;
		Behavior p1Mag   = new MagneticBehavior("player1 magnetic", 	   m_LeftPart, magnetForce);
		Behavior p1Up    = new RectTranslateBehavior("player1 move up",    m_LeftPart, new Vector3( 0,  1, 0) * speed);
		Behavior p1Down  = new RectTranslateBehavior("player1 move down",  m_LeftPart, new Vector3( 0, -1, 0) * speed);
		Behavior p1Left  = new RectTranslateBehavior("player1 move left",  m_LeftPart, new Vector3(-1,  0, 0) * speed);
		Behavior p1Right = new RectTranslateBehavior("player1 move right", m_LeftPart, new Vector3( 1,  0, 0) * speed);
		Behavior p2Mag   = new MagneticBehavior("player2 magnetic", 	   m_RightPart, magnetForce);
		Behavior p2Up    = new RectTranslateBehavior("player2 move up",    m_RightPart, new Vector3( 0,  1, 0) * speed);
		Behavior p2Down  = new RectTranslateBehavior("player2 move down",  m_RightPart, new Vector3( 0, -1, 0) * speed);
		Behavior p2Left  = new RectTranslateBehavior("player2 move left",  m_RightPart, new Vector3(-1,  0, 0) * speed);
		Behavior p2Right = new RectTranslateBehavior("player2 move right", m_RightPart, new Vector3( 1,  0, 0) * speed);

		m_Controls = new ControlScheme();
		m_Controls.AddControl(new TrueSignal(),                                                                     p1Mag  );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.W, new KeyCode[] { KeyCode.A, KeyCode.D }),          p1Up   );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.S, new KeyCode[] { KeyCode.A, KeyCode.D }),          p1Down );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.A, new KeyCode[] { KeyCode.W, KeyCode.S }),          p1Left );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.D, new KeyCode[] { KeyCode.W, KeyCode.S }),          p1Right);
		m_Controls.AddControl(new TrueSignal(),                                                                     p2Mag  );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.UpArrow   , new KeyCode[] { KeyCode.A, KeyCode.D }), p2Up   );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.DownArrow , new KeyCode[] { KeyCode.A, KeyCode.D }), p2Down );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.LeftArrow , new KeyCode[] { KeyCode.W, KeyCode.S }), p2Left );
		m_Controls.AddControl(new KeyCodeControlSignal(KeyCode.RightArrow, new KeyCode[] { KeyCode.W, KeyCode.S }), p2Right);

		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f, true), p1Up    );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f, true), p1Down  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f, true), p1Left  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.One, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f, true), p1Right );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y,  1.0f, true), p2Up    );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.Y, -1.0f, true), p2Down  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X, -1.0f, true), p2Left  );
		m_Controls.AddControl(new GamepadAxisControlSignal(GamepadInput.GamePad.Index.Two, GamepadInput.GamePad.Axis.LeftStick, GamepadAxisControlSignal.Dimension.X,  1.0f, true), p2Right );
	}
	
	// Update is called once per frame
	void Update() {
		if (!m_IsFinished) {
			if (m_Controls != null) {
				m_Controls.Update();
			}

			if (m_LeftTransform != null && m_RightTransform != null) {
				Vector3 offset = (m_LeftTransform.position + new Vector3(17, 0, 0)) - (m_RightTransform.position - new Vector3(17, 0, 0));
				if (offset.magnitude < 5) {
					m_holdAccum += Time.deltaTime;
					if (m_holdAccum > m_holdTimeRequirement) {
						m_IsFinished = true;
						m_FinishedCenter = (m_LeftTransform.position + new Vector3(17, 0, 0)) - (offset * 0.5f);

						m_LeftPart.GetComponent<PulseImage>().enabled = false;
						m_RightPart.GetComponent<PulseImage>().enabled = false;
						
						AudioSource audio = Camera.main.GetComponentInChildren<AudioSource>();
						audio.Play();
					}
				}
				else {
					m_holdAccum = 0.0f;
				}
			} else {
				m_IsFinished = true;
			}
		} else {
			// we're finished -- slide together
			if (m_LeftTransform != null && m_RightTransform != null) {
				// and grow bigger
				float w = 0.9f;
				float targetScale = 1.5f;
				float currentScale = m_LeftTransform.localScale.x * w + targetScale * (1 - w);
				Vector3 scalev = new Vector3(currentScale, currentScale, currentScale);
				m_LeftTransform.localScale  = scalev;
				m_RightTransform.localScale = scalev;

				Vector3 targetLeftPos = (m_FinishedCenter - targetScale * new Vector3(17, 0, 0));
				m_LeftTransform.position = m_LeftTransform.position * w + targetLeftPos * (1 - w);

				Vector3 targetRightPos = (m_FinishedCenter + targetScale * new Vector3(17, 0, 0));
				m_RightTransform.position = m_RightTransform.position * w + targetRightPos * (1 - w);
			}

			m_FinishedTimer -= Time.fixedDeltaTime;
			if (m_FinishedTimer <= 0.0f) {
				ScenarioManager.Instance.ActivateState("Play");
			}
		}
	}
}
