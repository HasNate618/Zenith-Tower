using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth, healAmount;
    public static float health = 75;
    public bool invincible;
    public bool dead;

    public float healthPercent { get { return health / maxHealth; } }

    [SerializeField] SlicedFilledImage healthBar, changeBar;
    [SerializeField] GameMenu menu;
    [SerializeField] GameObject ui;
    [SerializeField] ParticleSystem healEffect;
    [SerializeField] InventoryItemData batteryReference;
    [SerializeField] Text batteryCount, typedText;
    [SerializeField] string deathText;
    [SerializeField] float chipSpeed, delayTime;
    private float lerpTimer, delayTimer;
    private bool playingHealthAnimation;

    private void Awake()
    {
        if (FloorManager.floor == 0) health = 75;
        healthBar.fillAmount = healthPercent;
        changeBar.fillAmount = healthPercent;
        UpdateBatteryCount();
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;

        if (!invincible)
        {
            health -= damage;
            UpdateHealthBar(false);
            Audio.PlaySound("PlayerDamage" + Random.Range(1, 4).ToString(), null);
        }

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void EnhanceHealth(float newHealth)
    {
        if (health > newHealth) health = newHealth;
        maxHealth = newHealth;
        healthBar.fillAmount = healthPercent;
        changeBar.fillAmount = healthPercent;
    }

    public void AbsorbHealth()
    {
        if (healAmount > 0)
        {
            health += maxHealth * healAmount;
            if (health > maxHealth) health = maxHealth;
            UpdateHealthBar(true);
        }
    }

    public void HealByPercent(float healPercent)
    {
        HealByAmount(maxHealth * healPercent * 0.01f);
    }
    
    public void HealByThird()
    {
        HealByAmount(maxHealth / 3f);
    }

    public void HealByAmount(float healAmount)
    {
        if (health >= maxHealth || !InventorySystem.inventory.Contains(batteryReference))
        {
            Audio.PlaySound("InterfaceError", null);
            return;
        }

        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        UpdateHealthBar(true);

        InventorySystem.Remove(batteryReference);
        UpdateBatteryCount();

        healEffect.Play();
        Audio.PlaySound("Shattering", null);
    }

    private void UpdateHealthBar(bool healthGained)
    {
        lerpTimer = 0;
        delayTimer = 0;

        if (healthGained)
        {
            changeBar.fillAmount = healthPercent;
            changeBar.color = Color.green * 0.9f;
        }
        else
        {
            healthBar.fillAmount = healthPercent;
            changeBar.color = Color.red * 0.8f;
        }

        if (!playingHealthAnimation)
        {
            playingHealthAnimation = true;
            StartCoroutine(PlayHealthBarAnimation());
        }
    }

    private IEnumerator PlayHealthBarAnimation()
    {
        while (changeBar.fillAmount != healthBar.fillAmount || lerpTimer == 0)
        {
            float healthBarFill = healthBar.fillAmount;
            float changeBarFill = changeBar.fillAmount;

            delayTimer += Time.deltaTime;
            if (delayTimer > delayTime)
            {
                if (changeBarFill > healthPercent)
                {
                    //healthBar.fillAmount = healthPercent;
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / chipSpeed;
                    percentComplete *= percentComplete;
                    changeBar.fillAmount = Mathf.Lerp(changeBarFill, healthPercent, percentComplete);
                }
                if (healthBarFill < healthPercent)
                {
                    //changeBar.fillAmount = healthPercent;
                    lerpTimer += Time.deltaTime;
                    float percentComplete = lerpTimer / chipSpeed;
                    percentComplete *= percentComplete;
                    healthBar.fillAmount = Mathf.Lerp(healthBarFill, changeBarFill, percentComplete);
                }
            }
            yield return null;
        }
        playingHealthAnimation = false;
    }

    public void UpdateBatteryCount()
    {
        batteryCount.text = InventorySystem.inventory.Where(x => x != null && x == batteryReference).Count().ToString();
    }

    private void OnDeath()
    {
        dead = true;
        ui.SetActive(false);
        CharacterMovement movement = GetComponent<CharacterMovement>();
        movement.characterAnimator.SetBool("isDead", true);
        movement.dead = true;
        GetComponent<CharacterShooting>().dead = true;
        FloorManager.floor = 0;
        Audio.musicSource.Pause();
        menu.Invoke("EndGame", 4);
        StartCoroutine(CoroutineUtilities.TypeOutText(deathText, typedText, 0.2f, 1, null));
    }
}
