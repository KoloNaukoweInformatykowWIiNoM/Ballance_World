using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
// 
//	Poruszanie się gracza po mapie
//	
// ####################################################################################################
public class Player : MonoBehaviour {

	private		Vector3		move;
	private		float		speed_diff		=		0f;

	public		bool		jump_enable		=		true;
	public		bool		move_lock		=		false;
	public		bool		speed_enable	=		false;

	public		bool 		move_allow	 	= 		false;
	public		bool		jump_allow		=		true;
	public		int			jump_height		=		300;

	public		float		max_speed		=		5f;
	public		int			move_speed		=		10;
	public		int			move_direction	=		0;

	public		bool		teleportable	=		true;

	// ------------------------------------------------------------------------------------------
	private void Start()	{ speed_diff	=		max_speed; }
	private void Update()	{ /* nothing to do */ }

	// ------------------------------------------------------------------------------------------
	private void FixedUpdate() {
		if (!move_allow) { return; }

		if ( jump_enable && jump_allow ) {
			if ( Input.GetKey(KeyCode.Space) ) {
				GetComponent<Rigidbody>().AddForce( new Vector3( 0, jump_height, 0 ));
				jump_allow	=	false;
				move_allow	=	false;
				GetComponent<Collisions>().setBurning( false );
				GetComponent<Collisions>().setMoveLock( false );
				GetComponent<Collisions>().setSpeedup( false );
				GetComponent<Sounds>().SetMotionType( SoundType.None );
			}
		}

		if ( move_lock ) { Move(); return; }

		float	movex	=	Input.GetAxis( "Horizontal" );
		float	movez	=	Input.GetAxis( "Vertical" );

		switch( move_direction ) {
		case 0:
			move		=	new Vector3( movex, 0, movez );
			break;
		case 90:
			move		=	new Vector3( movez, 0, -movex );
			break;
		case 180:
			move		=	new Vector3( -movex, 0, -movez );
			break;
		case 270:
			move		=	new Vector3( -movez, 0, movex );
			break;
		}

		Move();
	}

	// ------------------------------------------------------------------------------------------
	private void Move() {
		var		rigidbody		=	GetComponent<Rigidbody>();
		float	move_speedN		=	move_speed;

		if ( speed_enable ) {
			speed_diff		=	max_speed * 4f;
			move_speedN		=	move_speed * 4f;
		} else {
			if ( speed_diff > max_speed ) { speed_diff -= Time.deltaTime * (max_speed * 4f); }
		}
			
		rigidbody.AddForce( move * move_speedN );
		if ( rigidbody.velocity.magnitude > speed_diff ) { rigidbody.velocity = rigidbody.velocity.normalized * speed_diff; }	
	}

	// ------------------------------------------------------------------------------------------
	public void StopMove() {
		move	=	Vector3.zero;
		GetComponent<Rigidbody>().velocity	=	Vector3.zero;
		Move();
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void MakeColorBurn() {
		float		red		=		GetComponent<Renderer>().material.color.r;
		float		green	=		GetComponent<Renderer>().material.color.g;
		float		blue	=		GetComponent<Renderer>().material.color.b;
		float		alpha	=		GetComponent<Renderer>().material.color.a;

		if ( green > 0 )	{ green = green - (Time.deltaTime/3); }
		if ( blue > 0 )		{ blue	= blue - (Time.deltaTime/3); }
		GetComponent<Renderer>().material.color = new Color( red, green, blue, alpha );
	}

	// ------------------------------------------------------------------------------------------
	public void MakeColorFrost() {
		float		red		=		GetComponent<Renderer>().material.color.r;
		float		green	=		GetComponent<Renderer>().material.color.g;
		float		blue	=		GetComponent<Renderer>().material.color.b;
		float		alpha	=		GetComponent<Renderer>().material.color.a;

		if ( green > 0 )	{ green = green - (Time.deltaTime/3); }
		if ( red > 0 )		{ red	= red - (Time.deltaTime/3); }
		GetComponent<Renderer>().material.color = new Color( red, green, blue, alpha );
	}

	// ------------------------------------------------------------------------------------------
	public void MakeColorNormal() {
		float		red		=		GetComponent<Renderer>().material.color.r;
		float		green	=		GetComponent<Renderer>().material.color.g;
		float		blue	=		GetComponent<Renderer>().material.color.b;
		float		alpha	=		GetComponent<Renderer>().material.color.a;

		if ( green < 1 )	{ green	= green + (Time.deltaTime/3); }
		if ( blue < 1 )		{ blue	= blue + (Time.deltaTime/3); }
		if ( red < 1 )		{ red 	= red + (Time.deltaTime/3); }
		GetComponent<Renderer>().material.color = new Color( red, green, blue, alpha );
	}

	// ------------------------------------------------------------------------------------------

}
// ####################################################################################################