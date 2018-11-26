using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ####################################################################################################
//public enum MusicType {
//	Levels01_03_HerSmile,
//	Levels04_06_LoveStory,
//	Levels07_08_WalkedAway,
//	Levels09_11_Naive,
//	Levels12_14_Karlita,
//	Levels15_17_AtSea,
//	Levels18_19_Weeks,
//	Levels20_22_Everdream
//}

public enum SoundType {
	None,
	Plain,
	Tracks,
	Lava,
	Ice
}
// ##################### ###############################################################################
public class Sounds : MonoBehaviour {

	private		AudioSource		sound_effects;
	private		AudioSource		sound_movement;
	private		AudioSource		sound_music;

	private		bool			music_enable			=	true;
	public		int				music;
	public		AudioClip[]		musics;
	public		bool			music_continue;
	private		int				load_continue;
	private		float			load_position;

	public		AudioClip		pickupCollect_Coin;
	public		AudioClip		pickupCollect_Diamond;
	public		AudioClip		pickupCollect_Fruit;
	public		AudioClip		pickupCollect_Hourglass;
	public		AudioClip		pickupCollect_Key;
	public		AudioClip		pickupCollect_Life;
	public		AudioClip		pickupObject_Checkpoint;

	public		AudioClip		playerMove_Plain;
	public		AudioClip		playerMove_Tracks;
	public		AudioClip		playerMove_Lava;
	public		AudioClip		playerMove_Ice;
	public		AudioClip		playerJump;

	public		AudioClip		playerHit_Walls;
	public		AudioClip		playerHit_ButtonEnable;
	public		AudioClip		playerHit_ButtonDisable;
	public		AudioClip		playerHit_Teleporter;
	public		AudioClip		playerHit_Explode;

	public		AudioClip		objectMover_Move;
	public		AudioClip		objectCrash_Crash;

	public		AudioClip		environment_Start;
	public		AudioClip		environment_Loose;
	public		AudioClip		environment_Win;

	public		float			volumeSounds			=		0.75f;
	public		float			volumeMusic				=		0.5f;

	private		SoundType		soundMotion_type		=		SoundType.None;
	private		float			soundMotion_sensitive	=		0.75f;

	// ------------------------------------------------------------------------------------------
	private void Start () {
		music_enable			=	false;
		sound_effects			=	transform.GetChild(0).gameObject.GetComponent<AudioSource>();
		sound_movement			=	transform.GetChild(1).gameObject.GetComponent<AudioSource>();
		sound_movement.volume	=	(volumeSounds / 4)*3;
		sound_music				=	transform.GetChild(2).gameObject.GetComponent<AudioSource>();

		LoadMusicData();
		string		scene		=		SceneManager.GetActiveScene().name;
		if ( scene != "Menu" && scene != "Levels" ) { PlaySound_environmentStart(); }
	}

