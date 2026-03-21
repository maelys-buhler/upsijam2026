using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Rigidbody2D rbSelf;
    private Vector3 offset;
    [SerializeField] private bool isEnabledWhileDragging = true;
    [SerializeField] private LayerMask groundLayer;
    private CameraFollow mainCamera;

    public void OnPointerClick(PointerEventData eventData) { }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;

        offset = transform.position - mouseWorldPos;
        GetComponent<Collider2D>().enabled = false || isEnabledWhileDragging;
        rbSelf.bodyType = RigidbodyType2D.Dynamic;
        mainCamera.setDragging(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;
        rbSelf.MovePosition(mouseWorldPos + offset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Collider2D>().enabled = true;
        rbSelf.bodyType = RigidbodyType2D.Static;
        mainCamera.setDragging(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Rigidbody2D player = collision.gameObject.GetComponent<Rigidbody2D>();
            player.linearVelocity/=2;
            player.linearVelocity = new Vector2(Mathf.Clamp(player.linearVelocityX, -5.0f, 5.0f), Mathf.Clamp(player.linearVelocityY, -5.0f, 5.0f));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {

    }

    void Awake()
    {
        rbSelf = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GameObject.Find("Camera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
