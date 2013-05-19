using UnityEngine;
using System.Collections;

public class PlayerBallBehavior : MonoBehaviour {

	private MessageManager _messageManager;
	private OTSprite _sprite;
	
	// Use this for initialization
	void Start () 
	{
		_messageManager = (MessageManager)GameObject.FindObjectOfType(typeof(MessageManager));
		_sprite = this.gameObject.GetComponent<OTSprite>();	
		_sprite.onCollision = OnCollision;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnCollision (OTObject owner) 
	{
		var other = _sprite.collisionObject;
		if (other.gameObject.CompareTag ("Ball-Trap")) 
		{
			_messageManager.QueueMessage(new BallLostMessage());
		}
	}
}
