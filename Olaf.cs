using UnityEngine;
using System.Collections;

public class Olaf : MonoBehaviour {

	// Use this for initialization
	Animator olafAnimator;
	GameObject enemyHit;

	bool facingRight = true; // For determining which way the player is currently facing
	bool collision = false;
	
	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	float grabRadius = 6.0f;
	float grabForce = 1000.0f;
	float platformGrabAdditionalMovement = 4.0f;
	float grabMovementSpeed = 25.0f;



	void Awake()
	{
		// Setting up references.
	    
		olafAnimator = GetComponent<Animator>();
		olafAnimator.SetBool("isPunch", false);
	}

	// Update is called once per frame

	public void Update()
	{

		LayerMask layerMask = 1 << 11;					// User defined layer Grabbable
		
		// Same code used for the grab mechanic, here used just to debug in scene
		RaycastHit2D[] grabbableObjects = Physics2D.CircleCastAll(transform.position, grabRadius, Vector2.zero, 0.0f, layerMask);
		foreach(RaycastHit2D inRange in grabbableObjects) {
			RaycastHit2D[] hitted = Physics2D.RaycastAll(transform.position, (inRange.point - (Vector2)(transform.position)).normalized, grabRadius, layerMask);
			foreach(RaycastHit2D hittedIn in hitted) {
				Debug.DrawLine(transform.position, hittedIn.point, Color.red);
			}
		}

	}

	IEnumerator DestroyEnemy()
	{
	
		yield return new WaitForSeconds(0.0f); // circa il doppio del tempo dell' animazione

		Debug.Log("destroy!");
		Destroy (enemyHit);
		collision=false;
	}
	
	void OnTriggerStay2D(Collider2D other)
	{

		if (other.tag == "enemy") 
		{
			enemyHit=other.gameObject; // mi salvo l' oggetto colpito
			collision=true;
		
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
	
		if (other.tag == "enemy") 
		{
			Debug.Log("exit");
			collision=false;

			
		}
	}

	public void Move(float move, bool punch, bool jump, bool grab)
	{

		if (move > 0 && !facingRight)
						Flip ();

		if (move < 0 && facingRight)
			Flip ();

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		// If the player should jump...

		olafAnimator.SetBool("isPunch", punch);
		if (punch) 
		{
			if(collision)
			{
				Debug.Log ("punch");
				StartCoroutine(DestroyEnemy()); 		
			}
		}


		if (jump) 
		{
			// Add a vertical force to the player.
			olafAnimator.SetBool("isJumping", true);
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
		
		// If the player attempts a grab...
		olafAnimator.SetBool("isGrabbing", grab);
		if(grab)
		{
			LayerMask layerMask = 1 << 11; //user defined layer Grabbable
			// To identify grabbable entities we cast a circle
			RaycastHit2D[] grabbableObjects = Physics2D.CircleCastAll(transform.position, grabRadius, Vector2.zero, 0.0f, layerMask);
			
			// TODO: For each hitted target we remove the ones that aren't front-facing the character and aren't in a specific angle range 
			foreach(RaycastHit2D inRange in grabbableObjects) {
				// For each hitted target we cast another Ray since CircleCast functionn is bugged and the hit points are accurate!
				RaycastHit2D[] hitted = Physics2D.RaycastAll(transform.position, (inRange.point - (Vector2)(transform.position)).normalized, grabRadius, layerMask);
				
				if ( hitted.Length > 0 ) {
					// TODO: right now only "platform" grabbing implemented, here we should check if the grabbed object is a platform or an enemy and act accordingly
					
					// At this point we should only have the interesting collider, we define the destination point of the platform grab animation
					Vector2 destination = (Vector2)hitted[0].point + (hitted[0].point - (Vector2)(transform.position)).normalized*platformGrabAdditionalMovement;
					// Start coruotine for the movement animation
					StartCoroutine(GrabMovement(destination));
					//StartCoroutine(GrabMovement(Vector2.Lerp(transform.position, hitted[0].point, 1.5f)));
					/*
					if (grabSpring.enabled) {
						grabSpring.enabled = false;
					}
					else {
						grabSpring.collideConnected = true;
						grabSpring.enabled = true;
						grabSpring.anchor = Vector2.zero;
						grabSpring.connectedBody = null;
						grabSpring.connectedAnchor = hitted[0].point;
						grabSpring.dampingRatio = 1.0f;
						grabSpring.frequency = 2.0f;
						grabSpring.distance = hitted[0].distance/4.0f;
						/*Vector2 forceDirection = (hitted[0].point - (Vector2)transform.position);
					Vector2 appliedForce = new Vector2(forceDirection.x* grabForce, forceDirection.y*grabForce/4);
					Debug.DrawRay(transform.position, forceDirection, Color.green, 10f);
					rigidbody2D.AddForce(appliedForce, ForceMode2D.Force);
					}*/
				}
			}
		}
		
	}

	IEnumerator GrabMovement(Vector2 target)
	{
		//Vector2 savedVelocity = new Vector2(rigidbody2D.velocity);
		while ((Vector2)transform.position != target)
		{
			// hacky way, better way?
			rigidbody2D.gravityScale = 0;
			transform.position = Vector3.MoveTowards(transform.position, target, grabMovementSpeed * Time.deltaTime);
			yield return 0;
		}
		
		// hacky way to keep some momentum, should figure out a better solution
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x+10.0f, 0f);
		rigidbody2D.gravityScale = 3;
	}


	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	
}
