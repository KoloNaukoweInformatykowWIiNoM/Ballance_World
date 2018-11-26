using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ####################################################################################################
// 
//	Obsługa elementów UI gry a także menu
//	
// ####################################################################################################
public class UI : MonoBehaviour {

	private		GameObject		player;
	private		GameObject		camera_map;

	private		bool			is_pause			=		false;
	private		bool			is_context			=		false;
	private		bool			is_settings			=		false;
	private		bool			is_cheats			=		false;

	private		string			context_level;
	private		int[]			cheat_array;

	public		GameObject		ui_game;
	public		GameObject		ui_pause;
	public		GameObject		ui_context;
	public		GameObject		ui_settings;
	public		GameObject		ui_cheats;

	public		GameObject		ui_gamekey;
	public		GameObject		ui_gametime;

	public		Button			uiPause_buttonResume;
	public		Button			uiPause_buttonRestart;
	public		Button			uiPause_buttonMenu;
	public		Button			uiPause_buttonExit;

	public		GameObject		uiGame_Lives;
	public		Text			uiGame_textKey;
	public		Text			uiGame_textScore;
	public		Text			uiGame_textTime;
	public		Text			uiGame_textLives;

	public		Text			uiContext_textTitle;
	public		Text			uiContext_textDescription;
	public		Button			uiContext_buttonYes;
	public		Button			uiContext_buttonNo;
	public		Text			uiContext_textYes;
	public		Text			uiContext_textNo;

	public		Text			uiSettings_Level;
	public		Text			uiSettings_Points;
	public		Text			uiSettings_BonusPoints;

	public		Button			uiSettings_resetGame;
	public		Button			uiSettings_buttonSave;

	public		Text			uiCheats_Edit;
	public		Button			uiCheats_Button;

	// ------------------------------------------------------------------------------------------
	private void Start() {
		player		=	GameObject.Find("Player");
		camera_map	=	GameObject.Find("MapCamera");
		camera_map.SetActive( false );

		Freeze( false );
		SetButtonsPause();
	}

	// ------------------------------------------------------------------------------------------
	private void Update() {
		if ( Input.GetKeyDown(KeyCode.Escape) ) { Escape(); }
		if ( Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.T) ) {
			UsingCheats();
		}
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	private void SetButtonsPause() { 
		uiPause_buttonResume.onClick.AddListener( OnClick_ButtonResume );
		uiPause_buttonRestart.onClick.AddListener( OnClick_ButtonRestart );
		uiPause_buttonMenu.onClick.AddListener( OnClick_ButtonMenu );
		uiPause_buttonExit.onClick.AddListener( OnClick_ButtonExit );
		try {
			uiSettings_resetGame.onClick.AddListener( onClick_ButtonResetGame );
			uiSettings_buttonSave.onClick.AddListener( OnClick_ButtonSettingsSave );
		} catch ( System.Exception ) {
			/* This is not main menu */
		}
		uiCheats_Button.onClick.AddListener( onCheatClick );
	}

	private void SetButtonsContext( int mode ) { 
		uiContext_buttonYes.onClick.RemoveAllListeners();
		uiContext_buttonNo.onClick.RemoveAllListeners();

		if ( mode == -1 ) {
			uiContext_textYes.text			=		"Zacznij grę";
			uiContext_textNo.text			=		"Pozostań w menu";
			uiContext_buttonYes.onClick.AddListener( OnClick_ButtonYesWin );
			uiContext_buttonNo.onClick.AddListener( OnCLick_ButtonNoContent );
			return;
		}
		else if ( mode == 0 ) {
			uiContext_textYes.text			=		"Powtórz poziom";
			uiContext_textNo.text			=		"Wróć do menu";
			uiContext_buttonYes.onClick.AddListener( OnClick_ButtonYesLoose );
		}
		else if ( mode == 1 ) {
			uiContext_textYes.text			=		"Graj Dalej";
			uiContext_textNo.text			=		"Wróć do menu";
			uiContext_buttonYes.onClick.AddListener( OnClick_ButtonYesWin );
		}
		else if ( mode == 2 ) {
			uiContext_textYes.text			=		"Bonus";
			uiContext_textNo.text			=		"Wróć do menu";
			uiContext_buttonYes.onClick.AddListener( OnClick_ButtonYesWin );
		}

		uiContext_buttonNo.onClick.AddListener( OnCLick_ButtonNo );
	}

	public void OnClick_ButtonResume() { Escape(); }
	public void OnClick_ButtonRestart() { Debug.Log("XD"); SceneManager.LoadScene( SceneManager.GetActiveScene().name ); }
	public void OnClick_ButtonMenu() { SceneManager.LoadScene( "Menu" ); }
	public void OnClick_ButtonExit() { Application.Quit(); }

