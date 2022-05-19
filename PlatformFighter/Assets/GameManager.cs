using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public CharacterStateManager Player1Manager;
    public CharacterStateManager Player2Manager;
    //public RawImage HealthBar1;
    // public RawImage HealthBar2;
    public GameObject healthBar1;
    public GameObject healthBar2; 
    private Vector3 healthBarScale;
    private float MaxHealthBarWidth;

    public float MaxPlayerHealth = 100;

    public float Player1Health;
    public float Player2Health;
    private bool isVectorAgent;
    public FighterAgent1 agent11;
    public FighterAgent agent1;
    public FighterAgent agent2; 
    float EndEpisodeTime;
    public float Timelimit = 30f; 
    public int GameCount = 250; 
    private int CurrentGame = 0;
    public string filepath = "Assets/Recourses/Player2History.txt";
    public TextMesh score1Text;
    public int score1Int = 0;

    public TextMesh score2Text;
    public int score2Int = 0;

    private void Start()
    {
        Time.timeScale = 5;
        if(agent1 == null)
        {
            isVectorAgent = true;
        }
        //Time.timeScale = 5f;
        EndEpisodeTime = Time.time + Timelimit;
        healthBarScale = healthBar1.transform.localScale;
        MaxHealthBarWidth = healthBarScale.x;
        
       //  MaxHealthBarWidth = HealthBar1.GetComponent<RectTransform>().rect.width;
        Restart();
    }
    public void Restart()
    {
       // HealthBar1.rectTransform.sizeDelta = new Vector2(MaxHealthBarWidth, HealthBar1.rectTransform.sizeDelta.y);
       //  HealthBar2.rectTransform.sizeDelta = new Vector2(MaxHealthBarWidth, HealthBar2.rectTransform.sizeDelta.y);
        healthBar1.transform.localScale = healthBarScale;    
        healthBar2.transform.localScale = healthBarScale;  


        Player1Manager.SwitchState(Player1Manager.CharacterMoveState);
        Player2Manager.SwitchState(Player2Manager.CharacterMoveState);

        Player1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player1.GetComponent<Transform>().position = Player1Manager.StartPosition;

        Player2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player2.GetComponent<Transform>().position = Player2Manager.StartPosition;

        Player1Health = MaxPlayerHealth;
        Player2Health = MaxPlayerHealth;
    }
    // isPlayer1: Is player 1 taking the damage? 
    // Damage: amount of damage applied
    public void TakeDamage(bool isPlayer1, int Damage)
    {
        if (isPlayer1)
        {
            Player1Health -= Damage;        
            // HealthBar1.rectTransform.sizeDelta = new Vector2((Player1Health / MaxPlayerHealth) * MaxHealthBarWidth,
              //                                               HealthBar1.rectTransform.sizeDelta.y);
            healthBar1.transform.localScale = new Vector3((Player1Health / MaxPlayerHealth) * MaxHealthBarWidth, 
                                                           healthBarScale.y,
            
                                                           healthBarScale.z);
            if(isVectorAgent)
            {
                agent11.DamagePlayer(Damage);
            }
            else
            {
                agent1.DamagePlayer(Damage);
            }
            agent2.DamageEnemy(Damage);
        }
        else
        {
            Player2Health -= Damage;
           // HealthBar2.rectTransform.sizeDelta = new Vector2((Player2Health / MaxPlayerHealth) * MaxHealthBarWidth,
            //                                                HealthBar2.rectTransform.sizeDelta.y);
            healthBar2.transform.localScale = new Vector3((Player2Health / MaxPlayerHealth) * MaxHealthBarWidth, 
                                                           healthBarScale.y,
                                                           healthBarScale.z);
            if(isVectorAgent)
            {
                agent11.DamagePlayer(Damage);
            }
            else
            {
                agent1.DamageEnemy(Damage);

            }
            agent2.DamagePlayer(Damage);
        }
    }
    void Update()
    {
         if(Player1Health < 0 || Player2Health < 0)
        {
            if(Player2Health < 0)
            {
                if(isVectorAgent)
                {
                    agent11.PlayerWon();
                }
                else
                {
                    agent1.PlayerWon();
                }
                score1Int++;
                score1Text.text = score1Int.ToString();
                agent2.PlayerLost();
                string line = "";
                line = "1,";
                line += Player1Health;
                writeline(line);
            }
            if(Player1Health < 0)
            {
                agent2.PlayerWon();
                if(isVectorAgent)
                {
                    agent11.PlayerLost();
                }
                else
                {
                    agent1.PlayerLost();
                }
                score2Int++;
                score2Text.text = score2Int.ToString();
                string line = "";
                line = "2,";
                line += Player2Health;
                writeline(line);
            }
            Restart();
            EndEpisodeTime = Time.time + Timelimit;
        }
        if(Time.time > EndEpisodeTime)
        {
            Debug.Log("Game end");
            if(isVectorAgent)
            {
                agent11.GameOver();
            }
            else
            {
                agent1.GameOver();
            }
            agent2.GameOver();
            EndEpisodeTime = Time.time + Timelimit;
        }
        if(Player1.transform.position.x > Player2.transform.position.x)
        {
            Vector3 Temp = Player1.transform.position;
            Player1.transform.position = Player2.transform.position;
            Player2.transform.position = Temp;
        }
    }

    void writeline(string line)
    {
        if(CurrentGame < GameCount)
        {
            StreamWriter writer = new StreamWriter(filepath, true);
            writer.WriteLine("pog");
            writer.Close();
        }
        else
        {
            Debug.Log("COMPLETED ALL GAMES YOU CAN NOW QUIT");
            Application.Quit();
        }
        CurrentGame++;
    }
}
