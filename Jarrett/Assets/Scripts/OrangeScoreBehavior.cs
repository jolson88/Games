using UnityEngine;
using System.Collections;

public class OrangeScoreBehavior : MonoBehaviour {
	public Texture SelectedImage;
	public int OrangePegCount;
	
	private MessageManager _messageManager;
	private OTSprite _sprite;
	private int _orangeScoreCount = 0;
	
	// Use this for initialization
	void Start () 
	{
		_messageManager = (MessageManager)GameObject.FindObjectOfType(typeof(MessageManager));
		_messageManager.AddListener<PegSelectedMessage>(OnPegSelected);
	
		_sprite = this.GetComponent<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnPegSelected(PegSelectedMessage msg)
	{
		if (msg.Peg.CompareTag("Peg-Orange"))
		{
			_orangeScoreCount++;
			if (OrangePegCount <= _orangeScoreCount)
			{
				_sprite.image = this.SelectedImage;
			}
		}
	}
}
