using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager Instance => _instance;

    [SerializeField, Tooltip("Player reference")]
    private GameObject _player;

    public GameObject Player => _player;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }
}
