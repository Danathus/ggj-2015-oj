using UnityEngine;
using System.Collections;

public abstract class ReplayEvent {

  private float _time = 0.0f;

  public float mTime {
    get { return _time; }
    private set { _time = value; }
  }

  public ReplayEvent () {
    mTime = Time.time;
  }

  public abstract void Activate();
}
