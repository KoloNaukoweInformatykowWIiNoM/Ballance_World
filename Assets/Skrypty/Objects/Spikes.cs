using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class Spikes : MonoBehaviour {

	private		float		position_Start		=		0.20f;
	private		float		position_Stop		=		0.50f;
	public		float		position_init		=		0.20f;
	private		float		position_active		=		0.20f;

	public		string		direction_init		=		"WU";
	private		string		direction_active	=		"WU";

	public		float		move_speed			=		0.75f;

	public		float		time_waitINIT		=		1.0f;
	private		float		time_wait			=		0;
	private		float		time_waitMAX		=		0;
	private		bool		time_active			=		false;

	// ------------------------------------------------------------------------------------------
	private void Start() {
		direction_active		=		direction_init;
		position_active			=		position_init;
		time_waitMAX			=		time_waitINIT;
		Prepare();
	}

	// ------------------------------------------------------------------------------------------
	private void Update() {

		Move( direction_active );
		ChangeDirection( direction_active );

	}

	// ------------------------------------------------------------------------------------------
	private void Prepare() {
		Vector3 tp = transform.position;

		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Block_FieldSpikes")) {
				Vector3	position	=	child.position;
				child.position		=	new Vector3( position.x, tp.y + position_init, position.z );
			}
		}

	}

	// ------------------------------------------------------------------------------------------
	private void Move( string dir ) {
		Vector3 tp = transform.position;

		if ( dir == "WD" || dir == "WU" ) {
			if ( !time_active ) { SetWait(); }
			else { Waiting(); }
			return;
		}

		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Block_FieldSpikes")) {
				Vector3	position	=	child.position;

				if ( dir == "U" ) { child.Translate(Vector3.forward * (Time.deltaTime * move_speed)); }
				if ( dir == "D" ) { child.Translate(Vector3.back * (Time.deltaTime * move_speed)); }
				position_active		=	child.position.y;
			}
		}

	}

	// ------------------------------------------------------------------------------------------
	private void ChangeDirection( string dir ) {
		Vector3 tp = transform.position;

		if ( dir == "U" && position_active >= tp.y+position_Stop ) { direction_active = "WD"; }
		if ( dir == "WD" && !time_active ) { direction_active = "D"; }
		if ( dir == "D" && position_active <= tp.y+position_Start ) { direction_active = "WU"; }
		if ( dir == "WU" && !time_active ) { direction_active = "U"; }
	}

	// ------------------------------------------------------------------------------------------
	private void SetWait() {
		time_wait		=		0;
		time_active		=		true;
	}

	private void Waiting() {

		if ( time_active ) { 
			if ( time_wait < time_waitMAX ) { time_wait += Time.deltaTime; }	
			else { time_active = false; }
		}

	}

	// ------------------------------------------------------------------------------------------

	/*
	private		float		posStart		=		0.20f;
	private		float		posStop			=		0.50f;

	public		float		time_showen		=		1f;
	public		float		time_hidden		=		1f;
	private		bool		time_active		=		false;
	private		float		time_wait		=		0f;
	private		float		time_max		=		0f;

	public		bool		step_init		=		true;
	public		int			steps			=		3;
	public		int			step_active		=		0;
	public		float		step_speed		=		0.5f;
	private		string		step_direction	=		"";
	private		float		step;

	// ------------------------------------------------------------------------------------------
	void Start () {
		step	=	posStart + ((posStop - posStart) * step_active);
		MovePrepare( step );

		if (step_init) { step_direction = "S"; }
		else { step_direction = "H"; }
	}
	
	// ------------------------------------------------------------------------------------------
	void Update () {
		if ( step_direction == "WS" ) { Wait(); }
		if ( step_direction == "WH" ) { Wait(); }

		if ( step_direction == "S" ) {
			if ( posStop > step ) {
				step = MoveShow();
			} else {
				step_direction = "WH";
				WaitStartHide( time_hidden );
			}
		}

		if ( step_direction == "H" ) {
			if ( posStop > step ) {
				step = MoveHide();
			} else {
				step_direction = "WHS";
				WaitStartHide( time_showen );
			}
		}
	}

	// ------------------------------------------------------------------------------------------
	private void MovePrepare( float step ) {
		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Block_FieldSpikes")) {
				Vector3	position = child.position;
				child.position = new Vector3( position.x, step, position.z );
			}
		}
	}

	private float MoveShow() {
		float		timeadd		=		Time.deltaTime;
		float		result		=		posStart;

		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Block_FieldSpikes")) {
				child.Translate(Vector3.forward * (timeadd * step_speed));
				result = child.position.y;
			}
		}

		return result;
	}

	private float MoveHide() {
		float		timeadd		=		Time.deltaTime;
		float		result		=		posStart;

		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Block_FieldSpikes")) {
				child.Translate(Vector3.back * (timeadd * step_speed));
				result = child.position.y;
			}
		}

		return result;
	}

	// ------------------------------------------------------------------------------------------
	private void WaitStartHide( float init_time ) {
		time_active		=		true;
		time_max		=		init_time;
		time_wait		=		0;
	}

	private void Wait() {
		if ( time_active && time_wait < time_max ) {
			time_wait += Time.deltaTime;
		} else {
			time_active = false;
			if ( step_direction == "WS" ) { step_direction = "S"; }
			if ( step_direction == "WH" ) { step_direction = "H"; }
		}
	}

	// ------------------------------------------------------------------------------------------
	*/
}
// ####################################################################################################