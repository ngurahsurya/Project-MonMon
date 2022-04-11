using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_StateAction : MonoBehaviour
{
  [Header("Pet Compoent")]
  [SerializeField] PetMovement petMovement;
  [SerializeField] PetEating petEating;
  [SerializeField] PetSleeping petSleeping;
  [SerializeField] PetInteract petInteract;
  [SerializeField] PetPlaying petPlaying;
 
  #region Getter Function
  void GettAllStateComponent(){
    petMovement = this.gameObject.GetComponent<PetMovement>(); //Movement Component
    petEating = this.gameObject.GetComponent<PetEating>(); //Eating Component
    petSleeping = this.gameObject.GetComponent<PetSleeping>(); //Sleeping Component
    petInteract = this.gameObject.GetComponent<PetInteract>(); //Interact Component
    petPlaying = this.gameObject.GetComponent<PetPlaying>(); //Playing Component
  }
  #endregion   
  #region Public Setter Function
  public void SetMovingStateComponent()
  {
    
  }
  #endregion
  private void Awake() {
      GettAllStateComponent(); //Getting all state component avaliable
  }
  private void Start() {
      //State Setter
      SetMovingStateComponent(); //Set moving pet component
  }
}
