using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FighterAgent : Agent
{
    private float Timelimit = 30f; 
    private float EndEpisodeTime;
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
        EndEpisodeTime = Time.time + Timelimit;
    }
    void FixedUpdate()
    {
        if(Time.time > EndEpisodeTime)
        {
            Debug.Log("Restarting, " + "IsPLayer1 " + IsPlayer1 + ", "+ GetCumulativeReward());
            if(IsPlayer1){
                Debug.Log("Player 1 " + gameManager.Player1Health + ", Player 2 health "  +  gameManager.Player2Health);
            }
            EndEpisode();
            EndEpisodeTime = Time.time + Timelimit;
        }
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.position = startPos;
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
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int input = actionBuffers.DiscreteActions[0];
        if(input == 1) { character.currentState.AIinput(character, "a"); }
        if(input == 2) { character.currentState.AIinput(character, "s"); }
        if(input == 3) { character.currentState.AIinput(character, "d"); }
        if(input == 4) { character.currentState.AIinput(character, "j"); }
        if(input == 5) { character.currentState.AIinput(character, "k"); }
        if(gameManager.Player1Health < 0 || gameManager.Player2Health < 0)
        {
            EndEpisode();
        }
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
        Debug.Log("Distance; " + distance + " reward, " + (0.1f / distance));
        AddReward(0.1f / distance);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if(Input.GetKeyDown("y")){
            discreteActionsOut[0] = 1;
        }
        if(Input.GetKeyDown("u")){
            discreteActionsOut[0] = 2;
        }
        if(Input.GetKeyDown("i")){
            discreteActionsOut[0] = 3;
        }
        if(Input.GetKeyDown("o")){
            discreteActionsOut[0] = 4;
        }
        if(Input.GetKeyDown("p")){
            discreteActionsOut[0] = 5;
        }

    }
    public void DamageEnemy(int Damage)
    {
        AddReward(Damage/15);
    }
    public void DamagePlayer(int Damage)
    {
        AddReward(-Damage/15);
    }
}
