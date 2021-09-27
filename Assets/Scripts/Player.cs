using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")){
            GameManager.Instance.ScoreController.GotHitPoints();
        }
    }
}
