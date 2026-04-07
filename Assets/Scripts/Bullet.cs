using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 500f;
    public float maxLifetime = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction)
    {
        // Il proiettile ha bisogno di una forza da aggiungere solo una volta poiché non ha resistenza per fermarsi
        rb.AddForce(direction * speed);

        // Distruzione proiettile dopo che ha raggiunto la sua durata massima...
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // e non appena entra in collisione con qualcosa
        Destroy(gameObject);
    }

}
