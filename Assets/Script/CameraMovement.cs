using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //SERVICES_COMPONENT
    GameState gameState{get { return Services.I.Get<GameState>();}}
    GameManager gameManager{get { return Services.I.Get<GameManager>();}}

    Swipe swipe{get{ return Services.I.Get<Swipe>();}}

    [Header("Component")]
    [SerializeField] Camera mainCamera;
    [Header("Transform & Positon")]
    [SerializeField] Vector3 currentVector = Vector3.zero;
    [SerializeField] Vector3 centerVector;
    [SerializeField] float lerp_speed = 2f;
    [SerializeField] float zoomIn = 5;
    [SerializeField] float zoomOut = 20;

    //STATUS
    bool finishPayLoad = false;

    //==================================================================================
    private void OnEnable() {
        Swipe.onFingerPress += fingerPressed;
        Swipe.onFingerHold += fingerHold;
        Swipe.onScrollMouse += scrollMouse;

        gameManager.payLoadFunction += AnimateCameraAtFirst;
    }

    private void OnDisable() {
        Swipe.onFingerPress -= fingerPressed;
        Swipe.onFingerHold -= fingerHold;
        Swipe.onScrollMouse -= scrollMouse;

        gameManager.payLoadFunction -= AnimateCameraAtFirst;
    }

    private void Awake() {
        GetMainCamera(); //Get Main Camera
    }

    

    //================================================================================================================================
    #region Communicator Method
    public bool GetDonePayLoad(){ return finishPayLoad; }

    #endregion

    void AnimateCameraAtFirst(){
        Debug.Log("Animate");
        Vector3 destination = new Vector3(centerVector.x, centerVector.y, mainCamera.transform.position.z);
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, destination, lerp_speed * Time.deltaTime);
        float checkDistance = Vector3.Distance(mainCamera.transform.localPosition, destination);
        Debug.Log("Distance " + checkDistance);
        if(checkDistance < 0.01f)
        {
            finishPayLoad = true; 
            gameManager.checkPayloadDone(); //Set this payload done
        }
    }
    //===================================================================================
    #region Register and Get
    void GetMainCamera()=> mainCamera = Camera.main;

    #endregion
    
    void fingerPressed(){ //User Finger Pressed
        if(!gameState.getIsAtPlayState()) //Check is Play State
            return;

        Debug.Log("Finger pressed");
        currentVector = swipe.GetWorldPositon(0);
    }

    void fingerHold(){ //User Finger Released
        if(!gameState.getIsAtPlayState()) //Check is Play State
            return;
            
        Debug.Log("Finger Released");
        Vector3 direction = currentVector - swipe.GetWorldPositon(0);
        Vector3 endPos = validationEndPos(mainCamera.transform.position + direction);
        MoveLerp(endPos); //Move Lerp Action
    }

    void scrollMouse(float axis) => PinchZoom(axis);

    #region Movement

    void MoveLerp(Vector3 _posToGo) =>  mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, _posToGo, lerp_speed * Time.deltaTime);

    void PinchZoom(float _increament) => mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - _increament, zoomIn, zoomOut); 
    #endregion

    Vector3 validationEndPos(Vector3 _pos){ //Check if value not out of bound
        Vector3 posToCheck = _pos;
        Vector3 posToReturn = Vector3.zero;
        float xVal = posToCheck.x;
        float yVal = posToCheck.y;
        
        //Check X Value Bound
        if(xVal > gameManager.GetMaxPos().x)
            xVal = gameManager.GetMaxPos().x;
        if(xVal < gameManager.GetMinPos().x)
            xVal = gameManager.GetMinPos().x;
        
        //Check Y Vakue Bound
        if(yVal > gameManager.GetMaxPos().y)
            yVal = gameManager.GetMaxPos().y;
        if(yVal < gameManager.GetMinPos().y)
            yVal = gameManager.GetMinPos().y;
            
        posToReturn = new Vector3(xVal, yVal, posToCheck.z);
        return posToReturn;
    }


}
