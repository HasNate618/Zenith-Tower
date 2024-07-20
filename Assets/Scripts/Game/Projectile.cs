using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;

    private Vector3 originalPosition;
    private float range, damage;
    private bool active;
    private string target;

    PlayerHealth health;

    [SerializeField] ParticleSystem impactParticle;

    public void Setup(float shootDirection, float range, Vector3 position, float damage, PlayerHealth health, string target)
    {
        transform.rotation = Quaternion.Euler(0, shootDirection, 0);
        this.range = range;
        originalPosition = position;
        this.damage = damage;
        GetComponent<Collider>().enabled = true;
        this.health = health;
        this.target = target;
        active = true;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        float distanceTravelled = Vector3.Distance(originalPosition, transform.position);

        if (distanceTravelled < range) transform.position += transform.forward * projectileSpeed;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") && !collider.isTrigger)
        {
            OnImpact(true, collider);
            return;
        }

        if (!collider.CompareTag(target)) return;

        switch (target)
        {
            case "Enemy":
                collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                health.AbsorbHealth();
                OnImpact(false, collider);
                return;

            case "Player":
                collider.GetComponent<PlayerHealth>().TakeDamage(damage);
                OnImpact(false, collider);
                return;
        }

    }

    private void OnImpact(bool playShatterSound, Collider collider)
    {
        impactParticle.transform.parent = collider.transform;
        impactParticle.Play();
        if (playShatterSound) Audio.PlaySoundAtPosition("ShatterImpact", transform.position);
        Destroy(gameObject);
    }
}
