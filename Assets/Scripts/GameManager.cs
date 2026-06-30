using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // References
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public Text playerHealthText;
    public Text enemyHealthText;
    public Text messageText;
    public Button attackBtn;
    public Button blockBtn;
    public Button healBtn;
    public Button restartBtn;

    // Game state
    private int playerMaxHP = 100;
    private int playerHP;
    private int enemyMaxHP = 100;
    private int enemyHP;
    private int enemyAttack = 12;
    private int playerShield = 0;
    private int healAmount = 15;
    private int attackDamage = 10;
    private bool isPlayerTurn = true;
    private bool gameOver = false;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        // Reset everything
        playerHP = playerMaxHP;
        enemyHP = enemyMaxHP;
        playerShield = 0;
        gameOver = false;
        isPlayerTurn = true;

        // Update UI
        UpdateUI();
        messageText.text = "⚔️ Choose your action!";
        
        // Show/hide buttons
        attackBtn.interactable = true;
        blockBtn.interactable = true;
        healBtn.interactable = true;
        restartBtn.gameObject.SetActive(false);
    }

    // Called when player clicks a button
    public void PlayerAttack()
    {
        if (!isPlayerTurn || gameOver) return;
        PerformPlayerAction("attack");
    }

    public void PlayerBlock()
    {
        if (!isPlayerTurn || gameOver) return;
        PerformPlayerAction("block");
    }

    public void PlayerHeal()
    {
        if (!isPlayerTurn || gameOver) return;
        PerformPlayerAction("heal");
    }

    void PerformPlayerAction(string action)
    {
        // Disable buttons during the action
        attackBtn.interactable = false;
        blockBtn.interactable = false;
        healBtn.interactable = false;

        // Player's turn
        if (action == "attack")
        {
            enemyHP -= attackDamage;
            if (enemyHP < 0) enemyHP = 0;
            messageText.text = $"⚔️ You dealt {attackDamage} damage!";
        }
        else if (action == "block")
        {
            playerShield = 8;
            messageText.text = "🛡️ You raise your shield!";
        }
        else if (action == "heal")
        {
            playerHP += healAmount;
            if (playerHP > playerMaxHP) playerHP = playerMaxHP;
            messageText.text = $"❤️ You healed {healAmount} HP!";
        }

        UpdateUI();

        // Check if enemy died
        if (enemyHP <= 0)
        {
            enemyHP = 0;
            UpdateUI();
            messageText.text = "🎉 YOU WIN! Press Restart to play again.";
            EndGame();
            return;
        }

        // Enemy's turn after a short delay
        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.0f);

        // Calculate damage with shield
        int damage = enemyAttack - playerShield;
        if (damage < 0) damage = 0;
        
        playerHP -= damage;
        if (playerHP < 0) playerHP = 0;
        
        // Reset shield after use
        playerShield = 0;

        // Show enemy attack message
        if (damage > 0)
            messageText.text = $"💢 Enemy attacks for {damage} damage!";
        else
            messageText.text = "🛡️ Your shield blocked all damage!";

        UpdateUI();

        // Check if player died
        if (playerHP <= 0)
        {
            playerHP = 0;
            UpdateUI();
            messageText.text = "💀 GAME OVER! Press Restart to try again.";
            EndGame();
            yield break;
        }

        // Back to player's turn
        isPlayerTurn = true;
        messageText.text = "⚔️ Your turn! Choose an action.";
        attackBtn.interactable = true;
        blockBtn.interactable = true;
        healBtn.interactable = true;
    }

    void UpdateUI()
    {
        // Update health bars
        playerHealthBar.value = playerHP;
        playerHealthBar.maxValue = playerMaxHP;
        playerHealthText.text = $"HP: {playerHP} / {playerMaxHP}";

        enemyHealthBar.value = enemyHP;
        enemyHealthBar.maxValue = enemyMaxHP;
        enemyHealthText.text = $"HP: {enemyHP} / {enemyMaxHP}";
    }

    void EndGame()
    {
        gameOver = true;
        isPlayerTurn = false;
        attackBtn.interactable = false;
        blockBtn.interactable = false;
        healBtn.interactable = false;
        restartBtn.gameObject.SetActive(true);
    }

    // Called by Restart button
    public void RestartGame()
    {
        StartGame();
    }
}