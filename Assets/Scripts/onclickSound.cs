using UnityEngine;
using UnityEngine.EventSystems;

public class onclickSound : MonoBehaviour, IPointerClickHandler
{
 
    private Player player;
    private bool requestJump = false;
   
    public void OnPointerClick( PointerEventData eventData ) {
        requestJump = true;
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update(){
        if (requestJump)
        {
            player.Jump(false);
            requestJump = false;
        }
    }
}
