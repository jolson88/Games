using UnityEngine;
using System.Collections;

public class MovementBehavior : MonoBehaviour
{
    public float VelocityX;
    
    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(this.VelocityX * Time.deltaTime, 0, 0);
    }
}
