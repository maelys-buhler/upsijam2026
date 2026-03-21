using UnityEngine;

public class rainElement : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "endOfRain"){
            Debug.Log("deleting rain");
            Destroy(gameObject);
        }
        if(other.name == "Player"){
            GameObject.Find("Player").GetComponent<Player>().death();
        }
    }
}
