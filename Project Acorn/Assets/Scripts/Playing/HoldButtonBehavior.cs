using UnityEngine;
using System.Collections;

public class HoldButtonBehavior : MonoBehaviour
{
	private GameLogicBehavior m_gameLogic;

	// Use this for initialization
	void Start ()
	{
		var logic = GameObject.Find("GameLogic");
		m_gameLogic = logic.GetComponent<GameLogicBehavior>();
	}

	// Update is called once per frame
	void Update ()
	{

	}
    
    void OnMouseDown () 
    {
        m_gameLogic.Hold();
    }
}
