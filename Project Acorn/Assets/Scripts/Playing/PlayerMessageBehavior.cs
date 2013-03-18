using UnityEngine;
using System.Collections;

public class PlayerMessageBehavior : MonoBehaviour
{
	private GUIText m_text;

	public void DisplayMessage(string msg)
	{
		m_text.text = msg;
	}

	// Use this for initialization
	void Start ()
	{
		m_text = this.GetComponent<GUIText> ();
		m_text.text = string.Empty;
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
