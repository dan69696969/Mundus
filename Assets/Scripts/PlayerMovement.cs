using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Důležité: Přidáno pro zjištění aktuální scény

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // PROMĚNNÉ PRO NEOMEZENÝ SKOK
    [Header("Infinite Jump Setup")]
    // maxJumps a availableJumps se používají POUZE pro skok ze země.
    // Skok ve vzduchu je neomezený (ignoruje availableJumps).
    private int maxJumps = 1;
    private int availableJumps;
    private const string WIND_SCENE_NAME = "Wind"; // Název scény, kde má být neomezený skok aktivní
    // ---------------------------------------------

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Registrace události pro resetování skoků při změně scény
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Odhlášení z události
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Resetujeme počet dostupných skoků při načtení scény
        availableJumps = maxJumps;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Kontrola, zda je aktivní Wind scéna
        bool isInWindScene = SceneManager.GetActiveScene().name == WIND_SCENE_NAME;

        // Reset dostupných skoků, když je hráč na zemi
        if (isGrounded())
        {
            availableJumps = maxJumps;
        }

        // Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
                Jump(isInWindScene);
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump(bool allowInfiniteJump) // Parametr říká, zda má být neomezený skok povolen
    {
        // 1. SKOK ZE ZEMĚ (Vždy povolen, spotřebuje první "availableJump")
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
            availableJumps--;
        }
        // 2. NEOMEZENÝ SKOK (Povolen POUZE ve Wind scéně, bez kontroly availableJumps)
        else if (allowInfiniteJump)
        {
            // Reset y-rychlosti pro konzistentní výšku skoku
            body.velocity = new Vector2(body.velocity.x, 0);
            // Aplikujeme novou sílu
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
            // Zde NEODEČÍTÁME availableJumps, proto je skok NEOMEZENÝ
        }
        // 3. WALL JUMP
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    // Zbytek metod pro kontrolu kolizí
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}