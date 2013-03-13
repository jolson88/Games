using UnityEngine;
using System.Collections;

public class CardBehavior : MonoBehaviour {
	
	public int CardIndex;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown () {
		print (string.Format("Card {0} Clicked", CardIndex));
	}
}
