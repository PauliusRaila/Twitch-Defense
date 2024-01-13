using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Helicopter : MonoBehaviour {

    public Transform helicopterSpot;
    //public Transform[] rescueSpots;
    public Transform destination;
    public float Cooldown = 30;
    public float timeOnGround = 10;
    public int seats = 10;
    public int civiliansRescued;

    public Transform propeller;
    private int speed = 1000;

   
    private Animator anim;


    
    public enum HelicopterState { Flying, Landed, Base }

    HelicopterState currentState;  

    private void Start()
    {
        anim = GetComponent<Animator>();
        SetCurrentState(HelicopterState.Flying);
    }

    private void Update()
    {
        propeller.Rotate(0f, speed * Time.deltaTime, 0f);

        if (currentState == HelicopterState.Flying && this.transform.position != destination.position)
            this.transform.position = Vector3.Lerp(transform.position, destination.position, Time.deltaTime );



            

    }

    IEnumerator HelicopterCountdown() {      
        yield return new WaitForSeconds(1f);
    }

    void SetCurrentState (HelicopterState state)
    {
        currentState = state;
    }



}
