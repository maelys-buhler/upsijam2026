using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int currentLevel = 0;
    private bool leftKey = false;
    private bool rightKey = false;
    private bool upKey = false;
    private LayerMask ground;
    private Rigidbody2D body;
    public AudioClip deathSound;
    public AudioClip walkSound;

    public float speed = 5f;
    private bool isGrounded = true;
    public float jumpForce = 8f;
    private bool wasGrounded = true;
    private bool jumpKeyed = false;
    private bool jumpClicked = false;

    [SerializeField] private AudioClip horn;

    [SerializeField] private Sprite scaredStickPerson;
    private Sprite defaultStickPerson;

    private bool lockedMovement = false;

    public void death()
    {
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        ResetAtCurrentLevelSpawnPoint();
    }

    public Vector3 GetCurrentLevelSpawnPoint()
    {
        return GameObject.Find("SpawnPoint"+currentLevel).transform.position;
    }

    public void ResetAtCurrentLevelSpawnPoint()
    {
        transform.position = GetCurrentLevelSpawnPoint();
        transform.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        foreach (var item in FindObjectsByType<ClickableObject>(FindObjectsSortMode.None))
        {
            item.ResetLocation();
        }
        lockedMovement = true;
        StartCoroutine(Sleep(0.5f, ()=>lockedMovement = false));
        if(currentLevel == 4)
        {

            ClickableObject clickableObject = GameObject.Find("BtnOver").GetComponent<ClickableObject>();
            clickableObject.isDragEnabled = false;
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        if(currentLevel == 4)
        {
            this.transform.parent = GameObject.Find("BtnOver").transform;
            this.transform.localPosition = new Vector2(0, 2);
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }


    IEnumerator Sleep(float seconds, Action then)
    {
        yield return new WaitForSeconds(seconds);
        then.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO REMOVE
        NextLevel();
        NextLevel();
        NextLevel();
        NextLevel();
        leftKey = true;
        rightKey = true;
        //upKey = true;
        //---------
        ResetAtCurrentLevelSpawnPoint();
    }

    private void Awake()
    {
        ground = LayerMask.GetMask("Ground");
        body = GetComponent<Rigidbody2D>();
        defaultStickPerson = GetComponent<SpriteRenderer>().sprite;
    }

    public void Jump(bool jumpKeyed)
    {
        // Cannot double jump by pressing 2 space
        if(this.jumpKeyed && jumpKeyed)
        {
            return;
        }
        // Cannot double jump by scare jumping 2 times
        if (this.jumpClicked && !jumpKeyed)
        {
            return;
        }
        // Cannot key jump after scare jump
        if(this.jumpClicked && jumpKeyed)
        {
            return;
        }
        body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce);
        if (jumpKeyed)
        {
            this.jumpKeyed = true;
        } 
        else
        {
            this.jumpClicked = true;
            transform.GetComponent<SpriteRenderer>().sprite = scaredStickPerson;
            GetComponent<AudioSource>().PlayOneShot(horn);
        }
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, ground);

        if (!wasGrounded && isGrounded)
        {
            transform.GetComponent<SpriteRenderer>().sprite = defaultStickPerson;
            this.jumpClicked = false;
            this.jumpKeyed = false;
        }

        if (Input.GetButtonDown("Jump") && upKey)
            Jump(true);

        wasGrounded = isGrounded;
    }

    void FixedUpdate()
    {
        if (lockedMovement)
            return;
        float direction = Input.GetAxisRaw("Horizontal");
        float targetSpeed = direction * speed;
        float speedDif = targetSpeed - body.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 8f : 9f;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 1f) * Mathf.Sign(speedDif);
        // Vector right to prevent affecting up/down velocity (which is alreary handled by jump)
        body.AddForce(movement * Vector2.right);

        if(currentLevel == 4){
            this.transform.localPosition = new Vector2(0, 2);
            ClickableObject btn = GameObject.Find("BtnOver").GetComponent<ClickableObject>();
            if(btn.isDragEnabled == false && !Input.GetMouseButton(0))
            {
                btn.isDragEnabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null || collision.gameObject == null)
        {
            return;
        }
        if(collision.gameObject.name == "Ouchies" || collision.gameObject.tag == "Deadline")
        {
            death();
        }
        if(collision.gameObject.tag == "EndLevel")
        {
            NextLevel();
            ResetAtCurrentLevelSpawnPoint();
        }
        if(collision.gameObject.name.StartsWith("Item") && collision.gameObject.name.EndsWith("Arrow"))
        {
            collision.gameObject.SetActive(false);
            GameObject.Find(collision.gameObject.name.Substring(4)).GetComponent<SpriteRenderer>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            string name = collision.gameObject.name;
            if (name.Contains("Left"))
                leftKey = true;
            if(name.Contains("Right"))
                rightKey = true;
            if(name.Contains("Up"))
                upKey = true;
        }
    }

}
