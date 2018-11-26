using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class LaserScript : MonoBehaviour {

	public		bool				active				=		false;
	public		ElectronicColor		color;

	public		float				light_range			=		1.0f;
	public		int					light_strange		=		3;

	// ------------------------------------------------------------------------------------------
	void Start () {
		Setup();
		if (active) { SetEnabled(); } else { SetDisabled(); }
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
			GameObject		children							=		transform.GetChild(iChildren).gameObject;
			children.GetComponent<Renderer>().material.color	=		col;

			for ( int iLight = 0; iLight < children.transform.childCount; iLight++ ) {
				var light										=		children.transform.GetChild(iLight).gameObject;
				light.GetComponent<Light>().color				=		col;
				light.GetComponent<Light>().range				=		light_range;
				light.GetComponent<Light>().intensity			=		light_strange;
			}
		}
	}
	
	// ------------------------------------------------------------------------------------------
	public void SetEnabled() {
		active		=		true;

		for ( int iChildren = 0; iChildren < transform.childCount; iChildren++ ) {
			GameObject		children						=		transform.GetChild(iChildren).gameObject;
			children.GetComponent<MeshRenderer>().enabled	=		true;
			children.GetComponent<MeshCollider>().enabled	=		true;

			for ( int iLight = 0; iLight < children.transform.childCount; iLight++ ) {
				var light									=		children.transform.GetChild(iLight).gameObject;
				light.GetComponent<Light>().range			=		light_range;
				light.GetComponent<Light>().intensity		=		light_strange;
			}
		}
	}

	// ------------------------------------------------------------------------------------------
	public void SetDisabled() {
		active		=		false;

		for ( int iChildren = 0; iChildren < transform.childCount; iChildren++ ) {
			GameObject		children						=		transform.GetChild(iChildren).gameObject;
			children.GetComponent<MeshRenderer>().enabled	=		false;
			children.GetComponent<MeshCollider>().enabled	=		false;

			for ( int iLight = 0; iLight < children.transform.childCount; iLight++ ) {
				var light									=		children.transform.GetChild(iLight).gameObject;
				light.GetComponent<Light>().range			=		0;
				light.GetComponent<Light>().intensity		=		0;
			}
		}
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################