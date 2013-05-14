using UnityEngine;
using System.Collections;

public class PlayerBallBehavior : MonoBehaviour {

	private GameLogic _logic;
	private OTSprite _sprite;
	
	// Use this for initialization
	void Start () {
		_logic = GameObject.Find("Logic-Game").GetComponent<GameLogic>();
		_sprite = this.gameObject.GetComponent<OTSprite>();	
		_sprite.onCollision = OnCollision;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollision (OTObject owner) {
		var other = _sprite.collisionObject;
		if (other.gameObject.CompareTag ("Trap")) {
			print ("Trap collided");
			_logic.BallLost();
		}
	}
}
