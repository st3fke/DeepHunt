using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float swimSpeed = 2f;         // Brzina plivanja
    public float fleeSpeed = 4f;         // Brzina bežanja od igrača
    public float fleeDistance = 3f;      // Udaljenost na kojoj riba počinje da beži od igrača
    public Transform player;             // Referenca na igrača
    private bool isFleeing = false;      // Da li riba beži od igrača?
    private int direction = 1;           // Pravac kretanja (1 je desno, -1 je levo)

    void Start()
    {
        // Početna orijentacija sprite-a
        FlipSprite();
    }

    void Update()
    {
        // Proveri udaljenost između ribe i igrača
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < fleeDistance)
        {
            // Ako je igrač preblizu, počinje bežanje
            isFleeing = true;
        }
        else
        {
            // Ako je igrač dovoljno daleko, prestaje da beži
            isFleeing = false;
        }

        if (isFleeing)
        {
            // Bežanje od igrača
            FleeFromPlayer();
        }
        else
        {
            // Normalno plivanje levo-desno
            Swim();
        }
    }

    void Swim()
    {
        // Kretanje levo-desno
        transform.Translate(Vector2.right * direction * swimSpeed * Time.deltaTime);
    }

    void FleeFromPlayer()
    {
        // Pravac bežanja od igrača
        Vector2 directionToFlee = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)directionToFlee, fleeSpeed * Time.deltaTime);

        // Okretanje sprite-a u odnosu na igrača
        if (transform.position.x < player.position.x) // Ako je riba levo od igrača
        {
            direction = -1;
        }
        else if (transform.position.x > player.position.x) // Ako je riba desno od igrača
        {
            direction = 1;
        }

        FlipSprite();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ako nije igrač, promeni pravac kretanja
        if (!collision.gameObject.CompareTag("Player"))
        {
            direction *= -1; // Promeni pravac
            FlipSprite();    // Okreni sprite
        }
    }

    void FlipSprite()
    {
        // Okreni sprite u pravcu kretanja
        if (direction == 1) // Desno
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction == -1) // Levo
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
