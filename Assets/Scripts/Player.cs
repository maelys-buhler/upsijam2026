using UnityEngine;

public class Player : MonoBehaviour
{

    private int currentLevel = 0;
    private Vector3 defaultBtnResetLoc;

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
        defaultBtnResetLoc = GameObject.Find("BtnReset").transform.localPosition;
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
            GameObject.Find("BtnReset").GetComponent<ClickableObject>().isDragEnabled = true;
        }
        if(collision.gameObject.name == "BtnReset")
        {
            GameObject btnReset = GameObject.Find("BtnReset");
            btnReset.transform.localPosition = defaultBtnResetLoc;
            btnReset.GetComponent<ClickableObject>().isDragEnabled = false;
            NextLevel();
            ResetAtCurrentLevelSpawnPoint();
        }
        if(collision.gameObject.name.StartsWith("Item") && collision.gameObject.name.EndsWith("Arrow"))
        {
            collision.gameObject.SetActive(false);
            GameObject.Find(collision.gameObject.name.Substring(4)).GetComponent<SpriteRenderer>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

}
