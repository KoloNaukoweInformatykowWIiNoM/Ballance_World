using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ####################################################################################################
// 
//	Odmierzanie czasu
//	Wykrywanie wygranej i przegranej
//	Obsługa poziomów gry
//	Zapis i odczyt danych gry
//	
// ####################################################################################################
public enum EnumFails { Burned, EndOfLives, Exploded, FallOff, TimeOut }
// ####################################################################################################
public class GamePlay : MonoBehaviour {

	private		bool			lock_settings		=		false;
	public		GameObject		ui;
	public		string			next_level			=		"Menu";

	public		bool			lives_mode			=		false;
	public		int				lives				=		3;
	public		Vector3			last_checkpoint;

	private		int				levels_game			=		1;
	private		int				points_game			=		0;
	public		int				points_player		=		0;

	public		int				keys_map			=		0;
	public		int				keys_player			=		0;

	public		bool			time_enable			=		false;
	public		float			time_map			=		90.0f;
	public		float			time_player			=		0.0f;

	public		int				falloff_height		=		-5;

	public		bool			burned_active		=		false;
	private		float			burned_time			=		0.0f;
	private		float			burned_timeout		=		3.0f;

	// ------------------------------------------------------------------------------------------
	private void Start() {
		time_player			=	time_map;
		burned_timeout		=	3.0f;
		last_checkpoint		=	transform.position;
			
		if ( lives_mode ) { ui.GetComponent<UI>().ShowHearts( lives_mode ); }

		LoadPlayerStats();
		PrepareLevels();
	}

	// ------------------------------------------------------------------------------------------
	private void Update() {
		Timer();
		Burn();
		Frost();
		KeysColor();

		DetectFail();
		UpdateUI();
	}

	// ------------------------------------------------------------------------------------------
	public void Burn() {
		if ( burned_active ) {
			if ( burned_time < burned_timeout ) {
				GetComponent<Player>().MakeColorBurn();
				burned_time = burned_time + Time.deltaTime;
			}
		} else {
			if ( burned_time > 0 ) {
				GetComponent<Player>().MakeColorNormal();
				burned_time = burned_time - Time.deltaTime;
			}
		}
	}

	// ------------------------------------------------------------------------------------------
	public void Frost() {
		if ( GetComponent<Player>().move_lock ) {
			GetComponent<Player>().MakeColorFrost();
		} else {
			if ( burned_time <= 0 ) { GetComponent<Player> ().MakeColorNormal (); }
		}
	}

	// ------------------------------------------------------------------------------------------
	private void Timer() {
		if ( time_enable ) {
			if ( time_player > 0 ) {
				time_player = time_player - Time.deltaTime;
			}

			if ( time_player < 10 ) { ui.GetComponent<UI>().SetTimeColorRed(); }
			else if ( time_player < time_map / 4 ) { ui.GetComponent<UI>().SetTimeColorYellow(); }
			else { ui.GetComponent<UI>().SetTimeColorNormal(); }
		}
	}

	// ------------------------------------------------------------------------------------------
	private void KeysColor() {
		if ( keys_player == keys_map ) { ui.GetComponent<UI>().SetKeyColorGreen(); }
	}

	// ------------------------------------------------------------------------------------------
	public void DetectWin() {
		if ( keys_player == keys_map ) {
			if ( SceneManager.GetActiveScene().name == "Bonus" ) { SaveBonusStats(); return; }
			else { SavePlayerStats(); }
			ShowWin();
		}
	}

	// ------------------------------------------------------------------------------------------
	public void MakeFail( EnumFails fail ) {
		
		if ( lives_mode ) {
			lives--;
			transform.position = last_checkpoint;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			if ( lives < 0 ) { ShowLoose( EnumFails.EndOfLives ); }
		} else {
			ShowLoose( fail );
		}

	}

