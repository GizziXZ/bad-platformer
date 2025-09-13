using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 5f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    private Vector3 initalSpawnPosition;

    public TextMeshProUGUI winText;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initalSpawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGround())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            playJumpSound();
        }
        //else if (context.canceled)
        //{
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        //}
    }

    private void playJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private bool isGround()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0,groundLayer))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    // Detect collision with "Spike" and respawn
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Respawn();
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            ShowWinText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            Respawn();
        }
        else if (other.CompareTag("Coin"))
        {
            ShowWinText();
        }
    }

    private void Respawn()
    {
        transform.position = initalSpawnPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void ShowWinText()
    {
        if (winText != null)
        {
            winText.text = "you win!!";
            winText.color = Color.yellow;
            winText.enabled = true;
        }
    }
}
