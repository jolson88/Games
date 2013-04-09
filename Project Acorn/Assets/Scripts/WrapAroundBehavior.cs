using UnityEngine;
using System.Collections;

public class WrapAroundBehavior : MonoBehaviour
{
    private exSprite m_sprite;
    private float m_lastWrapAroundTime;
    
    // Use this for initialization
    void Start()
    {
        m_sprite = this.GetComponent<exSprite>();
        if (m_sprite == null)
        {
            Debug.LogError("WrapAroundBehavior attached to component without exSprite");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_sprite)
        {
            var imageHalf = m_sprite.width / 2;
            var screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
            if (screenPosition.x + imageHalf < 0 || screenPosition.x - imageHalf > Screen.width)
            {
                if (Time.timeSinceLevelLoad - m_lastWrapAroundTime > 0.5)
                {
                    this.transform.Translate(this.transform.position.x * -1 * 2, 0, 0);
                    m_lastWrapAroundTime = Time.timeSinceLevelLoad;
                }
            }
        }
    }
}