	// ------------------------------------------------------------------------------------------
	private void DetectFail() {

		if ( lives_mode ) {
			if ( burned_time >= burned_timeout ) {
				burned_time = 3.0f;
				lives--;
				transform.position = last_checkpoint;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				if ( lives < 0 ) { ShowLoose( EnumFails.EndOfLives ); }
			}
			if ( transform.position.y <= falloff_height ) {
				lives--;
				transform.position = last_checkpoint;	
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				if ( lives < 0 ) { ShowLoose( EnumFails.EndOfLives ); }
			}
			if ( time_player < 0 ) {
				time_player			=	time_map;
				lives--;
				transform.position = last_checkpoint;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				if ( lives < 0 ) { ShowLoose( EnumFails.EndOfLives ); }
			}	
		} else {
			if ( burned_time >= burned_timeout ) { ShowLoose( EnumFails.Burned ); }
			if ( transform.position.y <= falloff_height ) { ShowLoose( EnumFails.FallOff ); }
			if ( time_player <= 0 ) { ShowLoose( EnumFails.TimeOut ); }
		}

	}

	// ------------------------------------------------------------------------------------------
	public void ShowWin() {
		string		title			=	"Gratulacje";
		string		description		=	"Poziom zakończony!";
		int			mode			=	1;

		if (next_level == "Bonus") {
			title					=	"Zakończono Grę!";
			description				=	"W menu czeka na Ciebie bonus!";
			mode					=	2;
		}

		GetComponent<Sounds>().PlaySound_environmentWin();
		GetComponent<Sounds>().SaveMusicData( GetComponent<Sounds>().music_continue );
		ui.GetComponent<UI>().Freeze( true );
		ui.GetComponent<UI>().ShowContext( mode, title, description, next_level );
	}

	// ------------------------------------------------------------------------------------------
	public void ShowLoose( EnumFails fail ) {
		string		title			=	"";
		string		description		=	"";

		switch ( fail ) {
			case EnumFails.Burned:
				title				=	"Spalony";
				description			=	"Piłka się spaliła.";
				GetComponent<Sounds>().PlaySound_HitExplode();
				break;
			case EnumFails.EndOfLives:
				title				=	"Koniec życia";
				description			=	"Koniec gry :(";
				break;
			case EnumFails.Exploded:
				title				=	"Przebity";
				description			=	"Piłka została przebita.";
				break;
			case EnumFails.FallOff:
				title				=	"Poza światem";
				description			=	"Piłka wypadła poza świat.";
				break;
			case EnumFails.TimeOut:
				title				=	"Koniec czasu";
				description			=	"Dostępny czas się skończył.";
				break;
		}

		if ( SceneManager.GetActiveScene().name == "Menu" )		{ description	=	"Serio?\nWypadłeś poza świat W menu?"; }
		if ( SceneManager.GetActiveScene().name == "Levels" )	{ description	=	"Serio?\nWypadłeś przy wyborze poziomu?"; }
		if ( SceneManager.GetActiveScene().name == "Bonus" )	{ /* nothing to do */ }

		GetComponent<Sounds>().PlaySound_environmentLoose();
		GetComponent<Sounds>().SaveMusicData( true );
		ui.GetComponent<UI>().Freeze( true );
		ui.GetComponent<UI>().ShowContext( 0, title, description, SceneManager.GetActiveScene().name );
	}

