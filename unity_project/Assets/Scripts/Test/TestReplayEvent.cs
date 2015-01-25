using UnityEngine;
using System.Collections;

public class TestReplayEvent : ReplayEvent {

  private string mKey;

  private TestReplayEvent(): base() {}
  public TestReplayEvent(string key): base() {
    mKey = key;
  }

  public override void Activate() {
    Debug.Log(mKey);
  }
}
