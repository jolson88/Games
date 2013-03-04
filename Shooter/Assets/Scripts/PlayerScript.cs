using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{	
	private float m_leftBounds;
	private float m_rightBounds;
	
	public float PlayerSpeed;
	
	void Start ()
	{
		var cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
		var gameCamera = cameraObject.GetComponent<Camera>();
		
		var leftInWorld = gameCamera.ScreenToWorldPoint(Vector3.zero);
		var rightInWorld = gameCamera.ScreenToWorldPoint(new Vector3(gameCamera.pixelWidth, gameCamera.pixelHeight, 0));
		
		m_leftBounds = leftInWorld.x;
		m_rightBounds = rightInWorld.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float amountToMove = Input.GetAxisRaw("Horizontal") * this.PlayerSpeed * Time.deltaTime;
		transform.Translate(Vector3.right * amountToMove);		
		
		// Remember, transform are coordinates for the center of the Game Object
		// So account for width of the game object to make wraping smoother
		float playerWidth = transform.localScale.x;
		if (transform.position.x + (playerWidth * 0.5f) < m_leftBounds)
		{
			transform.position = new Vector3(m_rightBounds + (playerWidth * 0.48f), transform.position.y, transform.position.z);
		}
		else if (transform.position.x - (playerWidth * 0.5f) > m_rightBounds)
		{
			transform.position = new Vector3(m_leftBounds - (playerWidth * 0.48f), transform.position.y, transform.position.z);
		}
	}
}
