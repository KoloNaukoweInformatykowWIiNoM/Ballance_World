using UnityEngine;
using System.Collections;

// ####################################################################################################
public class Colors : MonoBehaviour {

	public		Color[]		colors;

	public		int			currentIndex		=		0;
	private		int			nextIndex;

	public		float		changeColourTime	=	2.0f;

	private		float		lastChange			=	0.0f;
	private		float		timer				=	0.0f;

	// ------------------------------------------------------------------------------------------
	void Start() {
		if (colors == null || colors.Length < 2)
		nextIndex			=	(currentIndex + 1) % colors.Length;    
	}

	// ------------------------------------------------------------------------------------------
	void Update() {
		timer += Time.deltaTime;

		if (timer > changeColourTime) {
			currentIndex	=	(currentIndex + 1) % colors.Length;
			nextIndex		=	(currentIndex + 1) % colors.Length;
			timer			=	0.0f;

		}
		GetComponent<Renderer>().material.color = Color.Lerp(colors[currentIndex], colors[nextIndex], timer / changeColourTime );
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################