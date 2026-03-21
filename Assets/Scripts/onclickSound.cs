using UnityEngine;
using UnityEngine.EventSystems;

public class onclickSound : MonoBehaviour, IPointerClickHandler
{
 
    public GameObject stickPerson;
    public Sprite scaredStickPerson;
    public Sprite defaultStickPerson;
    public AudioClip horn;
    public int jumpStrength = 50000;
    private bool isStickPersonGrounded = true;
    private bool hasJumped = false;
    private LayerMask ground;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPointerClick( PointerEventData eventData ) {
        jumpPerson(stickPerson);
        GetComponent<AudioSource>().PlayOneShot(horn);
    }

    void jumpPerson(GameObject stickPerson){
        RaycastHit2D res = Physics2D.Raycast (stickPerson.transform.position, Vector2.down, 1f, ground);
        if (res) //is grounded?
        {
            stickPerson.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpStrength);
            stickPerson.gameObject.GetComponent<SpriteRenderer>().sprite = scaredStickPerson;
            hasJumped = true;
        }
    }

    void Start(){
       ground = LayerMask.GetMask("Ground"); 
    }

    void Update(){
        RaycastHit2D res = Physics2D.Raycast (stickPerson.transform.position, Vector2.down, 1f, ground);
        if (res && !isStickPersonGrounded) {
            stickPerson.gameObject.GetComponent<SpriteRenderer>().sprite = defaultStickPerson;
            isStickPersonGrounded = true;
        } 
        else if(!res && hasJumped){
            isStickPersonGrounded = false;
            hasJumped = false;
        }
    }
}
