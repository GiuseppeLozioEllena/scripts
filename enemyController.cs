using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	// Use this for initialization
	Rigidbody2D enemy;
	[SerializeField] float speed=2f;
	int direction=1;
	Vector2  position=new Vector2(0f,0f);
	void Start () {
		enemy = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () { 
		rigidbody2D.velocity = new Vector2(direction * speed, rigidbody2D.velocity.y);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		if (collider.tag == "trigger1") 
		{
			direction=1;
		
		}

		if (collider.tag == "trigger2") 
		{
			direction=-1;
			
		}

	}
}
