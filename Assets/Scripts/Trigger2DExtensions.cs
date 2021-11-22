using UnityEngine;

public static class Trigger2DExtensions
{
    public static bool WasTriggeredByPlayer(this Collider2D collision)
    {
        return collision.GetComponent<Player>();
    }
}