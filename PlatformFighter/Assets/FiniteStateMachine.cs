using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    Rigidbody rb; 
    CharacterStateManager character;
    public CharacterStateManager Enemy;
    FighterAgent agent;
    public enum states
    {
        Aggressive,
        PassiveAggressive,
        Passive,
        Approach
    }
    private states CurrentEnumState;

    private float NextSwitch;
    private float Interval1 = 1f; 
    private float Interval2 = 3f; 
    float distance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterStateManager>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<FighterAgent>();

        NextSwitch = Time.time + Random.Range(Interval1, Interval2);
    }

    // Update is called once per frame
    // 1: A
    // 2: S
    // 3: D
    // 4: J
    // 5: K
    void Update()
    {
        distance = Vector3.Distance(character.transform.position, Enemy.transform.position);
        
        if(Time.time > NextSwitch)
        {
            if(distance > 4)
            {
                CurrentEnumState = states.Approach;
            }
            else
            {   
                if(Random.value > 0.5)
                {
                    CurrentEnumState = states.Aggressive;
                }            
                else
                {
                    CurrentEnumState = states.PassiveAggressive;
                }
            }
            NextSwitch = Time.time + Random.Range(Interval1, Interval2);
        }        
    }
    public int FSMmove()
    {   
        switch (CurrentEnumState)
        {
            case states.Aggressive:
                return AggressiveUpdate();
            case states.PassiveAggressive:
                return PassiveAggressiveUpdate();
        }

        return AggressiveUpdate();
    }
    int Approach()
    {
        return 3;
    }
    int AggressiveUpdate()
    {
        if(character.currentEnum == CharacterStateManager.States.CharacterMoveState)
        {
            // crouch to block leg sweep
            if(Enemy.currentEnum == CharacterStateManager.States.CharacterLegSweepState)
            {
                if(Enemy.currentMove == CharacterStateManager.MoveState.Charge)
                {
                    if(Enemy.transform.rotation.eulerAngles.y < 35)
                    {
                        return 4; 
                    }
                    else
                    {
                        return 3; 
                    }
                     
                }
                if((Enemy.currentMove == CharacterStateManager.MoveState.Attack) && (distance < 3.5f))
                {
                    return 2;
                }
                if(Enemy.currentMove == CharacterStateManager.MoveState.Cooldown)
                {
                    if((character.currentEnum == CharacterStateManager.States.CharacterCrouchAndBlockState)
                    || (character.currentEnum == CharacterStateManager.States.CharacterCrouchState))
                    {
                        return 3; 
                    }
                    else
                    {
                        return 4; 
                    }
                } 
                return 3; 
            }
            // approach if to far away
            else if(distance > 2.5)
            {
                return 3; 
            }
            
            else if((Enemy.currentEnum == CharacterStateManager.States.CharacterPunchState)
                    && (Enemy.currentMove == CharacterStateManager.MoveState.Attack))
            {
                return 1; 
            }
            // if in range punch
            else
            {
                if(Random.value < 0.6)
                {
                    return 4;
                }
                // randomly move back
                else
                {
                    return 1; 
                }
            }  
        }
        else if(character.currentEnum == CharacterStateManager.States.CharacterCrouchState)
        {
            // crouch to block leg sweep
            if(Enemy.currentEnum == CharacterStateManager.States.CharacterLegSweepState)
            {
                return 2; 
            }
            // Stand up
            else 
            {
                return 3;
            }
        }
        else
        {
            // Punch by default
            return 4; 
        }
    }
    int PassiveAggressiveUpdate()
    {
        if(character.currentEnum == CharacterStateManager.States.CharacterMoveState)
        {
            // if in perfect range, kick
            if((distance < 3.5f) && (distance > 3.2f))
            {
                return 5;
            }
            // Move toward if too far
            if(distance > 3.5f)
            {
                return 3;
            }
            // if too close punch for space
            if(distance < 2f)
            {
                return 4; 
            }
            // if just too close
            if(distance < 3f)
            {
                return 1;
            }
        }
        // kick by default
        return 5;
    }
    int PassiveUpdate()
    {
        float distanceX = Mathf.Abs(Enemy.transform.position.x - character.transform.position.x);
        if(distanceX > 3.5f)
        {
            return 3; 
        }
        if(Enemy.currentEnum == CharacterStateManager.States.CharacterPunchState)
        {
            return 1; 
        }
        if(Enemy.currentEnum == CharacterStateManager.States.CharacterLegSweepState)
        {
            return 2; 
        }
        if(distanceX > 1.5f)
        {
            return 3;
        }
        else
        {
            if(character.currentEnum == CharacterStateManager.States.CharacterMoveState)
            {
                return 2; 
            }
            return 4;     
        }
    }
}
