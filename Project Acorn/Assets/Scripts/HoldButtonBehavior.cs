using UnityEngine;
using System.Collections;

public class HoldButtonBehavior : MonoBehaviour
{
	public Texture2D Texture;
	public float X;
	public float Y;

	private GameLogicBehavior m_gameLogic;

	// Use this for initialization
	void Start ()
	{
		var logic = GameObject.Find("GameLogic");
		m_gameLogic = logic.GetComponent<GameLogicBehavior>();
	}

	void OnGUI()
	{
		// Convert X and Y to pixel space (from screen space)
		// Represents the center coordinates of the button
		var x = Screen.width * X;
		var y = Screen.height * Y;

		var left = x - (Texture.width / 2);
		var top = y - (Texture.height / 2);

		if (GUI.Button (new Rect (left, top, Texture.width, Texture.height), Texture)) {
			m_gameLogic.Hold();		
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
