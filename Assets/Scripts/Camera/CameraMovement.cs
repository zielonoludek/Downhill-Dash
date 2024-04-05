using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;

    [SerializeField] private float cameraYPos = 7.5f;
    [SerializeField] private float maxDistance  = 15;
    [SerializeField] private float damping = 10;

    private void Start()
    {
        player = FindFirstObjectByType<SimulatedPlayer>().transform;
    }
    void FixedUpdate()
    { 
        Vector3 direction = (player.position - transform.position).normalized ;
        Vector3 height = new Vector3(transform.position.x, player.position.y + cameraYPos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, height, Time.deltaTime);
        float distance = Mathf.Abs(Vector3.Distance(transform.position, player.position));

        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime);
        if (distance > maxDistance) transform.position += (distance - maxDistance) * direction;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 1000 * damping);
    }
}