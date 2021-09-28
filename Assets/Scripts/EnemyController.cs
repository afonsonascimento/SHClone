using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class EnemyController : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy data")]
    private EnemySO _enemyData;
    
    [SerializeField, Tooltip("Is the enemy dead")]
    private bool _isDead;

    [SerializeField, Tooltip("Enemy weapon")]
    private GameObject _enemyWeapon;

    [SerializeField, Tooltip("Bullet reference")]
    private GameObject _bullet;
    
    [SerializeField, Tooltip("Bullet spawn point")]
    public Transform _bulletSpawnPoint;
    
    [SerializeField, Tooltip("Bullet Speed")]
    public float _bulletSpeed = 1000f;
    
    [SerializeField, Tooltip("Bullet life (seconds)")]
    public float _bulletLife = 5f;

    [SerializeField, Tooltip("Ragdoll Rigidbody list")]
    private List<Rigidbody> _ragdollRigidbodies;

    private Transform _playerTransform;
    private Animator _enemyAnimator;
    private Rigidbody _enemyWeaponRigidbody;
    private GunShoot _enemyWeaponScript;
    private CapsuleCollider _enemyCollider;

    private int _shootingRange;
    private int _runningSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
        _enemyAnimator = GetComponent<Animator>();
        _enemyWeaponRigidbody = _enemyWeapon.GetComponent<Rigidbody>();
        _enemyWeaponScript = _enemyWeapon.GetComponent<GunShoot>();
        _enemyWeaponRigidbody.isKinematic = true;
        _enemyWeaponRigidbody.interpolation = RigidbodyInterpolation.None;
        _enemyCollider = GetComponent<CapsuleCollider>();
        
        //Get data from Enemy SO
        _shootingRange = _enemyData.ShootingRange;
        _runningSpeed = _enemyData.RunningSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovementAndAnims();
    }

    /// <summary>
    /// Kills the enemy
    /// </summary>
    private void EnemyHit()
    {
        _isDead = true;
        _enemyAnimator.enabled = false;

        foreach (var rigidbody in _ragdollRigidbodies){
            rigidbody.isKinematic = false;
        }

        _enemyCollider.enabled = false;

        _enemyWeapon.transform.parent = null;
        _enemyWeaponRigidbody.isKinematic = false;
        //_enemyWeaponRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _enemyWeaponRigidbody.AddForce((_playerTransform.position - transform.position) *2, ForceMode.Impulse);
        _enemyWeaponRigidbody.AddForce(Vector3.up * 2, ForceMode.Impulse);
        
        GameManager.Instance.ScoreController.EnemyKilledPoints();
        StartCoroutine(nameof(DestroyEnemy));
    }

    /// <summary>
    /// Controls enemy movement and animations
    /// </summary>
    private void EnemyMovementAndAnims()
    {
        switch (_isDead){
            case true:
                return;
            case false:
                transform.LookAt(new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z));

                if (Vector3.Distance(_playerTransform.position, transform.position) >= _shootingRange){
                    _enemyAnimator.SetTrigger("Running");
                    transform.position += transform.forward * (_runningSpeed * Time.deltaTime);
                } else{
                    _enemyAnimator.SetTrigger("Shooting");
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.K)){
            EnemyHit();
        }
    }

    /// <summary>
    /// Called by enemy shooting animation event, shoots the player
    /// </summary>
    private void ShootPlayer()
    {
        if (_bullet != null && _bulletSpawnPoint != null)
        {
            _enemyWeaponScript.PlayShotSFX();
            GameObject clonedProjectile = Instantiate(_bullet, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            Rigidbody projectileRigidbody = clonedProjectile.GetComponent<Rigidbody>();
            float destroyTime = 0f;
            if (projectileRigidbody != null){
                clonedProjectile.transform.LookAt(new Vector3(_playerTransform.position.x, _playerTransform.position.y, _playerTransform.position.z) + new Vector3(0, 1, 0));
                projectileRigidbody.AddForce(clonedProjectile.transform.forward * _bulletSpeed);
                destroyTime = _bulletLife;
            }
            Destroy(clonedProjectile, destroyTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")){
            EnemyHit();
        }
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
