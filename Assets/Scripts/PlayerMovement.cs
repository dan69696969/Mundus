using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Scene Names")]
    private const string WIND_SCENE_NAME = "Wind";
    private const string WATER_SCENE_NAME = "Water";
    private const string TUTORIAL_SCENE_NAME = "Tutorial";
    private const string EARTH_SCENE_NAME = "Earth";

    [Header("Gravity Settings")]
    [SerializeField] private float normalGravity = 7f;
    [SerializeField] private float waterGravity = 1.5f;

    private int maxJumps = 1;
    private int availableJumps;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    public static PlayerMovement instance;

    private void Awake()
    {
        instance = this;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        string sceneName = SceneManager.GetActiveScene().name;
        bool isInWindScene = sceneName == WIND_SCENE_NAME;
        bool isInWaterScene = sceneName == WATER_SCENE_NAME;
        bool isInTutorialScene = sceneName == TUTORIAL_SCENE_NAME;
        bool isInEarthScene = sceneName == EARTH_SCENE_NAME;

        if (isGrounded())
            availableJumps = maxJumps;

        if (wallJumpCooldown > 0.2f)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = isInWaterScene ? waterGravity : normalGravity;
            }

            bool isHoldingJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
            bool isPressingJump = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);

            // 1. Ve WIND scéně (Nekonečné lítání na DRŽENÍ)
            if (isInWindScene && isHoldingJump)
            {
                Jump(true);
            }
            // 2. V TUTORIALU (Lezení ODRAZY na DRŽENÍ + Nekonečný skok na KLIKNUTÍ)
            else if (isInTutorialScene)
            {
                if (onWall() && !isGrounded() && isHoldingJump) Jump(false);
                if (isPressingJump) Jump(true);
            }
            // 3. V EARTH (Jen lezení ODRAZY na DRŽENÍ, ale skok ve vzduchu je NORMÁLNÍ)
            else if (isInEarthScene)
            {
                // Lezení odrazy při držení
                if (onWall() && !isGrounded() && isHoldingJump)
                    Jump(false);

                // Klasický skok (nebude nekonečný)
                else if (isPressingJump)
                    Jump(false);
            }
            // 4. OSTATNÍ SVĚTY
            else if (isPressingJump)
            {
                Jump(false);
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump(bool allowInfiniteJump)
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
            availableJumps--;
        }
        else if (allowInfiniteJump)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    private bool isGrounded() => Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer).collider != null;
    private bool onWall() => Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer).collider != null;

    public bool canAttack() => horizontalInput == 0 && isGrounded() && !onWall();
}