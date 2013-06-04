using UnityEngine;
using System.Collections;

public class PegBehavior : MonoBehaviour 
{
	public Texture SelectedImage;
	public bool IsSelected = false;
	public int PointValue;
	
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
	
	void OnCollision(OTObject owner)
	{
		if (_sprite.collisionObject.gameObject.CompareTag("Player") && !this.IsSelected)
		{
			this.IsSelected = true;
			_sprite.image = this.SelectedImage;
			
			_messageManager.QueueMessage(new PegSelectedMessage(this));
		}
	}
}
