using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager Instance => _instance;

    [SerializeField, Tooltip("Score Controller reference")]
    private ScoreController _scoreController;

    [SerializeField, Tooltip("Slow motion controller")]
    private SlowMotionController _slowMotionController;

    [SerializeField, Tooltip("Player reference")]
    private GameObject _player;

    

    public GameObject Player => _player;
    public ScoreController ScoreController => _scoreController;
    

    
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

    
}
