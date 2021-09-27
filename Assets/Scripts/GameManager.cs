using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager Instance => _instance;

    [SerializeField, Tooltip("Player reference")]
    private GameObject _player;

    [SerializeField, Tooltip("Player head")]
    private GameObject _playerHead;

    [SerializeField, Tooltip("Player right hand")]
    private GameObject _playerRightHand;
    
    [SerializeField, Tooltip("Player left hand")]
    private GameObject _playerLeftHand;

    public GameObject Player => _player;

    private bool _startSlowMotion;
    private float _headSpeed;
    private float _rHandSpeed;
    private float _lHandSpeed;
    private Vector3 _lastHeadPos;
    private Vector3 _lastRightHandPos;
    private Vector3 _lastLeftHandPos;
    private bool _slowMotion;
    private float _slowMotionThreshold = 0.1f;
    private float _elapsedTime;
    private float _timeScaleTarget;

    
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
        //StartCoroutine(nameof(WaitForGameLoad));
    }

    private IEnumerator WaitForGameLoad()
    {
        yield return new WaitForSeconds(1);
        _startSlowMotion = true;
    }

    private void LateUpdate()
    {
        if (!_startSlowMotion){
            return;
        }
        
        if (Time.realtimeSinceStartup - _elapsedTime >= 0.1f){
            _elapsedTime = Time.realtimeSinceStartup;
            CalculateSlowMotion();
        }
    }

    private void FixedUpdate()
    {
        if (!_startSlowMotion){
            return;
        }
        
        _timeScaleTarget = _slowMotion ? 0.1f : 1;

        Time.timeScale = Mathf.Lerp(Time.timeScale, _timeScaleTarget, Time.deltaTime * 30);
    }

    /// <summary>
    /// Calculates slow motion based on head and hands speed
    /// </summary>
    private void CalculateSlowMotion()
    {
        var headPosition = _playerHead.transform.position;
        _headSpeed = (headPosition - _lastHeadPos).magnitude;
        _lastHeadPos = headPosition;
        
        var rHandPosition = _playerRightHand.transform.position;
        _rHandSpeed = (rHandPosition - _lastRightHandPos).magnitude;
        _lastRightHandPos = rHandPosition;
        
        var lHandPosition = _playerLeftHand.transform.position;
        _lHandSpeed = (lHandPosition - _lastLeftHandPos).magnitude;
        _lastLeftHandPos = lHandPosition;

        _slowMotion = _headSpeed + _rHandSpeed + _lHandSpeed < _slowMotionThreshold;
    }
}
