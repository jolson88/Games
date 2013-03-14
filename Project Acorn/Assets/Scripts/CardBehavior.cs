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
	
	private exSprite m_sprite;
	private GameLogicBehavior m_gameLogic;
	private Dictionary<int,int> m_pointValueToAtlasIndex;
	
	public void Reset ()
	{		
		UpdateCardGraphic(CardBackingAtlasIndex);
	}
	
	// Use this for initialization
	void Start ()
	{
		var logic = GameObject.Find("GameLogic");
		m_gameLogic = logic.GetComponent<GameLogicBehavior>();
		
		m_sprite = this.GetComponent<exSprite>();
		
		m_pointValueToAtlasIndex = new Dictionary<int, int>();
		m_pointValueToAtlasIndex.Add(0, ZeroValueAtlasIndex);
		m_pointValueToAtlasIndex.Add(1, SingleValueAtlasIndex);
		m_pointValueToAtlasIndex.Add(2, DoubleValueAtlasIndex);
		
		Reset ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnMouseDown ()
	{
		UpdateCardGraphic(m_pointValueToAtlasIndex[CardValue]);
		m_gameLogic.CardSelected(this.CardIndex);		
	}
	
	void UpdateCardGraphic (int index)
	{
		m_sprite.SetSprite(m_sprite.atlas, index);
	}
}
