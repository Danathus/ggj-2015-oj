﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReplayManager : Singleton<ReplayManager> {
  protected ReplayManager () {}

  private List<ReplayEvent> mEventList = new List<ReplayEvent>();
  private float mCurrentTime = 0.0f;
  private int mCurrentEvent = -1;

  private bool _isReplaying = false;



  public bool mIsReplaying {
    get { return _isReplaying; }
    private set { _isReplaying = value; }
  }

  // Use this for initialization
  void Start () {
    DontDestroyOnLoad(gameObject);
  }
  
  // Update is called once per frame
  void FixedUpdate () {
    if (mIsReplaying) {
      UpdateReplay();
    }
  }

  public void AddEvent(ReplayEvent e) {
    mEventList.Add(e);
  }

  public void Clear() {
    mEventList.Clear();
  }

  public void Play() {
    if (mEventList.Count > 0) {
      mIsReplaying = true;
      mCurrentEvent = 0;
      mCurrentTime = mEventList[0].mTime;
    }
  }

  public void Stop() {
	Debug.Log("stopping!");
    mIsReplaying = false;
    mCurrentEvent = -1;
  }

  private void UpdateReplay() {
    mCurrentTime += Time.fixedDeltaTime;

    if (mCurrentEvent > -1 && mCurrentEvent < mEventList.Count) {
      while (mCurrentEvent < mEventList.Count && mEventList[mCurrentEvent].mTime <= mCurrentTime) {
		  //Debug.Log("UpdateReplay");
        mEventList[mCurrentEvent].Activate();
        mCurrentEvent++;
      }
    } else {
      // Stop the replay automatically when we come to the end of the events.
	  Debug.Log("getting to the end!");
      Stop();
    }
  }
}
