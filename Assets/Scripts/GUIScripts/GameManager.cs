﻿using UnityEngine;
using System.Collections;

/**
 * Handles game level loading, StartGame, EndGame, Pause, and GameModes
 * @author Christian Gunderman
 */
public class GameManager : MonoBehaviour {
	
	/** The sound played when the player dies */
	public AudioClip deathSound;

	public bool[,] teamTowersDead = new bool[2,3];
	
	/** Saves a static reference to instance ... sloppy I know, I don't care */
	private static GameManager instance;
	/** Stores the current GameMode (pause, play, dead, menu etc. */
	private static GameMode mode;
	
	/**
	 * Script initialization. This is called by unity on object creation.
	 */
	void Start () {
		instance = this;
		Pause ();
		mode = GameMode.StartMenu;

		// initiallizes all towers for each team as not dead
		for(int i = 0; i < teamTowersDead.GetLength(0); i++){
			for(int j = 0; j < teamTowersDead.GetLength(1); j++){
				teamTowersDead[i, j] = false;
			}
		}
	}
	
	/**
	 * Called once per frame by unity. Handles toggling of paused and play.
	 */
	void Update () {
		// guard case, exit function if this isn't a pause/unpause situation
		if (!Input.GetKeyDown ("escape")) {
			return;
		}
		
		// toggle game paused state
		switch(GameManager.GetGameMode()) {
		case GameMode.Paused:
			UnPause();
			break;
		case GameMode.UnPaused:
			Pause();
			break;
		}
	}

	/**
	 * Called when a tower has died
	 * flags that the tower from that team has died
	 * checks all towers for that team, if one is alive the method returns
	 * if all towers are dead, the loop is exited and the level is ended
	 * @param the team the tower was from
	 * @param the Element type the tower was
	 */
	public void TowerDied(int team, int towerType){
		teamTowersDead[team, towerType] = true;
		// checks all tower flags for the team
		for(int i = 0; i < teamTowersDead.GetLength(team); i++){
			// if one tower is alive, the method returns
			if(!teamTowersDead[team, i]){
				return;
			}
		}
		// if the method reaches this point then the teams towers have been destroyed
		EndLevel();
	}
	
	/**
	 * Checks if the game is paused.
	 * @return Returns true if paused, false if not.
	 */
	public static bool IsPaused() {
		return mode != GameMode.UnPaused;
	}
	
	/**
	 * Gets the GameMode of the current game. See GameMode enum definition
	 * for possible states.
	 * @return The current game mode.
	 */
	public static GameMode GetGameMode() {
		return mode;
	}

	/**
	 * Brings up game setup screen
	 */
	public static void SetupGame () {
		mode = GameMode.GameSetup;
	}
	
	/**
	 * Pauses the game.
	 */
	public static void Pause() {
		mode = GameMode.Paused;
		GameObject thisPlayer = NetworkingScript.GetThisPlayer ();
		if(thisPlayer != null) {
			thisPlayer.GetComponent<MouseLook> ().enabled = false;
			foreach(FPSInputController obj in thisPlayer.GetComponentsInChildren<FPSInputController> ()) {
				obj.enabled = false;
			}
			thisPlayer.GetComponent<PlayerControler> ().enabled = false;
		}

		// show mouse
		Screen.lockCursor = false;
		Screen.showCursor = true;
	}
	
	/**
	 * Unpauses the game.
	 */
	public static void UnPause() {
		mode = GameMode.UnPaused;
		GameObject thisPlayer = NetworkingScript.GetThisPlayer ();
		if(thisPlayer != null) {
			thisPlayer.GetComponent<MouseLook> ().enabled = true;
			foreach(FPSInputController obj in thisPlayer.GetComponentsInChildren<FPSInputController> ()) {
				obj.enabled = true;
			}
			thisPlayer.GetComponent<PlayerControler> ().enabled = true;
		}

		// hide mouse
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	/**
	 * Starts the execution of the game.
	 */
	public static void StartGame() {
		// register handler for game over event
		//OnScreenDisplay.RegisterDeathCallback (new OnDeathHandler ());
		PhotonNetwork.ConnectUsingSettings("alpha 0.1");
		UnPause ();
	}
	
	/**
	 * Restarts the game at the first level with full health, battery, and ammo.
	 */
	public static void RestartGame() {

	}
	
	/**
	 * Called when the player died. Displays end game UIs.
	 */
	public static void PlayerDied() {

	}
	
	/**
	 * Called when player reaches the end of a level. Dispatches EndLevel GUIs
	 * and loads next level.
	 */
	public static void EndLevel() {
		Debug.Log("GAAAMMMEEEE OOVEEERRR!!!!!!");
	}
	
	/**
	 * An enum of possible game states.
	 */
	public enum GameMode {
		StartMenu,			// at the start screen
		GameSetup,			// setting up your game to play
		Paused,				// at the pause screen
		UnPaused,			// in normal game play
		Dead,				// at the death screen
		EndLevel,			// at the end level screen
		EndGame				// at the end game screen
	}
}