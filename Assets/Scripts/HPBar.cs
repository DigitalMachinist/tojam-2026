using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStatsCurrent playerStats;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI totalHealthText;

    private int currentHP;
    private int maxHP;

    private void Start()
    {
        currentHP = playerHealth != null ? playerHealth.CurrentHP : 0;
        maxHP = playerHealth != null ? playerHealth.MaxHP : 0;
        Refresh();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.DamageTaken += OnHPChanged;
            playerHealth.Healed += OnHPChanged;
        }
        if (playerStats != null)
            playerStats.MaxHPChanged += OnMaxHPChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.DamageTaken -= OnHPChanged;
            playerHealth.Healed -= OnHPChanged;
        }
        if (playerStats != null)
            playerStats.MaxHPChanged -= OnMaxHPChanged;
    }

    private void OnHPChanged(int _)
    {
        currentHP = playerHealth.CurrentHP;
        Refresh();
    }

    private void OnMaxHPChanged(int newMax)
    {
        maxHP = newMax;
        Refresh();
    }

    private void Refresh()
    {
        if (playerHealth == null) return;

        if (fillImage != null)
            fillImage.fillAmount = maxHP > 0 ? (float)currentHP / maxHP : 0f;

        if (currentHealthText != null)
            currentHealthText.text = currentHP.ToString();

        if (totalHealthText != null)
            totalHealthText.text = maxHP.ToString();
    }
}
