﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    Rigidbody rb; 
    CharacterStateManager character;
    public CharacterStateManager Enemy;
    FighterAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterStateManager>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<FighterAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(character.currentEnum == CharacterStateManager.States.CharacterMoveState)
        {
            // move closer
            float Distance = Mathf.Abs(transform.position.x - Enemy.transform.position.x);
            if(Distance > 2.5f) 
            { 
                    if((character.IsPlayer1 && rb.velocity.x < 5)||
                      (!character.IsPlayer1 && rb.velocity.x > -5))
                    {
                        character.currentState.AIinput(character, "d");
                    }
            }
            // attack
            else
            {
                if(Enemy.currentEnum == CharacterStateManager.States.CharacterCrouchState)
                {
                    character.currentState.AIinput(character,"s");
                }
                else
                {
                    if(Random.value > 0.1){character.currentState.AIinput(character,"j");}
                    else{character.currentState.AIinput(character,"k");}
                }
            }
        }
        if(character.currentEnum == CharacterStateManager.States.CharacterCrouchState)
        {
            if(Enemy.currentEnum == CharacterStateManager.States.CharacterCrouchState)
            {
                character.currentState.AIinput(character,"j");
            }
            else
            {
                character.currentState.AIinput(character,"s");
            }
        }
    }
}
