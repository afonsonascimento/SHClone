using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy prefab reference")]
    private GameObject _enemyPrefab;

    private readonly int[] _difficultyTimers ={3, 2, 1};
    private float _elapsedTime = 0;
    private float _totalElapsedTime;
    private float _secondsBetweenSpawn;
    private float _gameTime = 300;
    private bool _startSpawning;
    private Vector3 _selectedRandomPosition;

    /// <summary>
    /// Enables enemy spawns and sets game total time
    /// </summary>
    public void StartSpawning(int gameTime)
    {
        _gameTime = gameTime;
        _startSpawning = true;
    }

    private void Update()
    {
        if (!_startSpawning){
            return;
        }

        if (_totalElapsedTime >= _gameTime){
            _startSpawning = false;
            GameManager.Instance.EndGame();
        }

        _totalElapsedTime += Time.deltaTime;
        _elapsedTime += Time.deltaTime;

        CheckSecondsBetweenSpawnTimer();
        if (_elapsedTime > _secondsBetweenSpawn){
            _elapsedTime = 0;
            
            GetRandomInstantiateTransform();
            
            Instantiate(_enemyPrefab, _selectedRandomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks elapsed time to set spawner frequency
    /// </summary>
    private void CheckSecondsBetweenSpawnTimer()
    {
        if (_elapsedTime < 30){
            _secondsBetweenSpawn = _difficultyTimers[0];
        } else if (_elapsedTime < 60){
            _secondsBetweenSpawn = _difficultyTimers[1];
        } else{
            _secondsBetweenSpawn = _difficultyTimers[2];
        }
    }

    /// <summary>
    /// Gets a random Transform from available list to instantiate enemy prefab
    /// </summary>
    private void GetRandomInstantiateTransform()
    {
        _selectedRandomPosition = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
}