	// ------------------------------------------------------------------------------------------
	private void UpdateUI() {
		string		string_lives	=	lives.ToString();
		string		string_key		=	keys_player.ToString() + " / " + keys_map.ToString();
		string		string_score	=	points_player.ToString();
		string		string_time		=	((int)(((int)time_player)/60)).ToString() + ":";
		int			sec				=	((int)time_player) % 60;

		if (sec < 10) { string_time = string_time + "0" + sec.ToString(); }
		else { string_time = string_time + sec.ToString(); }
		ui.GetComponent<UI>().UpdateUI( string_key, string_score, string_time, string_lives );
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	public void ReachNewGame( GameObject obj ) {
		SceneManager.LoadScene( "Level_01" );
		GetComponent<Sounds>().SaveMusicData( true );
	}

	// ------------------------------------------------------------------------------------------
	public void ReachLevels( GameObject obj ) {
		SceneManager.LoadScene( "Levels" );
		GetComponent<Sounds>().SaveMusicData( true );
	}
		
	// ------------------------------------------------------------------------------------------
	public void ReachLevel( GameObject obj ) {
		GameObject		parent		=		obj.transform.parent.gameObject;
		string	level_number		=	parent.GetComponent<TextMesh>().text;

		try { if ( int.Parse( level_number ) <= 3 ) { GetComponent<Sounds>().SaveMusicData( true ); }
		} catch ( System.Exception ) { /* nothing to do */ }

		SceneManager.LoadScene( "Level_" + level_number );
	}

	// ------------------------------------------------------------------------------------------
	public void ReachSettings( GameObject obj, bool can_open ) {
		if (!lock_settings) {
			ui.GetComponent<UI>().Settings( true );
			ui.GetComponent<UI>().Freeze( true );
		}

		if (!can_open) { ui.GetComponent<UI>().Settings( false ); }
		lock_settings	=	can_open;
	}

	// ------------------------------------------------------------------------------------------
	public void ReachBonus( GameObject obj ) {
		if (levels_game >= 23) {
			SceneManager.LoadScene ("Bonus");
		} else {
			ui.GetComponent<UI>().Freeze( true );
			ui.GetComponent<UI>().ShowContext( -1, "Bonus nie dostępny", "Najpierw przejdź grę", "Level_01" );
		}
	}

	// ------------------------------------------------------------------------------------------
	public bool ReachExit( GameObject obj ) {
		if ( SceneManager.GetActiveScene().name == "Menu" )		{ Application.Quit(); return true; }
		if ( SceneManager.GetActiveScene().name == "Levels" )	{
			GetComponent<Sounds>().SaveMusicData( true );
			SceneManager.LoadScene( "Menu" );
			return true;
		}

		if ( SceneManager.GetActiveScene().name == "Bonus" )	{
			DetectWin();
			SceneManager.LoadScene( "Menu" );
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------
	private void LoadPlayerStats() {
		try { points_game =	PlayerPrefs.GetInt( "points" ); }
		catch ( System.Exception ) { points_game = 0; }

		try { levels_game =	PlayerPrefs.GetInt( "levels" ); }
		catch ( System.Exception ) { levels_game = 0; }

		int zoomINT = 20;
		try {  zoomINT = PlayerPrefs.GetInt( "zoom" ); }
		catch ( System.Exception ) { zoomINT = 20; }

		GameObject.Find("ContainerCamera").GetComponent<Camera>().zoom	=	zoomINT;
	}

	private void SavePlayerStats() {
		//int			levelLOAD		=		0;
		//try { levelLOAD = PlayerPrefs.GetInt( "levels" ); }
		//catch ( System.Exception ) { levelLOAD = 0; }

		string		levelSTR		=		SceneManager.GetActiveScene().name.Substring( 6, 2 );
		int			levelINT		=		int.Parse( levelSTR );

		PlayerPrefs.SetInt( "Zoom", GameObject.Find("ContainerCamera").GetComponent<Camera>().zoom );
		if ( levels_game > levelINT ) { return; }
		PlayerPrefs.SetInt( "points", (points_game + points_player) );
		PlayerPrefs.SetInt( "levels", levelINT+1 );
	}

	private void SaveBonusStats() {
		int			levelLOAD		=		0;

		PlayerPrefs.SetInt( "Zoom", GameObject.Find("ContainerCamera").GetComponent<Camera>().zoom );
		PlayerPrefs.SetInt( "BonusPoints", (points_game + points_player) );
	}

	// ------------------------------------------------------------------------------------------
	private void PrepareLevels() {
		if ( SceneManager.GetActiveScene().name != "Levels" ) { return; }
		for (int i=1; i<=22; i++) {
			string		level_number	=	i.ToString();

			if ( i < 10 ) { level_number = "0" + level_number; }
			GameObject	text_object		=	GameObject.Find( "Level_" + level_number );

			if ( i <= levels_game ) { text_object.SetActive( true ); }
			else { text_object.SetActive( false ); }
		}
	}

	// ------------------------------------------------------------------------------------------
}
// ####################################################################################################