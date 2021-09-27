using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "Enemy Data", menuName = "Enemy Files/Enemy Data")]
public class EnemySO : ScriptableObject
{
    public int ShootingRange;
    public int RunningSpeed;
}
