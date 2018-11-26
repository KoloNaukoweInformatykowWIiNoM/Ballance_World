using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class Rotator : MonoBehaviour {

	public		float		rotateX;
    public		float		rotateY;
	public		float		rotateZ;

	// ------------------------------------------------------------------------------------------
	void Update () {
		Vector3		rotate		=	new Vector3( rotateX, rotateY, rotateZ );

		transform.Rotate( rotate * Time.deltaTime );
	}

}
// ####################################################################################################