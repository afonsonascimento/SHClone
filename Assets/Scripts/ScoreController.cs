using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField, Tooltip("Best Score")]
    private int _bestScore = 0;
    
    [SerializeField, Tooltip("Current Score")]
    private int _currentScore = 0;

    [SerializeField, Tooltip("Points for killing an enemy")]
    private int _enemyPoints = 100;
    
    [SerializeField, Tooltip("Points for getting hit by a bullet")]
    private int _getHitPoints = -20;

    [SerializeField, Tooltip("Current Score text reference")]
    private TextMeshProUGUI _currentScoreText;
    
    [SerializeField, Tooltip("Best Score text reference")]
    private TextMeshProUGUI _bestScoreText;

    private const string _bestScoreString = "HighScore: ";
    private const string _currentScoreString = "CurrentScore: ";


    private void Start()
    {
        if (PlayerPrefs.GetInt("BestScore") != null){
            _bestScoreText.text = _bestScoreString + PlayerPrefs.GetInt("BestScore");
        }
    }

    /// <summary>
    /// Ads points for killing an enemy and updates UI
    /// </summary>
    public void EnemyKilledPoints()
    {
        _currentScore += _enemyPoints;
        _currentScoreText.text = _currentScoreString + _currentScore;
    }

    /// <summary>
    /// Removes points for getting hit and updates UI
    /// </summary>
    public void GotHitPoints()
    {
        _currentScore += _getHitPoints;
        _currentScoreText.text = _currentScoreString + _currentScore;
    }

    /// <summary>
    /// Called at the end of the mini game to save high score
    /// </summary>
    public void SaveScore()
    {
        if (_currentScore > PlayerPrefs.GetInt("BestScore")){
            PlayerPrefs.SetInt("BestScore", _currentScore);
        }
    }
}
