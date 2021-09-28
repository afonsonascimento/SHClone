using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    [SerializeField, Tooltip("Player head")]
    private GameObject _playerHead;

    [SerializeField, Tooltip("Player right hand")]
    private GameObject _playerRightHand;
    
    [SerializeField, Tooltip("Player left hand")]
    private GameObject _playerLeftHand;
    
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
    private bool _bulletFired;

    [SerializeField, Tooltip("Time scale text")]
    private TextMeshProUGUI _timeScaleText;

    /// <summary>
    /// Enables slow motion mechanic
    /// </summary>
    public void StartSlowMotionMechanic()
    {
        StartCoroutine(nameof(WaitForGameLoad));
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

        if (_bulletFired){
            Time.timeScale = 1;
            _timeScaleText.text = "Timescale: " + Time.timeScale;
            return;
        }
        
        _timeScaleTarget = _slowMotion ? 0.1f : 1;

        Time.timeScale = Mathf.Lerp(Time.timeScale, _timeScaleTarget, Time.deltaTime * 15);
        _timeScaleText.text = "Timescale: " + Time.timeScale;
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

    /// <summary>
    /// Speeds the game for x seconds if the player fires the gun
    /// </summary>
    public void PlayerShotBullet()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(BulletTime));
    }

    /// <summary>
    /// Coroutine to speed the game when a bullet is fired from the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator BulletTime()
    {
        _bulletFired = true;
        yield return new WaitForSeconds(0.1f);
        _bulletFired = false;
    }
}
