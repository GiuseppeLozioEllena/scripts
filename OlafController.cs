using UnityEngine;
using System.Collections;

public class OlafController : MonoBehaviour {

	private Olaf character;
	private bool jump=false;
	private bool grab = false;
	private bool punch=false;
	// Use this for initialization

	void Awake()
	{
		character = GetComponent<Olaf>();

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
		if (Input.GetButtonDown("Jump")) jump = true;
		#endif

		if (Input.GetKeyDown (KeyCode.RightShift)) {
						punch = true;
						
				}
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
