using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Map"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
