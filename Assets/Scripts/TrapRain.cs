using UnityEngine;

public class TrapRain : MonoBehaviour
{
    private float chrono;
    public float intervalBetweenObjectAppearance;
    public Sprite sprite;
    public float rainSpeed; 
    public float width = 5;
    public float depth = 10;
    int layer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private bool isActive = false;
    void Start()
    {
        layer = LayerMask.NameToLayer("Rain");
        chrono = 0f;
        GameObject endOfRain = new GameObject("endOfRain");
        endOfRain.AddComponent<BoxCollider2D>();
        endOfRain.GetComponent<BoxCollider2D>().name = "endOfRain";
        endOfRain.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - depth);
        endOfRain.GetComponent<BoxCollider2D>().size = new Vector2(width, 2f);
        endOfRain.layer = layer;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }
        chrono += Time.deltaTime;
        if(chrono >= intervalBetweenObjectAppearance){
            chrono = 0f;
            makeRainAppear();
        }
    }

    void makeRainAppear(){
        float appearPlace  = Random.Range(1f, width-1f);
        Vector2 appearPos = new Vector2(transform.position.x - (width/2) + appearPlace, transform.position.y);
        GameObject newElement = new GameObject("rainBlock");
        newElement.transform.position = appearPos;
        newElement.transform.parent = this.gameObject.transform;
        newElement.AddComponent<Rigidbody2D>();
        newElement.AddComponent<SpriteRenderer>();
        newElement.GetComponent<SpriteRenderer>().sprite = sprite;
        newElement.GetComponent<Rigidbody2D>().gravityScale = 0.09f;
        newElement.GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        newElement.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        newElement.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        newElement.AddComponent<BoxCollider2D>();
        newElement.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0.5f);
        newElement.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1.3f);
        newElement.layer = layer;
        newElement.GetComponent<BoxCollider2D>().isTrigger = true;
        newElement.AddComponent<rainElement>();
    }
}
