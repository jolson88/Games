using UnityEngine;
using System.Collections;

public class ScoreBehavior : MonoBehaviour
{
	public int PlayerIndex;

	private GameLogicBehavior m_gameLogic;
	private GUIText m_text;

	// Use this for initialization
	void Start ()
	{
		var logic = GameObject.Find("GameLogic");
		m_gameLogic = logic.GetComponent<GameLogicBehavior>();

		m_text = this.GetComponent<GUIText> ();
	}

	// Update is called once per frame
	void Update ()
	{
		m_text.text = string.Format ("P{0} Score: {1}", PlayerIndex + 1, m_gameLogic.PlayerScores[PlayerIndex]);
	}
}
