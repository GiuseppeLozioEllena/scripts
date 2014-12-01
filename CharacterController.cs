using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	private Character character;
	private bool jump=false;
	private bool grab = false;
	private bool punch=false;
	private int run;
	
	
	
	// Use this for initialization
	
	void Awake()
	{
		character = GetComponent<Character>();
		
	}
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.LeftShift)) grab = true;
		// Read the jump input in Update so button presses aren't missed.
		#if CROSS_PLATFORM_INPUT
		if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
		#else
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		#endif
		
		if (Input.GetKeyDown (KeyCode.RightShift)) {
			punch = true;
			
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
			run++;
			Debug.Log(run);		
		}
		if (Input.GetAxis ("Horizontal") == 0)
			run = 0;
	}
	
	void FixedUpdate()
	{
		// Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		#else
		float h = Input.GetAxis("Horizontal");
		#endif
		
		// Pass all parameters to the character control script.
		character.Move( h, punch, jump , grab);
		
		// Reset the jump input once it has been used.
		
		jump = false;
		grab = false;
		punch = false;
		
	}
}
