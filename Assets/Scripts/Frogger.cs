using UnityEngine;
using System.Collections;
public class Frogger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    // public Sprite leapSprite;
    public Sprite deadSprite;

    private Vector3 startPosition;
    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.startPosition = transform.position;
    }

    private void Update()
    {
        // making the frog to move and also rotating the frog according to the move
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up);
        } else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
        } else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction) 
    {
        
        Vector3 destination = transform.position + direction;
        // detects if there is a barrier collider there
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        // if it doesn't exists, the frog can go there
        if (barrier != null)
        {
            return;
        }

        if (platform != null)
        {
            // the platform becomes the parent of our frog
            transform.SetParent(platform.transform);
        } else
        {
            transform.SetParent(null);
        }

        if (obstacle != null && platform == null)
        {
            transform.position = destination;
            FroggerDies();
        } else
        {
            transform.position += direction;
        }
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        spriteRenderer.sprite = idleSprite;
        transform.position = startPosition;
        enabled = true;
    }

    private void FroggerDies()
    {
        this.spriteRenderer.sprite = deadSprite;
        enabled = false;
        FindObjectOfType<GameManager>().Died();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            FroggerDies();
        }
    }
}
