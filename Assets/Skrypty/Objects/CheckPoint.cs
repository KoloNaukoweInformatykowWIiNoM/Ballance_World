using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public enum CheckPointStatus {
	Avalible,
	Active,
	Used
}
// ####################################################################################################
public class CheckPoint : MonoBehaviour {

	public		GameObject				container;	
	public		CheckPointStatus		status;

	// ------------------------------------------------------------------------------------------
	private void Start () {
		UpdateColor();
	}

	// ------------------------------------------------------------------------------------------
	public void ChangeStatus() {
		if ( status == CheckPointStatus.Avalible ) { UpdateAll(); }
		status = CheckPointStatus.Active;
		UpdateColor();
	}

	// ------------------------------------------------------------------------------------------
	public void UpdateColor() {
		if ( status == CheckPointStatus.Active ) { GetComponent<Renderer>().material.color = new Color( 0.443f, 0.725f, 0.968f, 1.000f ); }
		else if ( status == CheckPointStatus.Avalible ) { GetComponent<Renderer>().material.color = new Color( 0.282f, 0.827f, 0.474f, 1.000f ); }
		else if ( status == CheckPointStatus.Used ) { GetComponent<Renderer>().material.color = new Color( 0.827f, 0.282f, 0.282f, 1.000f ); }
	}

	// ------------------------------------------------------------------------------------------
	public void UpdateAll() {

		for ( int iChild=0; iChild < container.transform.childCount; iChild++ ) {
			GameObject child = container.transform.GetChild(iChild).gameObject;

			if ( !child.CompareTag( "Checkpoint" ) ) { continue; }
			var script = child.GetComponent<CheckPoint>();

			if ( script.status == CheckPointStatus.Active ) {
				script.status = CheckPointStatus.Used;
				script.UpdateColor();
			}
		}

	}

	// ------------------------------------------------------------------------------------------
}

// ####################################################################################################