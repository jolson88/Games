using UnityEngine;
using System.Collections.Generic;

public class CardBehavior : MonoBehaviour
{
	public int CardBackingAtlasIndex;
	public int SingleValueAtlasIndex;
	public int DoubleValueAtlasIndex;
	public int ZeroValueAtlasIndex;	
	public int CardIndex;
	public int CardValue;
	
    private MessageBus m_messageBus;
	private exSprite m_sprite;
	private Dictionary<int,int> m_pointValueToAtlasIndex;
	
	public void Reset ()
	{		
		UpdateCardGraphic(CardBackingAtlasIndex);
	}
	
	// Use this for initialization
	void Start ()
	{
		m_sprite = this.GetComponent<exSprite>();
		
		m_pointValueToAtlasIndex = new Dictionary<int, int>();
		m_pointValueToAtlasIndex.Add(0, ZeroValueAtlasIndex);
		m_pointValueToAtlasIndex.Add(1, SingleValueAtlasIndex);
		m_pointValueToAtlasIndex.Add(2, DoubleValueAtlasIndex);
		
        var go = GameObject.Find("MessageBus");
        m_messageBus = go.GetComponent<MessageBus>();
        
		Reset ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseDown ()
	{
		UpdateCardGraphic(m_pointValueToAtlasIndex[CardValue]);
        
        var msg = new SelectCardMessage() { CardIndex = this.CardIndex };
        m_messageBus.QueueMessage(msg);	
	}
	
	void UpdateCardGraphic (int index)
	{
		m_sprite.SetSprite(m_sprite.atlas, index);
	}
}
