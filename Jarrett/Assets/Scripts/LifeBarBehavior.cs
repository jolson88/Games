using UnityEngine;
using System.Collections;

public class LifeBarBehavior : MonoBehaviour 
{
	public int BallCount;
	public Texture SelectedImage;
	
	private MessageManager _messageManager;
	private OTSprite _sprite;
	private int _ballsLost = 0;
	
	// Use this for initialization
	void Start () 
	{
		_messageManager = (MessageManager)GameObject.FindObjectOfType(typeof(MessageManager));
		_messageManager.AddListener<BallLostMessage>(OnBallLost);
		
		_sprite = this.GetComponent<OTSprite>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnBallLost (BallLostMessage msg)
	{
		_ballsLost++;
		
		if (BallCount <= _ballsLost)
		{
			_sprite.image = this.SelectedImage;
		}
	}
}
