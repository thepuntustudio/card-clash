using UnityEngine;

public class EnemyScaler : MonoBehaviour
{
    private int fightCount = 0;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Call this when the player wins a fight
    public void NextFight()
    {
        fightCount++;
        // Each fight: enemy gets +5 HP and +1 attack
        // You'd need to expand GameManager to support this
        Debug.Log($"Fight {fightCount} - Enemy gets stronger!");
    }
}