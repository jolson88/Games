using UnityEngine;
using System.Collections;

public class BallSaverBehavior : MonoBehaviour 
{
	public float Speed;

	private MessageManager _messageManager;	
	private OTSprite _sprite;
	
	// Use this for initialization
	void Start () 
	{
		_messageManager = (MessageManager)GameObject.FindObjectOfType(typeof(MessageManager));
		_sprite = this.gameObject.GetComponent<OTSprite>();	
		_sprite.onCollision = OnCollision;
		
		var physics = this.gameObject.GetComponent<Rigidbody>();
		physics.freezeRotation = true;
	}
	
	void Update () 
	{
		_sprite.position += new Vector2(Speed * Time.deltaTime, 0);
	}
	
	void OnCollision (OTObject owner) 
	{
		print ("Ball Saver collision");
		var other = _sprite.collisionObject;
		if (other.gameObject.CompareTag ("Wall")) 
		{
			print ("Collision with wall");
			// Switch directions
			Speed = -Speed;
		}
		else if (other.gameObject.CompareTag ("Player")) 
		{
			print ("Collision with " + this.gameObject.tag.ToString());
			_messageManager.QueueMessage(new BallCollisionMessage(this.gameObject));
		}
		
		var physics = this.gameObject.GetComponent<Rigidbody>();
		physics.velocity = Vector3.zero;
		physics.angularVelocity = Vector3.zero;
	}
}
