using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float maxLifetime = 5f;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        transform.position += new Vector3(speed * Time.deltaTime * direction, 0, 0);


        lifetime += Time.deltaTime;
        if (lifetime > maxLifetime) gameObject.SetActive(false);

        Debug.Log($"Speed: {speed}, Direction: {direction}, Hit: {hit}");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");
    }
    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        Debug.Log("Direction:" + direction);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
