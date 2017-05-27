using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfo : MonoBehaviour {

    public GameObject MonsterPrefab;
    private Random Random;
    public Dictionary<int, float> YPositions;
    public float MonsterSpeed = 0.1f;
    public int MonsterSpawnThreshold;
    public float XPosition;
    public int Iteration;

    public CharacterSlotGrid Grid;

    public static int Score = 0;
    public static bool GameOver = false;
    private bool GameEnded = false;

    public GameObject GameOverScreen;
    public Text ScoreText;

    public static int NumberOfMonsters;
    public GameObject SpawnPosition;

    // Use this for initialization
    void Start () {
        Random = new Random();
        MonsterSpawnThreshold = 9980;
    }

    private void Update()
    {
        if (!GameEnded)
        {
            if (GameOver)
            {
                GameEnded = true;
                GameOverScreen.SetActive(true);
                ScoreText.text = "Score: " + Score.ToString();
            }
            else
            {
                MakeRandomMonster();
                Iteration++;
                if (Iteration > 1000)
                {
                    MonsterSpeed += 0.05f;
                    MonsterSpawnThreshold -= 10;
                    Iteration = 0;
                }
            }
        }
    }

    private void MakeRandomMonster()
    {
        if (YPositions == null)
        {
            YPositions = new Dictionary<int, float>();
            for (int i = 0; i < Grid.Rows.Length; i++)
            {
                YPositions.Add(i, Grid.Rows[i].transform.position.y);
            }
        }

        if (NumberOfMonsters == 0 || Random.Range(1,10000) > MonsterSpawnThreshold)
        {
            Colour colour = (Colour)Random.Range(0, 7);
            int position = Random.Range(0, 5);
            GameObject monster = (GameObject)Instantiate(MonsterPrefab, 
                new Vector2(SpawnPosition.transform.position.x, YPositions[position]), Quaternion.identity);
            Monster mon = monster.GetComponent<Monster>();
            mon.Colour = colour;
            mon.MonsterSpeed = MonsterSpeed;
            mon.Health = Random.Range(1, 10);
            NumberOfMonsters++;
        }
    }
}
