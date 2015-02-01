using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinalStats : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text text = this.GetComponent<Text> ();
		text.text = ScenarioManager.Instance.GenerateStatsText ();
	}
}
