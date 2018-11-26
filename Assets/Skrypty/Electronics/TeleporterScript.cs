using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class TeleporterScript : MonoBehaviour {

	public		bool				active				=		false;
	public		ElectronicColor		color;
	public		int					hope				=		0;

	public		float				speed_rotate		=		22.5f;

	// ------------------------------------------------------------------------------------------
	private void Start () {
		if (active) { SetEnabled(); } else { SetDisabled(); }
		Setup();
	}

	// ------------------------------------------------------------------------------------------
	private void Setup() {
		Color	col		=	new Color( 1.000f, 1.000f, 1.000f, 1.000f );

		switch (color) {
		case ElectronicColor.Blue:
			col = new Color( 0.000f, 0.670f, 1.000f, 1.000f );
			break;
		case ElectronicColor.Green:
			col = new Color( 0.000f, 1.000f, 0.215f, 1.000f );
			break;
		case ElectronicColor.Red:
			col = new Color( 1.000f, 0.109f, 0.109f, 1.000f );
			break;
		case ElectronicColor.Yellow:
			col = new Color( 0.984f, 1.000f, 0.000f, 1.000f );
			break;
		}

		var platform										=		transform.GetChild(1).gameObject;
		platform.GetComponent<Renderer>().material.color	=		col;
	}

	// ------------------------------------------------------------------------------------------
	private void Update () {
		if (!active) { return; }

		Vector3		rotate		=	new Vector3( 0, 1, 0 );
		transform.Rotate( rotate * (Time.deltaTime * speed_rotate) );
	}

	// ------------------------------------------------------------------------------------------
	public void SetEnabled() {
		active		=		true;
		//
	}

	// ------------------------------------------------------------------------------------------
	public void SetDisabled() {
		active		=		false;
		//
	}

	// ------------------------------------------------------------------------------------------
	public int FindNextTeleporter( int szukany ) {
		GameObject	TeleportersContainer		=		GameObject.Find("Electronic_Teleporters");
		int			stary						=		0;
		int			nowy						=		0;

		if (TeleportersContainer != null) {
			foreach(Transform child in TeleportersContainer.transform) {
				var teleporter					=		child.gameObject;
				var script						=		teleporter.GetComponent<TeleporterScript>();

				if ( script.color == this.color ) {
					nowy						=		script.hope;
					if ( stary > nowy && nowy >= szukany ) { stary = nowy; }
					else if ( stary < szukany && nowy >= szukany ) { stary = nowy; }

					if ( stary == szukany ) { return stary; }
				}
			}
				
		}

		return 0;
	}

	// ------------------------------------------------------------------------------------------
	public GameObject GetNextTeleporter( int szukany ) {
		GameObject	TeleportersContainer		=		GameObject.Find("Electronic_Teleporters");

		if (TeleportersContainer != null) {
			foreach(Transform child in TeleportersContainer.transform) {
				var teleporter					=		child.gameObject;
				var script						=		teleporter.GetComponent<TeleporterScript>();

				if ( script.color == this.color && script.hope == szukany ) {
					return teleporter;
				}
			}
		}

		return null;
	}

	// ------------------------------------------------------------------------------------------
	public Vector3 GetTeleportedPosition() {
		int x = (int) transform.position.x;
		int y = (int) transform.position.y;
		int z = (int) transform.position.z;

		return new Vector3( x, y+1, z );
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################