using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    private int currentLevel = 0;
    private bool leftKey = false;
    private bool rightKey = false;
    private bool upKey = false;
    public float speed = 10000;
    public float maxVelocity = 10;
    private LayerMask ground;
    private Rigidbody2D body;

    private bool lockedMovement = false;


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
    }

    public void NextLevel()
    {
        currentLevel++;
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
        //NextLevel();
        //NextLevel();
        leftKey = true;
        rightKey = true;
        upKey = true;
        //---------
        ResetAtCurrentLevelSpawnPoint();
    }

    private void Awake()
    {
        ground = LayerMask.GetMask("Ground");
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump"))
        {
            Debug.Log("uwu");
        }
    }

    void FixedUpdate()
    {
        if (lockedMovement)
            return;
        /*float moveHorizontal = Input.GetAxis("Horizontal");
        if(moveHorizontal > 0 && !rightKey){moveHorizontal = 0;}
        else if(moveHorizontal < 0 && !leftKey){moveHorizontal = 0;}
        else if(moveHorizontal < 0){moveHorizontal = -1;}
        else if(moveHorizontal > 0){moveHorizontal = 1;}
        float moveVertical = Input.GetAxis("Vertical");
        if(moveVertical < 0){moveVertical = 0;}
        else if(moveVertical > 0 && !upKey){moveVertical = 0;}
        else if(!(Physics2D.Raycast (transform.position, Vector2.down, 1f, ground)))
        {
            moveVertical = 0;
        }
        else if(moveVertical > 0){moveVertical = 1;}
        body.AddForce(new Vector2 (moveHorizontal*speed, moveVertical*speed));
        if(body.linearVelocity.magnitude > maxVelocity){
            body.linearVelocity = (body.linearVelocity / body.linearVelocity.magnitude) * maxVelocity;
        }*/
        float direction = Input.GetAxisRaw("Horizontal");
        float targetSpeed = direction * speed;
        float speedDif = targetSpeed - body.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 8f : 9f;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, 1f) * Mathf.Sign(speedDif);
        // Vector right to prevent affecting up/down velocity (which is alreary handled by jump)
        body.AddForce(movement * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null || collision.gameObject == null)
        {
            return;
        }
        if(collision.gameObject.name == "Ouchies" || collision.gameObject.tag == "Deadline")
        {
            ResetAtCurrentLevelSpawnPoint();
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
