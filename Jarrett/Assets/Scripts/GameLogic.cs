using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	private CannonControllerBehavior _cannon;
	private GameObject _playerBall;
	
	// Use this for initialization
	void Start () {
		_cannon = GameObject.Find("Cannon").GetComponent<CannonControllerBehavior>();
		
		_playerBall = GameObject.Find("PlayerBall");
		SpawnNewBall();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void BallLost()
	{
		print ("Ball lost!");
		SpawnNewBall();
	}
	
	void SpawnNewBall()
	{
		_cannon.AttachBall(_playerBall);
	}
}
