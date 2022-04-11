using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField] Transform maximumVerticalPos;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    private Vector2 currentSwipe;
    private float SWIPE_THRESHOLD = .5f;

    //EVENTS
    public delegate void OnFingerPress();
    public delegate void OnFingerHold();
    public delegate void OnFingerRelease();
    public delegate Vector2 OnSwipeVector();
    public delegate void OnSwipeHorizontal(bool isRight);
    public delegate void OnSwipeVertical(bool isUp);
    public delegate void OnScrollMouse(float axis);
    public static OnFingerPress onFingerPress;
    public static OnFingerHold onFingerHold;
    public static OnFingerRelease onFingerRelease;
    public static OnSwipeVector onSwipeVector;
    public static OnSwipeHorizontal onSwipeHorizontal;
    public static OnSwipeVertical onSwipeVertical;
    public static OnScrollMouse onScrollMouse;

    //Status
    bool isPressed = false;
    public bool isRight;
    Vector2 vectorPosSwiping;

    #region Public Get Method
    public Vector3 GetWorldPositon(float z){ 
        Ray mousepos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0,z));
        float distance = 0;
        ground.Raycast(mousepos, out distance);
        return mousepos.GetPoint(distance);
     }
    Vector2 getFingerDownPos(){ return fingerDown; }

    #endregion   

    private void Awake() {
        Services.I.Add<Swipe>(this, true);
    }
    private void Update() {
        SwipingChecker(); //Swiping Checker
    }

    #region Update Function
    void SwipingChecker(){
        if(Input.GetMouseButtonDown(0)) //PRESSED 
        {
            isPressed = true;
            onFingerPress?.Invoke(); //Running on finger press
            fingerDown = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if(Input.GetMouseButton(0)){ //HOLD
            onFingerHold?.Invoke();
        }
        
        if(onScrollMouse != null) //Scrolling Mouse input
            onScrollMouse(Input.GetAxis("Mouse ScrollWheel"));

        if(Input.GetMouseButtonUp(0) && isPressed){ //RELEASED
            isPressed = false;
            onFingerRelease?.Invoke(); //Running on finger release

            fingerUp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector2(fingerUp.x - fingerDown.x, fingerUp.y - fingerDown.y);
            currentSwipe.Normalize();

            float currentYPosTap = Camera.main.ScreenToWorldPoint(fingerDown).y;
            float maximumVerticalSwap = 0;
            if(maximumVerticalPos != null)
                maximumVerticalSwap = maximumVerticalPos.transform.position.y;

            //Swipe Up
            if (currentSwipe.y > 0 && currentSwipe.x > -SWIPE_THRESHOLD && currentSwipe.x < SWIPE_THRESHOLD && currentYPosTap < maximumVerticalSwap) OnSwipeUp();
            //Swipe Down
            if (currentSwipe.y < 0 && currentSwipe.x > -SWIPE_THRESHOLD && currentSwipe.x < SWIPE_THRESHOLD && currentYPosTap < maximumVerticalSwap) OnSwipeDown();
            //Swipe Right
            if (currentSwipe.x > 0 && currentSwipe.y > -SWIPE_THRESHOLD && currentSwipe.y < SWIPE_THRESHOLD) OnSwipeLeft();
            //Swipe Left
            if (currentSwipe.x < 0 && currentSwipe.y > -SWIPE_THRESHOLD && currentSwipe.y < SWIPE_THRESHOLD) OnSwipeRight();
        }
    }

    #endregion

    void VectorSwipe(Vector2 swipePos)
    {
        if (swipePos != null || swipePos != Vector2.zero)
        {
            Debug.Log(swipePos);
            vectorPosSwiping = swipePos;
            onSwipeVector?.Invoke();
        }
    }

    #region swiping direction
    void OnSwipeUp()
    {
        Debug.Log("Swipe Up");
        if (onSwipeVertical != null)
            onSwipeVertical(true);
    }
    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
        if (onSwipeVertical != null)
            onSwipeVertical(false);
    }
    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
        isRight = false;
        if (onSwipeHorizontal != null)
            onSwipeHorizontal(isRight);
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
        isRight = true;
        if (onSwipeHorizontal != null)
            onSwipeHorizontal(isRight);
    }
    
    #endregion
}
