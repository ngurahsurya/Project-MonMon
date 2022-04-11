using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    GameObject petObject;
    Transform petTranform;
    Vector3 petVector;

    //Data
    [SerializeField]
    float movement_speed = 5;
    Vector3 directionMovement = Vector3.zero;

    //Status
    bool isPetMove = false;

    #region Getter Function
  
    #endregion
    
    #region  Public Setter Function
    public void SetPetComponent(GameObject _gameobject){
        petObject = _gameobject;
        petTranform = petObject.transform;
        petVector = petTranform.position;
    }
    public void SetMove(Vector3 _movePos){ 
        directionMovement = _movePos;
        isPetMove = true;
    }
    #endregion
    
    private void Update() {

        //Movement Pet
        MovingPet(directionMovement);    
    }
    
    #region Logic Function
    void MovingPet(Vector3 nextDirection){
        if(!isPetMove)
            return;

        float step = movement_speed * Time.deltaTime;
        Vector3 target = nextDirection;
        if(Vector3.Distance(this.transform.position, target) > 0.1f){
            petTranform.position = Vector3.MoveTowards(petTranform.position, target, step);       
        }else{
            isPetMove = false;
        }
    }

    #endregion
}
