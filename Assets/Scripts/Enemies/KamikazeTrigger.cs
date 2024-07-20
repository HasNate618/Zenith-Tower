using UnityEngine;

public class KamikazeTrigger : MonoBehaviour
{
    public bool active;
    
    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            GetComponentInParent<EnemyHealth>().OnDeath();
            print("Boom!");
        }
    }
}
