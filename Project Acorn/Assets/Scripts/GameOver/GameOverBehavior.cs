using UnityEngine;
using System.Collections;

public class GameOverBehavior : MonoBehaviour
{
    private GameContext m_context;
    private GUIText m_text;
    
    // Use this for initialization
    void Start()
    {
        var go = GameObject.Find("GameContext");
        m_context = go.GetComponent<GameContext>();        
       
        m_text = this.GetComponent<GUIText>();
        m_text.text = string.Format("Player {0} Wins!!!", m_context.WinningPlayer + 1);
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }
}
