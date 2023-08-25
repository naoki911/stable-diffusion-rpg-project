
using UnityEngine;


public class WarpPoint : MonoBehaviour
{
    public string destinationName;
    public AudioClip warpSound;
    private WarpManager warpManager;
    private WarpUIController warpUIController;
    

    private void Start()
    {
        warpManager = FindObjectOfType<WarpManager>();
        warpUIController = FindObjectOfType<WarpUIController>();
    }

    // Get the warp destination
    public Transform GetWarpDestination(Transform warpPoint)
    {
        if (warpManager.warpPointsDictionary.TryGetValue(warpPoint, out Transform warpDestination))
        {
            //warpManager.SetWarpDestination(warpDestination);
            Debug.Log(warpDestination);
            return warpDestination;
        }
        return null;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warpManager.SetWarpDestination(GetWarpDestination(this.transform));
            warpUIController.ShowDestination(destinationName);
            AudioManager.instance.PlayClip(warpSound); // Use the AudioManager to play the sound
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warpManager.OutOfWarpDestination();
            warpUIController.HideDestination();
        }
    }
}
