using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.FinalIK;

public class Bicycle : MonoBehaviour
{
    private bool isPlayerInRange;
    public GameObject player;
    [SerializeField] private AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other) {
        isPlayerInRange = false;
    }

    public void OnGetRide(InputAction.CallbackContext context){
        if(isPlayerInRange){
            transform.parent = player.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(0,90,0);
            player.GetComponent<FullBodyBipedIK>().enabled = true;
            aud.PlayOneShot(aud.clip);
        }
    }

    public void OnGetOff(InputAction.CallbackContext context){
        transform.parent = null;
        player.GetComponent<FullBodyBipedIK>().enabled = false;
    }
}
