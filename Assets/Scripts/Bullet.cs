using UnityEngine;

public class Bullet : MonoBehaviour
{
    /*private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
