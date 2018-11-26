using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
// 
//	Detekcja zetknięcia się gracza z obiektem interakcyjnym
//	Interakcja gracza z elementami menu głównego
//	Interakcja gracza z objektami (podnoszenie objektów)
//	
// ####################################################################################################
public class Pickups : MonoBehaviour {

	// ------------------------------------------------------------------------------------------
	private void Start()	{ /* nothing to do */ }
	private void Update()	{ /* nothing to do */ }

	// ------------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider detector) {
		GameObject	obj		=	detector.gameObject;

		if ( detector.gameObject.CompareTag("Object_Coin50") )		{ PickupPointable( obj, 50 ); GetComponent<Sounds>().PlaySound_PickupCoin(); }
		if ( detector.gameObject.CompareTag("Object_Coin100") )		{ PickupPointable( obj, 100 ); GetComponent<Sounds>().PlaySound_PickupCoin(); }
		if ( detector.gameObject.CompareTag("Object_Coin200") )		{ PickupPointable( obj, 200 ); GetComponent<Sounds>().PlaySound_PickupCoin(); }
		if ( detector.gameObject.CompareTag("Object_Fruit") )		{ PickupPointable( obj, 400 ); GetComponent<Sounds>().PlaySound_PickupFruit(); }
		if ( detector.gameObject.CompareTag("Object_Diamond") )		{ PickupPointable( obj, 1000 ); GetComponent<Sounds>().PlaySound_PickupDiamond(); }
		if ( detector.gameObject.CompareTag("Object_Hourglass") )	{ PickupTimeable( obj ); }
		if ( detector.gameObject.CompareTag("Object_Key") )			{ PickupKeyable( obj ); }
		if ( detector.gameObject.CompareTag("Object_Life") )		{ PickupLifeable( obj ); }
		if ( detector.gameObject.CompareTag("Checkpoint") )			{ ReachCheckPoint( obj ); }
		if ( detector.gameObject.CompareTag("Finish") )				{ ReachFinish( obj ); }

		if ( detector.gameObject.CompareTag("Menu_NewGame") )		{ GetComponent<GamePlay>().ReachNewGame( obj ); }
		if ( detector.gameObject.CompareTag("Menu_Levels") )		{ GetComponent<GamePlay>().ReachLevels( obj ); }
		if ( detector.gameObject.CompareTag("Menu_Settings") )		{ GetComponent<GamePlay>().ReachSettings( obj, true ); }
		if ( detector.gameObject.CompareTag("Menu_Bonus") )			{ GetComponent<GamePlay>().ReachBonus( obj ); }
		if ( detector.gameObject.CompareTag("Block_Level") )		{ GetComponent<GamePlay>().ReachLevel( obj ); }
	}

	void OnTriggerExit(Collider detector) {
		if ( detector.gameObject.CompareTag("Menu_Settings") )		{ GetComponent<GamePlay>().ReachSettings( detector.gameObject, false ); }
	}

	// ------------------------------------------------------------------------------------------
	private void PickupPointable( GameObject obj, int points ) {
		obj.SetActive( false );
		Destroy( obj );
		GetComponent<GamePlay>().points_player += points;
	}

	// ------------------------------------------------------------------------------------------
	private void PickupTimeable( GameObject obj ) {
		obj.SetActive( false );
		Destroy( obj );
		GetComponent<GamePlay>().points_player += 150;
		GetComponent<GamePlay>().time_player = GetComponent<GamePlay>().time_map - GetComponent<GamePlay>().time_player;
		GetComponent<Sounds>().PlaySound_PickupHourglass();
	}

	// ------------------------------------------------------------------------------------------
	private void PickupKeyable( GameObject obj ) {
		obj.SetActive( false );
		Destroy( obj );
		GetComponent<GamePlay>().points_player += 150;
		GetComponent<GamePlay>().keys_player += 1;
		GetComponent<Sounds>().PlaySound_PickupKeys();
	}

	// ------------------------------------------------------------------------------------------
	private void PickupLifeable( GameObject obj ) {
		obj.SetActive( false );
		Destroy( obj );
		GetComponent<GamePlay>().lives += 1;
		GetComponent<Sounds>().PlaySound_PickupLife();
	}

	// ------------------------------------------------------------------------------------------
	private void ReachFinish( GameObject obj ) {
		GetComponent<Sounds>().SetMotionType( SoundType.None );
		if ( GetComponent<GamePlay>().ReachExit( obj ) ) { return; }
		GetComponent<GamePlay>().DetectWin();
	}

	// ------------------------------------------------------------------------------------------
	private void ReachCheckPoint( GameObject obj ) {
		var		script		=		obj.GetComponent<CheckPoint>();

		if ( script.status == CheckPointStatus.Avalible ) {
			GetComponent<GamePlay>().last_checkpoint = obj.transform.position;
			script.ChangeStatus();
			GetComponent<Sounds>().PlaySound_ReachCheckpoint();
		}
	}

	// ------------------------------------------------------------------------------------------
}

// ####################################################################################################