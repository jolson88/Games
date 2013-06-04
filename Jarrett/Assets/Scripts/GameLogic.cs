using UnityEngine;
using System.Collections.Generic;

public class BallLostMessage : Message
{
}

public class PegSelectedMessage : Message
{
	public PegBehavior Peg { get; set; }
	public PegSelectedMessage(PegBehavior peg) { this.Peg = peg; }
}

public class GameLogic : MonoBehaviour 
{
	public int OrangePegCount = 10;
	public int Balls = 8;
	
	private MessageManager _messageManager;
	private CannonControllerBehavior _cannon;
	private GameObject _playerBall;
	private int _score;
	private OTTextSprite _scoreLabel;
	
	void Awake ()
	{
		_messageManager = this.gameObject.AddComponent<MessageManager>();
		_messageManager.AddListener<BallCollisionMessage>(OnBallCollision);
	}
	
	// Use this for initialization
	void Start () 
	{
		_scoreLabel = GameObject.Find("Label-Score-Value").GetComponent<OTTextSprite>();
		_cannon = GameObject.Find("Cannon").GetComponent<CannonControllerBehavior>();
		
		_playerBall = GameObject.Find("PlayerBall");
		SpawnNewBall();
		
		InitializeLevel();
	}
	
	// Update is called once per frame
	void Update () {
		_scoreLabel.text = _score.ToString();
	}
	
	private void OnBallCollision(BallCollisionMessage msg)
	{
		if (msg.CollisionObject.CompareTag("Ball-Saver"))
		{
			CalculatePoints();
			SpawnNewBall();
		}
		else if (msg.CollisionObject.CompareTag("Trap"))
		{
			CalculatePoints();
			BallLost();
		}
	}
	
	private void BallLost()
	{
		_messageManager.QueueMessage(new BallLostMessage());
		if (LevelWon())
		{
			print ("WON!!!!!");
		}
		else
		{
			Balls--;		
			if (Balls > 0)
			{
				SpawnNewBall();
			}
			else
			{
				print("Game Over");
			}
		}
	}
	
	private void SpawnNewBall()
	{
		_cannon.AttachBall(_playerBall);
	}
	
	private bool LevelWon()
	{
		return (GameObject.FindGameObjectsWithTag("Peg-Orange").Length == 0);
	}
	
	private void InitializeLevel()
	{
		var orangeIndexes = new List<int>();
		
		var pegsToRemove = new List<GameObject>();
		var bluePegs = GameObject.FindGameObjectsWithTag("Peg-Blue");
		for (int i = 0; i < OrangePegCount; i++)
		{
			var index = 0;
			do
			{
				index = (int)(Random.value * (bluePegs.Length - 1));
			} while (orangeIndexes.Contains(index));
			
			orangeIndexes.Add(index);
			OT.CreateSpriteAt("Peg-Orange", new Vector2(bluePegs[index].transform.position.x, bluePegs[index].transform.position.y));
			pegsToRemove.Add(bluePegs[index]);
		}

		foreach(var peg in pegsToRemove)
		{
			GameObject.DestroyImmediate(peg);
		}	
	}
	
	private void CalculatePoints()
	{
		var scoreIncrease = 0;
		
		var pegObjects = GameObject.FindObjectsOfType(typeof(PegBehavior));
		foreach (var obj in pegObjects)
		{
			var peg = (PegBehavior)obj;
			if (peg.IsSelected)
			{
				scoreIncrease += peg.PointValue;
				GameObject.DestroyImmediate(peg.gameObject);
			}
		}
		
		_score += scoreIncrease;
	}
}
