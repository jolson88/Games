using UnityEngine;
using System.Collections;

public class BallCollisionMessage : Message
{
	public GameObject CollisionObject { get; set; }
	public BallCollisionMessage(GameObject collisionObject) { this.CollisionObject = collisionObject; }
}

public class BallCollisionBehavior : MonoBehaviour 
{
	private MessageManager _messageManager;
	private OTSprite _sprite;
	
	// Use this for initialization
	void Start () 
	{
		_messageManager = (MessageManager)GameObject.FindObjectOfType(typeof(MessageManager));
		_sprite = this.gameObject.GetComponent<OTSprite>();	
		_sprite.onCollision = OnCollision;
	}
	
	void OnCollision (OTObject owner) 
	{
		var other = _sprite.collisionObject;
		if (other.gameObject.CompareTag ("Player")) 
		{
			print ("Collision with " + this.gameObject.tag.ToString());
			_messageManager.QueueMessage(new BallCollisionMessage(this.gameObject));
		}
	}
}
