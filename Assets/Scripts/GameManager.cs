using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager Instance => _instance;

    [SerializeField, Tooltip("Score Controller reference")]
    private ScoreController _scoreController;

    [SerializeField, Tooltip("Slow motion controller")]
    private SlowMotionController _slowMotionController;

    [SerializeField, Tooltip("EnemySpawnerController")]
    private EnemySpawnerController _enemySpawnerController;

    [SerializeField, Tooltip("Start game object")]
    private GameObject _startGameObject;

    [SerializeField, Tooltip("Player reference")]
    private GameObject _player;
    
    [SerializeField, Tooltip("Game time")]
    private int _gameTime = 300;
    public GameObject Player => _player;
    public ScoreController ScoreController => _scoreController;
    public SlowMotionController SlowMotionController => _slowMotionController;
    

    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {
        _slowMotionController.StartSlowMotionMechanic();
    }

    /// <summary>
    /// Starts the game, called by Start game button in scene SHClone
    /// </summary>
    public void StartGame()
    {
        _enemySpawnerController.StartSpawning(_gameTime);
        _startGameObject.SetActive(false);
    }



    public void EndGame()
    {
        _scoreController.SaveScore();
        _startGameObject.SetActive(true);
        //RESET GAME
    }
    
}
