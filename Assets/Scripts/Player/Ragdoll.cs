using System.Collections;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject ragdollMesh;
    [SerializeField] private GameObject armature;
    [SerializeField] private float delay = 2f;
    
    private Rigidbody[] rigidbodies;
    
    private bool ragdollActive;

    Vector3 CameraOffset;
    private Animator animator;
    // private Animator animator;

    private IEnumerator EnableRagdollDelay() {
        Vector3 force = GetComponentInParent<SimulatedPlayer>().GetPlayerVelocity().normalized * 10;
        foreach (Rigidbody rigidbody in rigidbodies) {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(force, ForceMode.Impulse);
        } 
        animator.enabled = false;
        ragdollMesh.SetActive(true);
        playerMesh.SetActive(false);
        ragdollActive = true;
        
        yield return new WaitForSeconds(delay);
        
        DisableRagdoll();
        
        
    }
    private void Awake() {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DisableRagdoll();
    }

    // Use this to enable Ragdoll + implement 2 second delay:
    public void EnableRagdoll() {
        CameraOffset = Camera.main.transform.position - transform.position;
        Camera.main.GetComponent<OvershootCamPrototype>().SwitchTargetToRagdoll(armature.transform);
        StartCoroutine(EnableRagdollDelay());
    }
    
    public void DisableRagdoll() {
        foreach (Rigidbody rigidbody in rigidbodies) {
            rigidbody.isKinematic = true;
        } 
        animator.enabled = true;
        playerMesh.SetActive(true);
        armature.transform.position = transform.position;
        armature.transform.rotation = transform.rotation;
        ragdollMesh.SetActive(false);
        Camera.main.GetComponent<OvershootCamPrototype>().SwitchTargetToPlayer();
        GetComponentInParent<PlayerSpawnToCheckpoint>().cameraOffset = CameraOffset;
        GetComponentInParent<PlayerSpawnToCheckpoint>().RespawnPlayer();
        ragdollActive = false;
    }

    public bool IsRagdollActive() {
        return ragdollActive;
    }
}
