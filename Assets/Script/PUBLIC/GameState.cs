using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateValue{
    None, Load, Start, Play, Lose, End, Pause
}
//None  is current game is none/inactive
//Load  is game status is loading new SCENE or Loading assets (Preparing the assets ready to use)
//Start is game started and fully loaded
//Play  is game at play state
//Lose  is current game is lose (wether restared the scene / exit the current scene)
//End   is game state that exit current scene
//Pause is paused and freeze all the activity
public class GameState : MonoBehaviour
{
    [SerializeField]
    GameStateValue currentGameState = GameStateValue.None;

    void Awake(){
        Services.I.Add<GameState>(this, true);
        DontDestroyOnLoad(this);

        NoneGame(); //Set to none
    }


    #region Communicator Setter
    public void NoneGame()=>  currentGameState = GameStateValue.None;
    public void LoadGame()=>  currentGameState = GameStateValue.Load;
    public void StartGame()=> currentGameState = GameStateValue.Start;
    public void PlayGame()=>  currentGameState = GameStateValue.Play;
    public void LoseGame()=>  currentGameState = GameStateValue.Lose;
    public void EndGame()=>   currentGameState = GameStateValue.End;
    public void PauseGame()=> currentGameState = GameStateValue.Pause;

    #endregion

    
    #region Communicator Getter
    public GameStateValue getCurrentState(){ return currentGameState; }

    public bool getIsAtPlayState(){ return currentGameState == GameStateValue.Play; } //Get and check condition is current state is PLAY

    #endregion
}
