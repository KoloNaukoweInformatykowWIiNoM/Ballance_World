using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################

public enum ElectronicColor {
	Blue,
	Green,
	Red,
	Yellow
}

// ####################################################################################################
public class ButtonScript : MonoBehaviour {

	public		ElectronicColor		color;
	public		bool				active		=		false;

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

		var light										=		transform.GetChild(0).gameObject;
		var container									=		transform.parent.gameObject;
		var casing										=		container.transform.GetChild(0);
		Material[] c_materials							=		casing.GetComponent<Renderer>().materials;
		GetComponent<Renderer>().material.color			=		col;
		light.GetComponent<Light>().color				=		col;
		c_materials[3].color							= 		col;
	}

	// ------------------------------------------------------------------------------------------
	public void Touched() {
		if (active) {
			SetDisabled();
		} else {
			SetEnabled();
		}
	}

	// ------------------------------------------------------------------------------------------
	public void SetEnabled() {
		active									=		true;
		var light								=		transform.GetChild(0).gameObject;
		light.GetComponent<Light>().intensity	=		3;
	}

	// ------------------------------------------------------------------------------------------
	public void SetDisabled() {
		active									=		false;
		var light								=		transform.GetChild(0).gameObject;
		light.GetComponent<Light>().intensity	=		0;
	}
		
	// ------------------------------------------------------------------------------------------
	public void ActionOnButtons( bool action ) {
		GameObject	ButtonsContainer			=		GameObject.Find("Electronic_Buttons");

		if (ButtonsContainer != null) {
			foreach(Transform child in ButtonsContainer.transform) {
				var button						=		child.gameObject;
				var click						=		button.transform.GetChild(1).gameObject;
				var script						=		click.GetComponent<ButtonScript>();

				if ( script.color == color ) {
					if (action) { script.SetEnabled(); }
					else { script.SetDisabled(); }
				}
			}
		}
	}

	// ------------------------------------------------------------------------------------------
	public void ActionOnLasers( bool action ) {
		GameObject	LasersContainer				=		GameObject.Find("Electronic_Lasers");

		if (LasersContainer != null) {
			foreach(Transform child in LasersContainer.transform) {
				if ( !child.gameObject.CompareTag( "Electronic_Laser" ) ) { continue; }

				var laser						=		child.gameObject;
				var script						=		laser.GetComponent<LaserScript>();

				if ( script.color == this.color ) {
					if (action) { script.SetEnabled(); }
					else { script.SetDisabled(); }
				}
			}
		}
	}

	// ------------------------------------------------------------------------------------------
	public void ActionOnTeleporters( bool action ) {
		GameObject	TeleportersContainer		=		GameObject.Find("Electronic_Teleporters");

		if (TeleportersContainer != null) {
			foreach(Transform child in TeleportersContainer.transform) {
				var teleporter					=		child.gameObject;
				var script						=		teleporter.GetComponent<TeleporterScript>();

				if ( script.color == this.color ) {
					if (action) { script.SetEnabled(); }
					else { script.SetDisabled(); }
				}
			}
		}
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################