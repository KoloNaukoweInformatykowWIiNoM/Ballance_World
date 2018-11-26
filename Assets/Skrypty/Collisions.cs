using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ####################################################################################################
// 
//	Detekcja zetknięcia się gracza z obiektem stałym
//	
// ####################################################################################################
public class Collisions : MonoBehaviour {

	private		bool		teleportable	=		true;

	// ------------------------------------------------------------------------------------------
	private void Start()	{ /* nothing to do */ }
	private void Upadte()	{ /* nothing to do */ }

	// ------------------------------------------------------------------------------------------
	private void OnCollisionEnter(Collision detector) { 

		if ( !GetComponent<Player>().jump_allow && !detector.gameObject.CompareTag("Block_Tracks") ) {
			GetComponent<Sounds>().PlaySound_playerJump();
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Normal Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Block_Field") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Plain );
		}

		if ( detector.gameObject.CompareTag("Block_FieldTransparent") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Plain );
		}

		if ( detector.gameObject.CompareTag("Block_Tracks") ) {
			setJumpAllow( false );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Tracks );
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Special Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Block_FieldCrack") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Plain );
		}

		if ( detector.gameObject.CompareTag("Block_FieldLava") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( true );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Lava );
		}

		if ( detector.gameObject.CompareTag("Block_FieldIceSpeed") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( true );
			GetComponent<Sounds>().SetMotionType( SoundType.Ice );
		}

		if ( detector.gameObject.CompareTag("Block_FieldIceUncontrol") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( true );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Ice );
		}
			
		if ( detector.gameObject.CompareTag("Block_FieldBlink") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Plain );
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Moving Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Block_FieldMove") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.Plain );
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Killing Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Block_FieldSpikes") ) {
			GetComponent<Sounds>().SetMotionType( SoundType.None );
			GetComponent<GamePlay>().MakeFail( EnumFails.Exploded );
			//GetComponent<Sounds>().PlaySound_HitExplode();
		}
			
		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Walls Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Block_Wall") )	{
			GetComponent<Player>().StopMove();
			GetComponent<Sounds>().PlaySound_HitWall();
		}

		if ( detector.gameObject.CompareTag("Block_WallTrack") )	{
			/* nothing to do */
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Electronic Blocks ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		if ( detector.gameObject.CompareTag("Electronic_Button") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.None );

			var script	=	detector.gameObject.GetComponent<ButtonScript>();
			script.Touched();
			script.ActionOnButtons( script.active );
			script.ActionOnTeleporters( script.active );
			script.ActionOnLasers( script.active );

			if ( script.active ) { GetComponent<Sounds>().PlaySound_HitButtonEnable(); }
			else { GetComponent<Sounds>().PlaySound_HitButtonDisable(); }
		}

		if ( detector.gameObject.CompareTag("Electronic_Teleporter") ) {
			setJumpAllow( true );
			setMoveAllow( true );
			setBurning( false );
			setMoveLock( false );
			setSpeedup( false );
			GetComponent<Sounds>().SetMotionType( SoundType.None );

			if (!teleportable) { teleportable = !teleportable; return; }
			var	teleporter		=	detector.gameObject.transform.parent.gameObject;
			if (!teleporter.GetComponent<TeleporterScript>().active) { return; }

			var script			=	teleporter.GetComponent<TeleporterScript>();
			var new_teleporter	=	script.GetNextTeleporter( script.FindNextTeleporter( script.hope + 1 ) );

			if (new_teleporter != null) {
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				transform.position = new_teleporter.GetComponent<TeleporterScript>().GetTeleportedPosition();
				GetComponent<Sounds>().PlaySound_HitTeleporter();
			}
			teleportable = !teleportable;
		}

		if ( detector.gameObject.CompareTag("Electronic_Laser") ) {
			GetComponent<Sounds>().SetMotionType( SoundType.None );
			GetComponent<GamePlay>().MakeFail( EnumFails.Burned );
		}
	}

	// ------------------------------------------------------------------------------------------
	//private void OnCollisionExit(Collision detector) { 
	//}

	// ------------------------------------------------------------------------------------------
	public bool getJumpAllow() { return GetComponent<Player>().jump_allow; }
	public void setJumpAllow( bool value ) { GetComponent<Player>().jump_allow = value; }
	public void setMoveAllow( bool value ) { GetComponent<Player>().move_allow = value; }
	public void setBurning( bool value ) { GetComponent<GamePlay>().burned_active = value; }
	public void setSpeedup( bool value ) { GetComponent<Player>().speed_enable = value; }
	public void setMoveLock( bool value ) { GetComponent<Player>().move_lock = value; }
	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################