using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class MoveBall : MonoBehaviour {

	public		bool		enable_move		=		true;
	private		bool		waiting			=		false;

	private		float		wait_max		=		0;
	private		float		wait_time		=		0;

	public		int			speed			=		1;
	public		int			speed_rotate	=		1;
	public		string[]	directions;
	public		int[]		move;

	private		float		actualx;
	private		float		actualz;
	private		int			next			=		-1;
	private		string		direction 		=		"";
	private		int			nextx;
	private		int			nextz;

	// ------------------------------------------------------------------------------------------
	void Start() {
		UpdateActualXZ();
	}

	// ------------------------------------------------------------------------------------------
    void Update() {
		if ( !enable_move ) { return; }

		if ( waiting ) { Wait(); return; }
		if ( Move( direction, speed ) ) {

			next++;
			if ( next >= move.Length ) { next = 0; }
			if ( next < 0 ) { next = 0; }
			PrepareDirection( next );

		}
 
    }
	// ------------------------------------------------------------------------------------------
	private void PrepareDirection( int index ) {
		if ( index >= move.Length ) { return; }
		if ( index >= directions.Length ) { return; } 
		Transform	spikes		=		transform.GetChild(0);

		direction = directions[ index ];
		if ( directions[ index ] == "F" ) { nextz = Round(actualz) + move[ index ]; spikes.eulerAngles = Vector3.zero; }
		if ( directions[ index ] == "B" ) { nextz = Round(actualz) - move[ index ]; spikes.eulerAngles = Vector3.zero; }
		if ( directions[ index ] == "L" ) { nextx = Round(actualx) - move[ index ]; spikes.eulerAngles = Vector3.zero; }
		if ( directions[ index ] == "R" ) { nextx = Round(actualx) + move[ index ]; spikes.eulerAngles = Vector3.zero; }
		if ( directions[ index ] == "W" ) { WaitStart( move[ index ] ); spikes.eulerAngles = Vector3.zero; }
	}

	// ------------------------------------------------------------------------------------------
	private bool Move( string direction, int speed ) {
		bool		result		=		false;
		Vector3		rotate		=		transform.eulerAngles;
		Transform	spikes		=		transform.GetChild(0);

		if (direction == "F") {
			if ( ((float)nextz) >= actualz ) {
				transform.Translate(Vector3.forward * (Time.deltaTime * speed));
				spikes.Rotate( speed_rotate, 0, 0 );
				result = false;
			} else { result = true; }
		}
		else if (direction == "B") {
			if ( ((float)nextz) < actualz ) {
				transform.Translate(Vector3.back * (Time.deltaTime * speed));
				spikes.Rotate( -speed_rotate, 0, 0 );
				result = false;
			} else { result = true; }
		}
		else if (direction == "L") {
			if ( ((float)nextx) < actualx ) {
				transform.Translate(Vector3.left * (Time.deltaTime * speed));
				spikes.Rotate( 0, 0, speed_rotate );
				result = false;
			} else { result = true; }
		}
		else if (direction == "R") {
			if ( ((float)nextx) >= actualx ) {
				transform.Translate(Vector3.right * (Time.deltaTime * speed));
				spikes.Rotate( 0, 0, -speed_rotate );
				result = false;
			} else { result = true; }
		}
		else { result = true; }

		UpdateActualXZ();
		return result;
	}

	// ------------------------------------------------------------------------------------------
	private void WaitStart( int time ) {
		waiting		=		true;
		wait_max	=		time;
		wait_time	=		0;
	}

	private void Wait() {
		if ( waiting && wait_time < wait_max ) { wait_time += Time.deltaTime; }
		else { waiting = false; }
	}

	// ------------------------------------------------------------------------------------------
	private void UpdateActualXZ() {
		actualx		=		transform.position.x;
		actualz		=		transform.position.z;
	}

	// ------------------------------------------------------------------------------------------
	public int Round( float value ) {
		int		result		=		0;
		float	dec			=		value - (int) value;

		if (value > 0) {
			if (dec <= 0.5000000) { result = (int) value; }
			else { result = (int) value + 1; }
		} else {
			if (dec > -1.0 && dec < -0.5000000) { result = (int) value - 1; }
			else { result = (int) value; }
		}

		return result;
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################