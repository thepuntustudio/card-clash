using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "CardClash/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP;
    public int attackDamage;
    [TextArea] public string introLine; // handy later for Phase 6
}