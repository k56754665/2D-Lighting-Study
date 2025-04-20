using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance => _instance; // Singleton 인스턴스

    [Header("Prefabs")]
    public PlayerController player;
    public Canvas_Script canvas;
    private TextMeshProUGUI timerText;

    [Header("Game System")]
    bool _isgameover = false;
    public bool IsGameOver => _isgameover; // 게임 오버 상태
    public bool isGameClear = false;
    public float startingTime = 60f; // 시작시간 초 단위
    private float timeRemaining;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Init();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Build")
        {
            Init();
        }
    }

    void Init()
    {
        timeRemaining = startingTime;
        _isgameover = false;
        isGameClear = false;

        player = GameObject.FindFirstObjectByType<PlayerController>();
        canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
        if (canvas != null)
        {
            timerText = canvas.timer.GetComponent<TextMeshProUGUI>();
            canvas.gameOver.SetActive(false);
        }

        NavMeshManager.Instance.RequestNavMeshUpdate();
    }

    void Update()
    {
        // 남은 시간이 0보다 큰 경우에만 감소
        if (timeRemaining > 0 && !_isgameover)
        {
            timeRemaining -= Time.deltaTime; // 매 프레임 시간 감소
            UpdateTimerDisplay(); // 타이머 표시 업데이트
        }
        else
        {
            Gameover();
            timerText.text = "00:00";
        }

        if (isGameClear)
        {
            SceneManager.LoadScene("ClearMenu");
        }
    }


    public void Gameover()
    {
        if (player == null) return;
        Destroy(player.gameObject);
        StartCoroutine(DelayedGameOverActions());
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // 분 계산
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // 초 계산

        // "HH:mm" 형식으로 문자열 생성
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    IEnumerator DelayedGameOverActions()
    {
        yield return new WaitForSeconds(1f); 

        _isgameover = true;
        canvas.gameOver.SetActive(true);
    }

    private void OnApplicationFocus(bool focus)
    {
        Screen.SetResolution(1920, 1080, true);
    }

    private void OnApplicationQuit()
    {
        Screen.SetResolution(1920, 1080, true);
    }

}
