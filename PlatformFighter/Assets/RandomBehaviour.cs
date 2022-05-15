using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviour : MonoBehaviour
{
    CharacterStateManager character;
    float actionInterval = 1f;
    float NextAction;
    string[] Move_Actions = new string[] {"a", "s", "d", "j", "k"}; // "a", "s", "d", , "k"
    string[] Crouch_Actions = new string[] {"s", "j"}; //, 
    string RandomMove;
    int RandomIndex;
    // Start is called before the first frame update
    void Start()
    {
       
        character = GetComponent<CharacterStateManager>();
        NextAction = Time.time + actionInterval;       
        RandomIndex = Random.Range(0, Move_Actions.Length);
        RandomMove = Move_Actions[RandomIndex]; 
    }

    // Update is called once per frame
    void Update()
    {
        character.currentState.AIinput(character, RandomMove);
        if(Time.time > NextAction)
        {
            if(character.currentEnum == CharacterStateManager.States.CharacterCrouchState)
            {

                RandomIndex = Random.Range(0, Crouch_Actions.Length);
                RandomMove = Crouch_Actions[RandomIndex];
                
            }
            else
            {
                RandomIndex = Random.Range(0, Move_Actions.Length);
                RandomMove = Move_Actions[RandomIndex];
                character.currentState.AIinput(character, RandomMove);
                NextAction = Time.time + Random.Range(0.1f, actionInterval);
            }
        }
    }
}
