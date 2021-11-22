using UnityEngine;

public static class Collision2DExtensions
{
    public static bool WasHitFromBelow(this Collision2D collision)
    {
        return collision.contacts[0].normal.y > 0.5f;
    }
    
    public static bool WasHitFromAbove(this Collision2D collision)
    {
        return collision.contacts[0].normal.y < -0.5f;
    }
    public static bool WasHitFromLeft(this Collision2D collision)
    {
        return collision.contacts[0].normal.x > 0.5f;
    }
    public static bool WasHitFromRight(this Collision2D collision)
    {
        return collision.contacts[0].normal.x < -0.5f;
    }

    public static bool WasHitByPlayer(this Collision2D collision)
    {
        return collision.collider.GetComponent<Player>();
    }

    public static bool WasHitByFireball(this Collision2D collision)
    {
        return collision.collider.GetComponent<Fireball>();
    }
}