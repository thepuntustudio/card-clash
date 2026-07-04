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

    public Image playerPanelImage;
    public Image enemyPanelImage;

    public GameObject floatingTextPrefab;
    public Transform playerTextSpawn; // e.g. PlayerHealthBar position
    public Transform enemyTextSpawn;  // e.g. EnemyHealthBar position

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
            SpawnFloatingText(enemyTextSpawn, $"-{attackDamage}", Color.red);
            StartCoroutine(FlashPanel(enemyPanelImage, Color.white));
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
            SpawnFloatingText(playerTextSpawn, $"+{healAmount}", Color.green);
            StartCoroutine(FlashPanel(playerPanelImage, Color.green));
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
    yield return new WaitForSeconds(1.0f); // let player read their own action first

    messageText.text = "👹 Enemy is preparing to attack...";
    yield return new WaitForSeconds(1.0f);

    int damage = enemyAttack - playerShield;
    
    SpawnFloatingText(playerTextSpawn, damage > 0 ? $"-{damage}" : "Blocked!", damage > 0 ? Color.red : Color.cyan);
    StartCoroutine(FlashPanel(playerPanelImage, Color.white));

    if (damage < 0) damage = 0;
    playerHP -= damage;
    if (playerHP < 0) playerHP = 0;
    playerShield = 0;

    messageText.text = damage > 0 ? $"💢 Enemy attacks for {damage} damage!" : "🛡️ Shield blocked the attack!";
    UpdateUI();

    yield return new WaitForSeconds(1.2f);

    if (playerHP <= 0)
    {
        playerHP = 0;
        UpdateUI();
        messageText.text = "💀 GAME OVER! Press Restart to try again.";
        EndGame();
        yield break;
    }

    isPlayerTurn = true;   // <- this line was missing before, causing the stuck cards
    messageText.text = "⚔️ Your turn! Choose an action.";
    attackBtn.interactable = true;
    blockBtn.interactable = true;
    healBtn.interactable = true;
}

    void UpdateUI()
{
    playerHealthBar.maxValue = playerMaxHP;
    enemyHealthBar.maxValue = enemyMaxHP;
    playerHealthText.text = $"HP: {playerHP} / {playerMaxHP}";
    enemyHealthText.text = $"HP: {enemyHP} / {enemyMaxHP}";

    StartCoroutine(SmoothBar(playerHealthBar, playerHP));
    StartCoroutine(SmoothBar(enemyHealthBar, enemyHP));
}

IEnumerator SmoothBar(Slider bar, float target)
{
    float start = bar.value;
    float elapsed = 0f;
    float duration = 0.4f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        bar.value = Mathf.Lerp(start, target, elapsed / duration);
        yield return null;
    }
    bar.value = target;
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

 // Add a red flash to whichever panel got hit, and a green flash on heal
    IEnumerator FlashPanel(Image panelImage, Color flashColor)
{
    Color original = panelImage.color;
    panelImage.color = flashColor;
    yield return new WaitForSeconds(0.15f);
    panelImage.color = original;
}

void SpawnFloatingText(Transform spawnPoint, string message, Color color)
{
    GameObject go = Instantiate(floatingTextPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.root);
    go.GetComponent<FloatingText>().Setup(message, color);
}

}

