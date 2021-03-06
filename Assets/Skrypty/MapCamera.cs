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
public class MapCamera : MonoBehaviour {

	private		GameObject		player;

	public		float			rayX		=		0.0f;
	public		float			rayY		=		0.0f;
	public		float			rayZ		=		-10.0f;

	public		bool			rotateX		=		false;
	public		bool			rotateY		=		false;
	public		bool			rotateZ		=		false;

	public		bool			inverseX	=		false;
	public		bool			inverseY	=		false;
	public		bool			inverseZ	=		false;

	public		float			speedX		=		0.5f;
	public		float			speedY		=		0.5f;
	public		float			speedZ		=		0.5f;

	// ------------------------------------------------------------------------------------------
	private void Start () {
		player					=		GameObject.Find("Player");
		SetLook();
	}

	// ------------------------------------------------------------------------------------------
	public void SetLook() {
		transform.position		=		player.transform.position + new Vector3( rayX, rayY, rayZ );
	}

	// ------------------------------------------------------------------------------------------
	private void Update () {
		transform.LookAt( player.transform );

		if (rotateX) {
			if (inverseX) { transform.RotateAround( player.transform.position, Vector3.left, speedX ); } 
			else { transform.RotateAround( player.transform.position, Vector3.right, speedX ); }
		}

		if (rotateY) {
			if (inverseY) { transform.RotateAround( player.transform.position, Vector3.down, speedY ); } 
			else { transform.RotateAround( player.transform.position, Vector3.up, speedY ); }
		}

		if (rotateZ) {
			if (inverseZ) { transform.RotateAround( player.transform.position, Vector3.back, speedZ ); } 
			else { transform.RotateAround( player.transform.position, Vector3.forward, speedZ ); }
		}

	}

	// ------------------------------------------------------------------------------------------
	public void RandomSettings() {
		int		random		=	0;
		bool	_switch		=	false;

		for ( int iR = 0; iR <= 5; iR++ ) {
			random = Random.Range( 0, 10 );
			if ( random <= 5 ) { _switch = true; } else { _switch = false; }

			switch ( iR ) {
				case 0: rotateX = _switch; break;
				case 1: inverseX = _switch; break;
				case 2: rotateY = _switch; break;
				case 3: inverseY = _switch; break;
				case 4: rotateZ = _switch; break;
				case 5: inverseZ = _switch; break;
			}

		}

	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################