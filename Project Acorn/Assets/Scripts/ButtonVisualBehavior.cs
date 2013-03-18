using UnityEngine;
using System.Collections;

public class ButtonVisualBehavior : MonoBehaviour
{
    private exSprite m_sprite;
    
    // Use this for initialization
    void Start()
    {
        m_sprite = this.GetComponent<exSprite>();
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
    
    void OnMouseDown()
    {
        m_sprite.SetSprite(m_sprite.atlas, 2);
    }
    
    void OnMouseUp()
    {
        m_sprite.SetSprite(m_sprite.atlas, 0);
    }
    
    void OnMouseEnter()
    {
        m_sprite.SetSprite(m_sprite.atlas, 1);
    }
    
    void OnMouseExit()
    {
        m_sprite.SetSprite(m_sprite.atlas, 0);
    }
}
