using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health, maxHealth, deathTime;

    public GameObject canvas, minimapIndicator;
    public SlicedFilledImage healthBar;
    public bool showBossBar;

    [SerializeField] EnemyAI ai;

    [SerializeField] Transform explosion, enemyCenter;

    [SerializeField] GameObject enhancementObject;

    [SerializeField] string explosionSound;

    private void Awake()
    {
        maxHealth = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;
        Audio.PlaySound("MetalImpact", ai.audioSource);

        if (health <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Destroy(canvas);
        Destroy(minimapIndicator);
        GetComponent<Enemy>().OnDeath();
        GetComponent<Collider>().enabled = false;
        ai.animator.SetBool("isDead", true);
        ai.agent.isStopped = true;
        ai.enabled = false;
        Invoke("DestroySelf", deathTime);
    }

    private void DestroySelf()
    {
        Audio.PlaySoundAtPosition(explosionSound, transform.position);
        Instantiate(explosion, explosion.position + enemyCenter.position, explosion.rotation);
        ai.OnExplosion(explosion.position + enemyCenter.position);

        int randomNum = Random.Range(0, 4);
        if (randomNum == 0) Instantiate(enhancementObject, new Vector3(enemyCenter.position.x, 0, enemyCenter.position.z), Quaternion.identity);

        Destroy(gameObject);
    }

    void Update()
    {
        if (canvas != null)
        {
            canvas.transform.rotation = Quaternion.Euler(45, -30, 0);
        }
    }

}
