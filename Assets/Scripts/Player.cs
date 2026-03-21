using UnityEngine;

public class Player : MonoBehaviour
{

    private int currentLevel = 0;
    private bool leftKey = false;
    private bool rightKey = false;
    private bool upKey = false;

    public Vector3 GetCurrentLevelSpawnPoint()
    {
        return GameObject.Find("SpawnPoint"+currentLevel).transform.position;
    }

    public void ResetAtCurrentLevelSpawnPoint()
    {
        transform.position = GetCurrentLevelSpawnPoint();
        foreach (var item in FindObjectsByType<ClickableObject>(FindObjectsSortMode.None))
        {
            item.ResetLocation();
        }
    }

    public void NextLevel()
    {
        currentLevel++;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null || collision.gameObject == null)
        {
            return;
        }
        if(collision.gameObject.name == "Deadline")
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
