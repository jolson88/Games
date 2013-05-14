using UnityEngine;
using System.Collections;

public class CannonControllerBehavior : MonoBehaviour {
	public float RotationSpeed;
	public float ShootingThrust;
	
	private GameObject _spawn;
	private GameObject _playerBall;
	private bool _ballReleased;
	private float _rotation;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		var newRotation = _rotation + Input.GetAxis("Horizontal") * Time.deltaTime * RotationSpeed;
		_rotation = Mathf.Clamp(newRotation, -65, 65) % 360f;
		
		var cannonSprite = this.GetComponent<OTSprite>();
		cannonSprite.rotation = _rotation;
		
		if (Input.GetButtonDown("Jump") && !_ballReleased && _playerBall != null) 
		{
			print ("FIRING THRUSTERS!!!!");
			
			var sprite = _playerBall.GetComponent<OTSprite>();
			var physics = _playerBall.GetComponent<Rigidbody>();
			_playerBall.transform.parent = null;
			
			sprite.physics = OTObject.Physics.RigidBody;
			physics.useGravity = true;

			var rotateQuat = Quaternion.AngleAxis(_rotation, Vector3.forward);
			var forceVector = rotateQuat * Vector3.down;
			forceVector *= ShootingThrust;
			
			physics.AddForce(forceVector, ForceMode.Impulse);
			_ballReleased = true;
		}
	}
	
	public void AttachBall (GameObject ball)
	{
		_spawn = GameObject.Find("Cannon-Ball-Spawn");
		_ballReleased = false;
		_playerBall = ball;
		
	    _playerBall.transform.position = _spawn.transform.position;
	    _playerBall.transform.rotation = _spawn.transform.rotation;
	    _playerBall.transform.parent = this.transform;
	    
		var sprite = _playerBall.GetComponent<OTSprite>();
		var physics = _playerBall.GetComponent<Rigidbody>();
		sprite.physics = OTObject.Physics.NoGravity;
		physics.useGravity = false;
		physics.velocity = Vector3.zero;
		physics.angularVelocity = Vector3.zero;
	}
}
