using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    Rigidbody2D rb;
    public float jumpForce;
    public float speed;
    public int direction;

    //Ground check
    public bool isGrounded;

    //Animation variables
    Animator anim;
    public bool moving;

    //audio variables
    public AudioSource soundEffects;
    public AudioClip itemCollect;
    public AudioClip doorEnter;
    public AudioClip Footsteps;
    public AudioClip[] sounds;

    public Gamemanager gm;

    public ProjectileBehaviour ProjectilePrefab;
    public Transform ShootingPoint;

    // Start is called before the first frame update
    void Start()
    {
            soundEffects = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;

        //variables to mirror the player
        Vector3 newScale = transform.localScale;
        float currentScale = Mathf.Abs(transform.localScale.x); //take absolute value of the current x scale, this is always positive


        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= speed;
            newScale.x = -currentScale;
            moving = true;
            direction = 1;
        }

        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += speed;
            newScale.x = currentScale;
            moving = true;
            direction = 0;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            moving = true;

        }

        if(Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moving = false;
        }

        if(Input.GetKeyDown("g"))
        {
            var bullet = Instantiate(ProjectilePrefab, ShootingPoint.position, transform.rotation);
            if(direction == 0)
            {
                bullet.Speed = bullet.Speed;
            }
            if (direction == 1)
            {
                bullet.Speed = -bullet.Speed;
            }
        }

        anim.SetBool("Ismoving", moving);

        transform.position = newPosition;
        transform.localScale = newScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("i hit the ground");
            isGrounded = true;
        }

        if (collision.gameObject.tag.Equals("coin"))
        {
            //score goes up
            gm.score++;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene("EndScene");
        }


        if (collision.gameObject.tag.Equals("Ground"))
        {
            isGrounded = false;
        }

     if(collision.gameObject.tag.Equals("Door"))
        {
            Debug.Log("change scene");
            //soundEffects.PlayOneShot(sounds[0], .7f); //play door sound effect
            SceneManager.LoadScene("Level 2"); //take to new scene
        }

        if (collision.gameObject.tag.Equals("Door2"))
        {
            Debug.Log("change scene");
            //soundEffects.PlayOneShot(sounds[0], .7f); //play door sound effect
            SceneManager.LoadScene("EndScene"); //take to new scene
        }

    }
}
