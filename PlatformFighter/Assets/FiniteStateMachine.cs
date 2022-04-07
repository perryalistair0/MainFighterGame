using System.Collections;
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
                    if((character.IsPlayer1 && rb.velocity.x != 10)||
                        !character.IsPlayer1 && rb.velocity.x != -10)
                    {
                        agent.FSMInput("d");
                    }
            }

            // attack
            else
            {
                if(Enemy.currentEnum == CharacterStateManager.States.CharacterCrouchState)
                {
                    agent.FSMInput("s");
                }
                else
                {
                    if(Random.value > 0.1){agent.FSMInput("j");}
                    else{agent.FSMInput("k");}
                }
            }
        }
        if(character.currentEnum == CharacterStateManager.States.CharacterCrouchState)
        {
            if(Enemy.currentEnum == CharacterStateManager.States.CharacterCrouchState)
            {
                agent.FSMInput("j");
            }
            else
            {
                agent.FSMInput("s");
            }
        }
    }
}
