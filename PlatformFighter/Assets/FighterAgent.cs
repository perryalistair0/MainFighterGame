using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FighterAgent : Agent
{
    public bool isFSM = false;
    public FiniteStateMachine FSM_Model;
    string CurrentFSMInput = "";
    public CharacterStateManager enemyfighter;
    GameManager gameManager;
    CharacterStateManager character;
    Rigidbody rb;
    bool IsPlayer1; 
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {  
        FSM_Model = GetComponent<FiniteStateMachine>();
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        character = GetComponent<CharacterStateManager>();
        IsPlayer1 = character.IsPlayer1;


        if(IsPlayer1)
        {
            startPos = new Vector3(-5, 4, 0);
        }
        else
        {
            startPos = new Vector3(5, 4, 0);
        }
        
    }
    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.position = character.StartPosition;
        gameManager.Restart();
    
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        
        // Obeserve internal state
        
        for (int ci = 0; ci < (int)CharacterStateManager.States.CharacterLastState; ci++)
        {
            sensor.AddObservation((int)character.currentEnum == ci ? 1.0f : 0.0f);
        }
        // Observe enemy state
        for (int ci = 0; ci < (int)CharacterStateManager.States.CharacterLastState; ci++)
        {
            sensor.AddObservation((int)enemyfighter.currentEnum == ci ? 1.0f : 0.0f);
        }
        // observe current move state
        for (int ci = 0; ci < (int)CharacterStateManager.MoveState.LastState; ci++)
        {
            sensor.AddObservation((int)enemyfighter.currentMove == ci ? 1.0f : 0.0f);
        }
        // See what stage they're at
        sensor.AddObservation(enemyfighter.transform.rotation.eulerAngles.y);

        // Distance to other fighter
        sensor.AddObservation(transform.position);
        sensor.AddObservation(enemyfighter.transform.position);

        if(IsPlayer1)
        {
            sensor.AddObservation(gameManager.Player1Health);
            sensor.AddObservation(gameManager.Player2Health);
        }
        else
        {            
            sensor.AddObservation(gameManager.Player2Health);
            sensor.AddObservation(gameManager.Player1Health);
        }       
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int input = actionBuffers.DiscreteActions[0];
        if(input == 1) { character.currentState.AIinput(character, "a");}
        if(input == 2) { character.currentState.AIinput(character, "s");}
        if(input == 3) { character.currentState.AIinput(character, "d"); }
        if(input == 4) { character.currentState.AIinput(character, "j"); }
        if(input == 5) { character.currentState.AIinput(character, "k"); }
        /*
        if(IsPlayer1 && GetComponent<Rigidbody>().velocity.x == 10)
        {
            Debug.Log("Rewarded going forward");
            AddReward(0.1f);
        }
        else if (!IsPlayer1 && GetComponent<Rigidbody>().velocity.x == -10)
        {
            AddReward(0.1f);
        }
        */

        //float distance = Vector3.Distance(transform.position, enemyfighter.transform.position);
//        Debug.Log("Distance; " + distance + " reward, " + (0.1f / distance));
      //  if(distance > 3f)
       // {
            // Debug.Log("Too far 0.0");
            //AddReward(-0.000001f);
        //}
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if(isFSM)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            discreteActionsOut[0] = FSM_Model.FSMmove();
        }
        else{
            if(IsPlayer1)
            {
                var discreteActionsOut = actionsOut.DiscreteActions;
                if(Input.GetKey("a"))
                {
                    discreteActionsOut[0] = 1;
                }
                if(Input.GetKey("s"))
                {
                    discreteActionsOut[0] = 2;
                }
                if(Input.GetKey("d"))
                {
                    discreteActionsOut[0] = 3;
                }
                if(Input.GetKey("j"))
                {
                    discreteActionsOut[0] = 4;
                }
                if(Input.GetKey("k"))
                {
                    discreteActionsOut[0] = 5; 
                }        
            }
            if(!IsPlayer1)
            {
                var discreteActionsOut = actionsOut.DiscreteActions;
                if(Input.GetKey("left"))
                {
                    discreteActionsOut[0] = 1;
                }
                if(Input.GetKey("down"))
                {
                    discreteActionsOut[0] = 2;
                }
                if(Input.GetKey("right"))
                {
                    discreteActionsOut[0] = 3;
                }
                if(Input.GetKey("x"))
                {
                    discreteActionsOut[0] = 4;
                }
                if(Input.GetKey("c"))
                {
                    discreteActionsOut[0] = 5; 
                }        
            }
        }

        
        /*
        if(IsPlayer1)
        {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if(CurrentFSMInput == "a"){
            discreteActionsOut[0] = 1;
        }
        if(CurrentFSMInput == "s"){
            discreteActionsOut[0] = 2;
        }
        if(CurrentFSMInput == "d"){
            discreteActionsOut[0] = 3;
        }
        if(CurrentFSMInput == "j"){
            discreteActionsOut[0] = 4;
        }
        if(CurrentFSMInput == "k"){
            discreteActionsOut[0] = 5;
        }
        CurrentFSMInput = "";
        } */
    }
    public void GameOver()
    {
        if(IsPlayer1)
        {
            Debug.Log("Final: " + GetCumulativeReward());
        }
        EndEpisode();
    }

    public void FSMInput(string input)
    {
        CurrentFSMInput = input; 
    }
    
    public void DamageEnemy(float Damage)
    {
        
        AddReward(Damage*0.001f);
    }
    public void DamagePlayer(float Damage)
    {
        AddReward(-Damage*0.001f);
    }
    
    public void PlayerWon()
    {
        AddReward(1);
        if(IsPlayer1)
        {
            Debug.Log("Score: " + GetCumulativeReward());
        }
    }
    public void PlayerLost()
    {
        AddReward(-1);
        
        if(IsPlayer1)
        {
            Debug.Log("Score: " + GetCumulativeReward());
        }
    }
    public float GetReward()
    {
        return GetCumulativeReward();
    }
}
