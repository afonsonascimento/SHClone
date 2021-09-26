using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
    private CapsuleCollider _enemyCollider;

    private int shootingRange = 10;
    
    private bool _isShooting = false;
    private int _shootingTimer = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
        _enemyAnimator = GetComponent<Animator>();
        _enemyWeaponRigidbody = _enemyWeapon.GetComponent<Rigidbody>();
        _enemyCollider = GetComponent<CapsuleCollider>();
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
        _enemyWeaponRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _enemyWeaponRigidbody.AddForce((_playerTransform.position - transform.position) *2, ForceMode.Impulse);
        _enemyWeaponRigidbody.AddForce(Vector3.up * 2, ForceMode.Impulse);

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

                if (Vector3.Distance(_playerTransform.position, transform.position) >= shootingRange){
                    _enemyAnimator.SetTrigger("Running");
                    transform.position += transform.forward * (1f * Time.deltaTime);
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
}
