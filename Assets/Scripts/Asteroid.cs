using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites;

    // --- AGGIUNGI QUESTA VARIABILE ---
    [Header("Audio")]
    public AudioClip explosionSound;

    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxLifetime = 30f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        rb.mass = size;

        Destroy(gameObject, maxLifetime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            if (explosionSound != null)
            {
                
                // Questo suono NON si toglie quando l'asteroide viene distrutto
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);// Crea un punto audio temporaneo nella posizione dell'asteroide
            }

            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            GameManager.Instance.OnAsteroidDestroyed(this);
            Destroy(gameObject);
        }
    }

    private Asteroid CreateSplit()
    {
        Vector2 position = transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, transform.rotation);
        half.size = size * 0.5f;

        // Passa il riferimento del suono anche al resto degli asteroidi creati dalla divisione
        half.explosionSound = this.explosionSound;

        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }
}