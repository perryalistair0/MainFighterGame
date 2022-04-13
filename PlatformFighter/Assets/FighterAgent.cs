using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FighterAgent : Agent
{
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
        sensor.AddObservation(Vector3.Distance(transform.position, enemyfighter.transform.position));
        sensor.AddObservation(rb.velocity.x);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int input = actionBuffers.DiscreteActions[0];
        if(input == 1) { character.currentState.AIinput(character, "a"); }
        if(input == 2) { character.currentState.AIinput(character, "s"); Debug.Log("Crouched");}
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

        float distance = Vector3.Distance(transform.position, enemyfighter.transform.position);
//        Debug.Log("Distance; " + distance + " reward, " + (0.1f / distance));
        if(distance > 3f)
        {
            Debug.Log("Too far 0.0");
            AddReward(-0.0001f);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if(IsPlayer1)
        {
            var discreteActionsOut = actionsOut.DiscreteActions;
            if(Input.GetKeyDown("a"))
            {
                discreteActionsOut[0] = 1;
            }
            if(Input.GetKeyDown("s"))
            {
                discreteActionsOut[0] = 2;
            }
            if(Input.GetKeyDown("d"))
            {
                discreteActionsOut[0] = 3;
            }
            if(Input.GetKeyDown("j"))
            {
                discreteActionsOut[0] = 4;
            }
            if(Input.GetKeyDown("k"))
            {
                discreteActionsOut[0] = 5; 
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
        EndEpisode();
    }

    public void FSMInput(string input)
    {
        CurrentFSMInput = input; 
    }
    public void DamageEnemy(float Damage)
    {
        Debug.Log("Damage: " + Damage/15);
        AddReward(Damage/15);
    }
    public void DamagePlayer(float Damage)
    {
        AddReward(-Damage/15);
    }
}
