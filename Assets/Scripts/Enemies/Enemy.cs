using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RoomSetup roomInside;

    public void OnDeath()
    {
        if (roomInside != null)
        {
            roomInside.onEnemyDeath(gameObject);
        }
    }
}
