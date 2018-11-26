using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
// 
//	Podążanie komerą za graczem
//	Przyblizanie i oddalanie kamery
//	Obrót kamery o 90 stopni w zasięgu 360 stopni
//	
// ####################################################################################################
public class Camera : MonoBehaviour {

	public		GameObject		player;
	//public		GameObject		camera;

	private		Vector3			difference;
	private		Vector3			approach;
	public		int				zoom				=	20;

	public		int				cameraPosHeight		=	4;
	public		int 			cameraPosSide 		=	-2;
	public		float			zoomScaleHeight		=	0.1f;
	public		float			zoomScaleSide		=	-0.05f;
	public		int				zoomSpeed			=	2;
	public		int				zoomMax				=	20;
	public		int				zoomMin				=	0;

	// ------------------------------------------------------------------------------------------
	private void Start () {
		difference					=	new Vector3( 0, cameraPosHeight, cameraPosSide );
		approach					=	new Vector3( 0, zoomScaleHeight, zoomScaleSide );
	}

	// ------------------------------------------------------------------------------------------
	private void Update () {
		if ( Time.timeScale == 0 ) { return; }

		Zoom();
		Rotate();

		transform.position			=	player.transform.position;
		transform.GetChild(0).transform.position	=	transform.position + difference + (approach*zoom);
	}

	// ------------------------------------------------------------------------------------------
	private void Rotate() {
		Vector3	rotator				=	new Vector3( 0, 90, 0 );
		int		move_direction		=	player.GetComponent<Player>().move_direction;
		int		prev_direction		=	(int) transform.eulerAngles.y;

		if ( Input.GetKeyDown(KeyCode.Q) ) {
			move_direction -= (int) rotator.y;
			if ( move_direction < 0 ) { move_direction = 270; }
			player.GetComponent<Player>().move_direction = move_direction;

			transform.eulerAngles -= rotator;
			RepairDifference( prev_direction, move_direction );
		}

		if ( Input.GetKeyDown(KeyCode.E) ) {
			move_direction += (int) rotator.y;
			if ( move_direction >= 360 ) { move_direction = 0; }
			player.GetComponent<Player>().move_direction = move_direction;

			transform.eulerAngles += rotator;
			RepairDifference( prev_direction, move_direction );
		}


	}

	// ------------------------------------------------------------------------------------------
	private void RepairDifference( int prev, int next ) {
		//Vector3	cameraPosition		=	camera.transform.position;
		//Vector3	newPosition			=	camera.transform.position;
		switch( next ) {
		case 0:
			//if ( prev == 90 ) { newPosition = new Vector3( cameraPosition.z, 0, -cameraPosition.x ); }
			//else if ( prev == 270 ) { newPosition = new Vector3( -cameraPosition.z, 0, cameraPosition.x ); }
			difference				=	new Vector3( 0, cameraPosHeight, cameraPosSide );
			approach				=	new Vector3( 0, zoomScaleHeight, zoomScaleSide );
			break;
		case 90:
			//if ( prev == 0 ) { newPosition = new Vector3( cameraPosition.z, 0, -cameraPosition.x ); }
			//else if ( prev == 180 ) { newPosition = new Vector3( -cameraPosition.z, 0, cameraPosition.x ); }
			difference				=	new Vector3( cameraPosSide, cameraPosHeight, 0 );
			approach				=	new Vector3( zoomScaleSide, zoomScaleHeight, 0 );
			break;
		case 180:
			//if ( prev == 90 ) { newPosition = new Vector3( -cameraPosition.z, 0, cameraPosition.x ); }
			//else if ( prev == 270 ) { newPosition = new Vector3( cameraPosition.z, 0, -cameraPosition.x ); }
			difference				=	new Vector3( 0, cameraPosHeight, -cameraPosSide );
			approach				=	new Vector3( 0, zoomScaleHeight, -zoomScaleSide );
			break;
		case 270:
			//if ( prev == 0 ) { newPosition = new Vector3( -cameraPosition.z, 0, cameraPosition.x ); }
			//else if ( prev == 180 ) { newPosition = new Vector3( cameraPosition.z, 0, -cameraPosition.x ); }
			difference				=	new Vector3( -cameraPosSide, cameraPosHeight, 0 );
			approach				=	new Vector3( -zoomScaleSide, zoomScaleHeight, 0 );
			break;
		}
		//camera.transform.position	=	newPosition + difference + (approach*zoom);;
	}

	// ------------------------------------------------------------------------------------------
	private void Zoom() {
		if ( (Input.GetAxis("Mouse ScrollWheel") < 0) && (zoom < zoomMax) ) { zoom += zoomSpeed; }
		if ( (Input.GetAxis("Mouse ScrollWheel") > 0) && (zoom > zoomMin) ) { zoom -= zoomSpeed; }
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################