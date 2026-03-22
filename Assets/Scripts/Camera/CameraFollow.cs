using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform player;
    private bool isDragging = false;

    [SerializeField]
    public float normalTime = 0.5f;
    [SerializeField]
    public float dragTime = 1f;

    private Vector3 velocity = Vector3.zero;


    void LateUpdate()
    {
        float currentSmoothTime = isDragging ? dragTime : normalTime;

        // Where the camera needs to go (basically the player's position, ignoring Z value)
        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        // Lerp from the current camera position to the target position in the specified amount of time
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            currentSmoothTime
        );
    }

    public void setSpeed(float normalSpeed)
    {
        this.normalTime = normalSpeed;
    }

    public void resetSpeed()
    {
        this.normalTime = 0.5f;
    }

    public void setDragging(bool isDragging)
    {
        this.isDragging = isDragging;
    }

}