	// ------------------------------------------------------------------------------------------
	private void Update() {
		Vector3	move_sensor				=		GetComponent<Rigidbody>().velocity;
		float soundMotion_activeP		=		Mathf.Max( new float[] { move_sensor.x, move_sensor.y, move_sensor.z } );
		float soundMotion_activeM		=		Mathf.Min( new float[] { move_sensor.x, move_sensor.y, move_sensor.z } );
		//Debug.Log ( move_sensor + " Max of this is: " + soundMotion_active );
		PlayMusic();
		PlayMoitionSound( soundMotion_activeP, soundMotion_activeM );
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void LoadMusicData() {
		sound_music.clip	=	musics[music];
		sound_music.volume	=	volumeMusic;

		load_continue		=	0;
		load_position		=	0.0f;

		try {
			load_continue = PlayerPrefs.GetInt( "continue_music" );
			PlayerPrefs.SetInt( "continue_music", 0 );
		} catch ( System.Exception ) { /* nothing to do */ }

		try {
			load_position = PlayerPrefs.GetFloat( "position_music" );
			PlayerPrefs.SetFloat( "position_music", 0.0f );
		} catch ( System.Exception ) { /* nothing to do */ }
			
		if ( load_continue >= 1 ) { sound_music.time = load_position; }

		sound_music.Play();
		music_enable 		=	true;
	}

	public void SaveMusicData( bool continue_play ) {
		if (continue_play) {
			PlayerPrefs.SetInt( "continue_music", 1 );
			PlayerPrefs.SetFloat( "position_music", sound_music.time );
		} else {
			PlayerPrefs.SetInt( "continue_music", 0 );
			PlayerPrefs.SetFloat( "position_music", 0.0f );
		}
	}

	private void PlayMusic() {
		if ( !music_enable ) { return; }
		if ( sound_music.isPlaying == false ) { sound_music.time = 0.0f; sound_music.Play(); }
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void SetMotionType( SoundType sound_type ) {
		soundMotion_type = sound_type;

		switch (soundMotion_type) {
		case SoundType.None:
			sound_movement.clip = null;
			break;
		case SoundType.Plain:
			sound_movement.clip = playerMove_Plain;
			break;
		case SoundType.Tracks:
			sound_movement.clip = playerMove_Tracks;
			break;
		case SoundType.Ice:
			sound_movement.clip = playerMove_Ice;
			break;
		case SoundType.Lava:
			sound_movement.clip = playerMove_Lava;
			break;
		}
	}

	private void PlayMoitionSound( float soundMotion_activeP, float soundMotion_activeM ) {
		if ( sound_movement.isPlaying ) {
			if ( soundMotion_type == SoundType.None ) { sound_movement.Stop(); }
			if ( soundMotion_activeM >= (-soundMotion_sensitive) && soundMotion_activeP <= soundMotion_sensitive ) { sound_movement.Stop(); }
		} else {
			if ( soundMotion_activeP > soundMotion_sensitive || soundMotion_activeM < (-soundMotion_sensitive) ) { sound_movement.Play(); }
			else { sound_movement.Stop(); }
		}
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void PlaySound_PickupCoin()			{ sound_effects.PlayOneShot( pickupCollect_Coin,		volumeSounds ); }
	public void PlaySound_PickupDiamond() 		{ sound_effects.PlayOneShot( pickupCollect_Diamond,		volumeSounds ); }
	public void PlaySound_PickupFruit()			{ sound_effects.PlayOneShot( pickupCollect_Fruit,		volumeSounds ); }
	public void PlaySound_PickupHourglass()		{ sound_effects.PlayOneShot( pickupCollect_Hourglass,	volumeSounds ); }
	public void PlaySound_PickupKeys() 			{ sound_effects.PlayOneShot( pickupCollect_Key,			volumeSounds ); }
	public void PlaySound_PickupLife() 			{ sound_effects.PlayOneShot( pickupCollect_Life,		volumeSounds ); }

	public void PlaySound_HitWall()				{ sound_effects.PlayOneShot( playerHit_Walls,			volumeSounds ); }
	public void PlaySound_HitButtonEnable()		{ sound_effects.PlayOneShot( playerHit_ButtonEnable,	volumeSounds ); }
	public void PlaySound_HitButtonDisable()	{ sound_effects.PlayOneShot( playerHit_ButtonDisable,	volumeSounds ); }
	public void PlaySound_HitTeleporter()		{ sound_effects.PlayOneShot( playerHit_Teleporter,		volumeSounds*1.25f ); }
	public void PlaySound_HitExplode()			{ sound_effects.PlayOneShot( playerHit_Explode,			volumeSounds ); }

	public void PlaySound_ReachCheckpoint()		{ sound_effects.PlayOneShot( pickupObject_Checkpoint,	volumeSounds ); }

	public void PlaySound_playerJump()			{ sound_effects.PlayOneShot( playerJump,				volumeSounds ); }
	public void PlaySound_objectMover()			{ sound_effects.PlayOneShot( objectMover_Move,			volumeSounds ); }
	public void PlaySound_objectCrash()			{ sound_effects.PlayOneShot( objectCrash_Crash,			volumeSounds*1.25f ); }

	public void PlaySound_environmentStart()	{ sound_effects.PlayOneShot( environment_Start,			volumeSounds ); }
	public void PlaySound_environmentLoose()	{ /*sound_effects.PlayOneShot( environment_Loose,		volumeSounds/8 );*/ }
	public void PlaySound_environmentWin()		{ sound_effects.PlayOneShot( environment_Win,			volumeSounds ); }
	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################