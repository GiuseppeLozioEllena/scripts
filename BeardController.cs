using UnityEngine;
using System.Collections;

public class BeardController : MonoBehaviour {

	// Use this for initialization
	Animator beardAnimator ;
	void Start () {
		beardAnimator = GetComponent<Animator>();

	}


	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "enemy") 
		{
			
			StartCoroutine(DestroyEnemy(other.gameObject));
		}
	}
	
	IEnumerator DestroyEnemy(GameObject gameObject)
	{
		
		yield return new WaitForSeconds(0.2f); // circa il doppio del tempo dell' animazione
		
		Debug.Log("destroy!");
		Destroy (gameObject);
		
	}	
}
