using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private InputManager inputManager;
    private SimulatedPlayer player;
    private string currentState;

    void Awake()
    {
        player = FindObjectOfType<SimulatedPlayer>();
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        currentState = player.GetState();
        animator.SetFloat("speed", player.GetPlayerSpeed());
        animator.SetFloat("swipeDir", inputManager.swipeDirection);

        if (currentState == "Crouching") animator.SetBool("isCrouching", true);
        else animator.SetBool("isCrouching", false);

        if (currentState == "Sliding") animator.SetBool("isSliding", true);
        else animator.SetBool("isSliding", false) ;
    }
}
