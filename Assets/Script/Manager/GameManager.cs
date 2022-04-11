using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //State Game
    GameState gameState{get { return Services.I.Get<GameState>();}}
    TilesController tilesController;
    
    //EVENT METHOD
    public delegate void LoadAsset();
    public delegate void PayLoadFunction();
    public LoadAsset loadAsset;
    public PayLoadFunction payLoadFunction;


    //Status
    int payLoadDone = 0;
    int payLoadLength = 0;

    private void Awake() {
        Services.I.Add<GameManager>(this, true); //Add Services
        StartCoroutine(initState());
        
        GetTilesController(); //Get Local tulesController
    }

    private void Update() {
        RunningPreload(); //Running PreLoad Function (Start State)
    }

    #region Inisiate Method
    void GetTilesController()=> tilesController = FindObjectOfType<TilesController>();
    public Vector3 GetMaxPos(){ Debug.Log("Get Max ");  return tilesController.getMaxPos(); }
    public Vector3 GetMinPos(){ Debug.Log("Get Min"); return tilesController.getMinPos(); }
    
    #endregion

    #region  Communcator Method

    void checkPayloadListener()=> payLoadLength = payLoadFunction.GetInvocationList().Length;
    public void checkPayloadDone ()=> payLoadDone += 1;

    #endregion

    //=========================================================================================================================
    #region State Controller
    IEnumerator initState(){
        Debug.Log("Inisiate Game State");
        yield return new WaitUntil(setGameLoad);
        checkPayloadListener();
        Debug.Log("Listener Length " + payLoadLength);
        setGameStart();
    }
    bool setGameLoad(){
        loadAsset?.Invoke();
        gameState.LoadGame();
        return true;
    }

    void setGameStart(){
        gameState.StartGame();
    }

    void setGamePlay(){
        Debug.Log("Start game state");
        gameState.PlayGame();
    }
    void setGameEnd(){
        gameState.EndGame();
    }

    void setGamePause(){
        gameState.PauseGame();
    }
    #endregion

    #region Method
    void RunningPreload(){
        if(gameState.getCurrentState() != GameStateValue.Start)
            return;
        
        payLoadFunction?.Invoke(); //Execute preload
        bool isDone = payLoadDone == payLoadLength;
        Debug.Log(payLoadDone);
        if(isDone)
            setGamePlay();
    }

    #endregion
}
