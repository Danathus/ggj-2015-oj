using UnityEngine;
using System.Collections;

public class ReplayEvent {

  private float _time = 0.0f;

  public float mTime {
    get { return _time; }
    private set { _time = value; }
  }

  ReplayEvent () {
    mTime = Time.time;
  }

  public void Activate() {

  }
}
