using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
public class Blinker : MonoBehaviour {

	public		bool		visibility			=		true;
	private		bool		anime				=		false;

	public		float		time_visible		=		3f;
	public		float		time_invisible		=		3f;
	public		float		time_animation		=		2f;

	public		float		shift				=		0f;

	private		float		time				=		0f;

	private		float		anime_time			=		0f;
	private		float		anime_div			=		0f;
	public		int			anime_fragments		=		5;

	// ------------------------------------------------------------------------------------------
	void Start () {
		time			=		shift;
		if (!visibility) { HideIM(); }
	}
	
	// ------------------------------------------------------------------------------------------
	void Update () {
		if ( visibility ) {
			time += Time.deltaTime;
			if ( time > time_visible ) {
				if ( !anime ) { AnimationStart(); }
				else { Animation(); }
			}
		} else {
			time += Time.deltaTime;
			if ( time > time_invisible ) { ShowIM(); }
		}
	}

	// ------------------------------------------------------------------------------------------
	void AnimationStart() {
		anime			=		true;
		anime_time		=		0f;
		anime_div		=		time_animation / anime_fragments;
	}

	void Animation() {
		anime_time += Time.deltaTime;

		for ( int i=0; i<=anime_fragments; i++ ) {

			if ( anime_time > (anime_div*(i-1)) && anime_time <= (anime_div*i) ) {
				if ( i%2 == 0 ) { GetComponent<MeshRenderer>().enabled = false; }
				else { GetComponent<MeshRenderer>().enabled = true; }
				break;
			}
		}

		if ( anime_time	> time_animation ) {
			anime		=		false;
			HideIM();
		}
	}

	// ------------------------------------------------------------------------------------------
	void ShowIM() {
		visibility		=		true;
		time			=		0f;

		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<BoxCollider>().enabled = true;
		//gameObject.SetActive( true );
	}

	void HideIM() {
		visibility		=		false;
		time			=		0f;

		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		//gameObject.SetActive( false );
	}
}

// ####################################################################################################