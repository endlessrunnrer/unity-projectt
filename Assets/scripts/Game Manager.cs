using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager 사용을 위해 추가
using TMPro;  // TextMeshPro 관련 지시어 추가



public enum GameState {

    Intro, 

    Playing,

    Dead
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState State = GameState.Intro; 

    public float PlayStartTime; 

    public int Lives = 3; 

    [Header("References")]

    public GameObject IntroUI; 

    public GameObject DeadUI; 

    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldenSpanwer;

    public Player PlayerScript;

    public TMP_Text scoreText; 

    void Awake(){
        if(Instance == null){
            Instance = this; 

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IntroUI.SetActive(true);
    }

    float CalculateScore(){
        return Time.time - PlayStartTime; 
    }

    void SaveHighScore(){
        int score = Mathf.FloorToInt(CalculateScore()); 
        int currentHighScore = PlayerPrefs.GetInt("highScore", 0);
        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            PlayerPrefs.Save(); 
            Debug.Log("새로운 최고 점수 저장: " +score);
        }
    }

    int GetHighScore(){
        return PlayerPrefs.GetInt("highScore", 0);
    }

    public float CalculateGameSpeed() {
        if(State != GameState.Playing){
            return 15f; 
        }
        float speed = 15f + (0.5f * Mathf.Floor(CalculateScore() / 10f));
        return Mathf.Min(speed, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if(State == GameState.Playing){
            scoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore()); 
        }else if(State == GameState.Dead) {
            scoreText.text = "High Score: " + GetHighScore(); 
            Debug.Log("게임 오버 - 최고 점수: " + GetHighScore());
        }

        if(State == GameState.Intro && Input.GetKeyDown(KeyCode.Space)){
            State = GameState.Playing; 
            IntroUI.SetActive(false);
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpanwer.SetActive(true);
            PlayStartTime = Time.time;
            Debug.Log("게임 시작 = PlayStartTime: " + PlayStartTime ); 
        }
        if(State == GameState.Playing && Lives == 0){
            if (PlayerScript != null)
                {
                    PlayerScript.KillPlayer();
                }
            else
                {
                    Debug.LogWarning("PlayerScript가 null입니다. 인스펙터에서 PlayerScript를 확인하세요.");
                }
            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpanwer.SetActive(false);
            DeadUI.SetActive(true);
            State = GameState.Dead;
            SaveHighScore();

        }
        if(State == GameState.Dead && Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene("main");
        }
    }
}
