using UnityEngine;
using System.Collections;

public class ArrowBounce : MonoBehaviour {

  public enum DirectionEnum {
    Up,
    Right,
    Down,
    Left
  };

  public DirectionEnum m_Direction = DirectionEnum.Right;

  [Tooltip("The distance (in percentage of screen) for the arrow to move when bouncing.")]
  public float m_BounceDistance = 0.1f;

  [Tooltip("The rate (in seconds) between each bounce.")]
  public float m_BounceRate     = 1.0f;

  private RectTransform m_Transform = null;
  private float m_Delta;
  private float m_Origin;
  private bool  m_IsClimbing = true;

  // Use this for initialization
  void Start () {
    m_Transform = GetComponent<RectTransform>() as RectTransform;
    m_Delta = 0.0f;
    
    if (m_Transform != null) {
      switch (m_Direction) {
        case DirectionEnum.Up:
        case DirectionEnum.Down:
          m_Origin = m_Transform.position.y;
          break;
        case DirectionEnum.Left:
        case DirectionEnum.Right:
          m_Origin = m_Transform.position.x;
          break;
      }
    }
  }

  // Update is called once per frame
  void FixedUpdate () {
    if (m_Transform != null) {
      if (m_IsClimbing) {
        m_Delta += Time.fixedDeltaTime * (1.0f / m_BounceRate);
        if (m_Delta >= 1.0f) {
          m_IsClimbing = false;
        }
      } else {
        m_Delta -= Time.fixedDeltaTime * (1.0f / m_BounceRate) * 0.5f;
        if (m_Delta <= 0.0f) {
          m_IsClimbing = true;
        }
      }

      float bounceMax = 0.0f;
      switch (m_Direction) {
        case DirectionEnum.Up:
          bounceMax = Screen.height * m_BounceDistance;
          m_Transform.position = new Vector2(m_Transform.position.x, m_Origin + (bounceMax * m_Delta));
          break;

        case DirectionEnum.Down:
          bounceMax = Screen.height * m_BounceDistance;
          m_Transform.position = new Vector2(m_Transform.position.x, m_Origin - (bounceMax * m_Delta));
          break;

        case DirectionEnum.Left:
          bounceMax = Screen.width * m_BounceDistance;
          m_Transform.position = new Vector2(m_Origin - (bounceMax * m_Delta), m_Transform.position.y);
          break;

        case DirectionEnum.Right:
          bounceMax = Screen.width * m_BounceDistance;
          m_Transform.position = new Vector2(m_Origin + (bounceMax * m_Delta), m_Transform.position.y);
          break;
      }
    }
  }
}
