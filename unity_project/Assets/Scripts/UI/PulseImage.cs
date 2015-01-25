using UnityEngine;
using System.Collections;

public class PulseImage : MonoBehaviour {

	public float m_MinScale = 1.0f;
	public float m_MaxScale = 1.1f;
	public float m_PulseSpeed = 1.0f;

	private float m_CurrentScale = -1.0f;
	private bool  m_IsScalingUp  = true;

	private RectTransform m_ImageComponent = null;

	// Use this for initialization
	void Start () {
		m_CurrentScale = m_MinScale;

		m_ImageComponent = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_IsScalingUp) {
			m_CurrentScale += Time.deltaTime * m_PulseSpeed;

			if (m_CurrentScale >= m_MaxScale) {
				m_CurrentScale = m_MaxScale;
				m_IsScalingUp = false;
			}
		} else {
			m_CurrentScale -= Time.deltaTime * m_PulseSpeed;

			if (m_CurrentScale <= m_MinScale) {
				m_CurrentScale = m_MinScale;
				m_IsScalingUp = true;
			}
		}

		if (m_ImageComponent != null) {
			m_ImageComponent.localScale = new Vector3(m_CurrentScale, m_CurrentScale, m_CurrentScale);
		}
	}

	void OnDisable() {
		if (m_ImageComponent != null) {
			m_ImageComponent.localScale = new Vector3(m_MinScale, m_MinScale, m_MinScale);
		}
	}
}
