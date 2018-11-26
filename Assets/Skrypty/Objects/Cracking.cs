using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class Cracking : MonoBehaviour {

	private		bool		enable_cracking		=		false;
	private		float		time_cracking		=		0.0f;
	public		float		time_crackingMax	=		1.5f;

	// ------------------------------------------------------------------------------------------
	private void Start() {
		time_cracking		=	0.0f;
	}

	// ------------------------------------------------------------------------------------------
	private void Update() {
		if ( enable_cracking ) { time_cracking += Time.deltaTime; }
		if ( time_cracking > time_crackingMax ) { Crack(); }
	}

	// ------------------------------------------------------------------------------------------
	private void OnCollisionEnter(Collision detector) { 
		if ( detector.gameObject.CompareTag("Player") ) { enable_cracking = true; }
	}

	// ------------------------------------------------------------------------------------------
	private void OnCollisionExit(Collision detector) { 
		if ( detector.gameObject.CompareTag("Player") ) {
			if ( enable_cracking ) { Crack(); }
		}
	}

	// ------------------------------------------------------------------------------------------
	private void Crack() {
		GameObject.Find("Player").GetComponent<Sounds>().PlaySound_objectCrash();
		Destroy(gameObject);
	}
	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################