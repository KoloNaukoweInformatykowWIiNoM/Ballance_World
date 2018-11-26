using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class BeamScript : MonoBehaviour {

	public		ElectronicColor		color;

	// ------------------------------------------------------------------------------------------
	void Start () {
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
			
		for ( int iChildren = 0; iChildren < transform.childCount; iChildren++ ) {
			GameObject		children					=		transform.GetChild(iChildren).gameObject;
			Material[]		c_materials					=		children.GetComponent<Renderer>().materials;
			c_materials[1].color						= 		col;
		}
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################