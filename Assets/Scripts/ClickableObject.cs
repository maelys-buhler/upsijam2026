using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    private Vector3 offset;
    [SerializeField] private bool isEnabledWhileDragging = true;
    private CameraFollow mainCamera;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("rien xd");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Calcul du décalage entre souris et objet
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;

        offset = transform.position - mouseWorldPos;
        GetComponent<Collider2D>().enabled = false || isEnabledWhileDragging;
        mainCamera.setDragging(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0;

        transform.position = mouseWorldPos + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("drop");
        GetComponent<Collider2D>().enabled = true;
        mainCamera.setDragging(false);
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