	void OnClick_ButtonYesWin() { SceneManager.LoadScene( context_level ); Context (false); }
	void OnClick_ButtonYesLoose() { SceneManager.LoadScene( SceneManager.GetActiveScene().name ); }
	void OnCLick_ButtonNo() { SceneManager.LoadScene( "Menu" ); }

	void OnCLick_ButtonNoContent() { Freeze( false ); Context(false); }

	void OnClick_ButtonSettingsSave() {
		Settings( false );
		Freeze( false );
	}

	void onClick_ButtonResetGame() {
		PlayerPrefs.SetInt( "points" , 0 );
		PlayerPrefs.SetInt( "BonusPoints" , 0 );
		PlayerPrefs.SetInt( "levels" , 1 );
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void ShowContext( int mode, string title, string descritpion, string level ) {
		uiContext_textTitle.text			=		title;
		uiContext_textDescription.text		=		descritpion;
		context_level						=		level;
		SetButtonsContext( mode );
		Context( true );
	}

	// ------------------------------------------------------------------------------------------
	public void Escape() {
		if (is_context) { return; }
		if (is_settings) { return; }

		if (is_cheats) {
			is_cheats = false;
			Freeze( is_cheats );
			ui_cheats.SetActive( is_cheats );
			return;
		}

		is_pause = !is_pause;

		if ( is_pause ) {
			Freeze( true );
			ui_pause.SetActive( true );
			camera_map.SetActive( true );
			camera_map.GetComponent<MapCamera>().SetLook();
			camera_map.GetComponent<MapCamera>().RandomSettings();
		} else {
			camera_map.SetActive( false );
			Freeze( false );
			ui_pause.SetActive( false );
		}
	}

	// ------------------------------------------------------------------------------------------
	public void Settings( bool active ) {
		if (is_context) { return; }
		if (is_pause) { return; }
		if (is_cheats) { return; }
		is_settings		=		active;
		ui_settings.SetActive( active );

		int loadPoints		=	0;
		int loadBonus		=	0;
		int loadLevel		=	0;
		try { loadPoints	=	PlayerPrefs.GetInt( "points" ); } 		catch ( System.Exception ) { loadPoints = 0; }
		try { loadBonus		=	PlayerPrefs.GetInt( "BonusPoints" ); } 	catch ( System.Exception ) { loadBonus = 0; }
		try { loadLevel		=	PlayerPrefs.GetInt( "levels" ); } 		catch ( System.Exception ) { loadLevel = 0; }

		uiSettings_Level.text			=	loadLevel.ToString();
		uiSettings_Points.text			=	loadPoints.ToString();
		uiSettings_BonusPoints.text		=	loadBonus.ToString();
	}

	// ------------------------------------------------------------------------------------------
	public void Context( bool active ) {
		if (is_pause) { return; }
		if (is_settings) { return; }
		if (is_cheats) { return; }
		is_context		=		active;
		ui_context.SetActive( active );
	}

	// ------------------------------------------------------------------------------------------
	public void Freeze( bool value ) {
		if ( value ) {
			//player.SetActive(false);
			Time.timeScale = 0;
		} else {
			//player.SetActive(true);
			Time.timeScale = 1;
		}
	}

	// ------------------------------------------------------------------------------------------
	public void ShowHearts( bool enabled ) {
		uiGame_Lives.SetActive( enabled );
	}

	// ------------------------------------------------------------------------------------------
	public void UpdateUI( string key, string score, string time, string lives ) {
		uiGame_textKey.text		=	key;
		uiGame_textScore.text	=	score;
		uiGame_textTime.text	=	time;
		uiGame_textLives.text	=	lives;
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void UsingCheats() {
		if (is_pause) { return; }
		if (is_settings) { return; }
		if (is_context) { return; }

		is_cheats = true;
		Freeze( is_cheats );
		ui_cheats.SetActive( is_cheats );
	}

	public void onCheatClick() {
		string		cheat		=		uiCheats_Edit.text.ToLower();
		int			length		=		uiCheats_Edit.text.Length;

		//Debug.Log( cheat.Substring( 1, 5 ) );
		if ( cheat.Substring( 0, 10 ) == "/cheat.win" ) {
			is_cheats = false;
			Freeze( is_cheats );
			ui_cheats.SetActive( is_cheats );
			SetButtonsContext( 1 );
			GameObject.Find("Player").gameObject.GetComponent<GamePlay>().ShowWin();
			return;
		}

		else if ( cheat.Substring( 0, 11 ) == "/cheat.load" ) {
			string	level	=	getCheatString( cheat, 12 );
			try { SceneManager.LoadScene( level ); } catch( System.Exception ) { /* This is invalid level name */ }
		}

		else if ( cheat.Substring( 0, 11 ) == "/cheat.time" ) {
			int		time	=	getCheatInt( cheat, 12 );
			GameObject.Find("Player").gameObject.GetComponent<GamePlay>().time_player = time;
		}

		else if ( cheat.Substring( 0, 11 ) == "/cheat.keys" ) {
			int		keys	=	getCheatInt( cheat, 12 );
			GameObject.Find("Player").gameObject.GetComponent<GamePlay>().keys_player = keys;
		}

		else if ( cheat.Substring( 0, 12 ) == "/cheat.reset" ) {
			onClick_ButtonResetGame();
		}

		else if ( cheat.Substring( 0, 12 ) == "/cheat.loose" ) {
			int			loose	=	getCheatInt( cheat, 13 );
			EnumFails	fail	=	EnumFails.TimeOut;

			if ( loose <= 0 ) { fail = EnumFails.Burned; }
			if ( loose == 1 ) { fail = EnumFails.EndOfLives; }
			if ( loose == 2 ) { fail = EnumFails.Exploded; }
			if ( loose >= 3 ) { fail = EnumFails.FallOff; }

			is_cheats = false;
			Freeze( is_cheats );
			ui_cheats.SetActive( is_cheats );
			GameObject.Find("Player").gameObject.GetComponent<GamePlay>().ShowLoose( fail );
			return;
		}

		else if ( cheat.Substring( 0, 13 ) == "/cheat.reload" ) {
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
		}

		else if ( cheat.Substring( 0, 13 ) == "/cheat.levels" ) {
			int		level	=	getCheatInt( cheat, 14 );
			PlayerPrefs.SetInt( "levels", level );
		}

		else if ( cheat.Substring( 0, 13 ) == "/cheat.points" ) {
			int		points	=	getCheatInt( cheat, 14 );
			GameObject.Find("Player").gameObject.GetComponent<GamePlay>().points_player = points;
		}

		else if ( cheat.Substring( 0, 15 ) == "/cheat.position" ) {
			int[]	array	=	getCheatValues( cheat, 16, 3 );
			GameObject.Find("Player").transform.position = new Vector3( array[0], array[1], array[2] );
		}


		else if ( cheat.Substring( 0, 18 ) == "/cheat.bonuspoints" ) {
			int		points	=	getCheatInt( cheat, 19 );
			PlayerPrefs.SetInt( "BonusPoints", points );
		}

		else if ( cheat.Substring( 0, 19 ) == "/cheat.pointsglobal" ) {
			int		points	=	getCheatInt( cheat, 20 );
			PlayerPrefs.SetInt( "points", points );
		}
			
		is_cheats = false;
		Freeze( is_cheats );
		ui_cheats.SetActive( is_cheats );
	}

	// ------------------------------------------------------------------------------------------
	public int[] getCheatValues( string cheat, int start, int count ) {
		int[]	array	=	new int[count];
		int		c		=	0;
		string	getit	=	"";

		for (int i = start; i < cheat.Length; i++) {
			if ( cheat.Substring( i, 1 ) == " " ) {
				if ( getit == "" ) { continue; }
				array[c] = int.Parse( getit );
				getit = "";
				c++;
				continue;
			}

			getit = getit + cheat.Substring( i, 1 );
		}

		if (getit != "") { array[c] = int.Parse( getit ); }
		return array;
	}

	// ------------------------------------------------------------------------------------------
	public int getCheatInt( string cheat, int start ) {
		int 	value	=	0;
		string	getit	=	"";

		for (int i = start; i < cheat.Length; i++) {
			if ( cheat.Substring( i, 1 ) == " " ) { break; }
			getit = getit + cheat.Substring( i, 1 );
		}

		if (getit != "") { value = int.Parse( getit ); }
		return value;
	}

	// ------------------------------------------------------------------------------------------
	public string getCheatString( string cheat, int start ) {
		string text = cheat.Substring( start, cheat.Length-start );
		Debug.Log( text );
		return text;
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void SetKeyColorNormal()		{ ui_gamekey.GetComponent<Image>().color	=	new Color( 0.000f, 0.000f, 0.000f, 0.501f ); }
	public void SetKeyColorGreen()		{ ui_gamekey.GetComponent<Image>().color	=	new Color( 0.192f, 0.807f, 0.180f, 0.501f ); }
	public void SetTimeColorNormal()	{ ui_gametime.GetComponent<Image>().color	=	new Color( 0.000f, 0.000f, 0.000f, 0.501f ); }
	public void SetTimeColorYellow()	{ ui_gametime.GetComponent<Image>().color	=	new Color( 0.882f, 0.882f, 0.125f, 0.501f ); }
	public void SetTimeColorRed()		{ ui_gametime.GetComponent<Image>().color	=	new Color( 0.807f, 0.180f, 0.192f, 0.501f ); }
	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################