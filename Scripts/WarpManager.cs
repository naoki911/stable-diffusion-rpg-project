using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;


[System.Serializable]
public class WarpPointData
{
    public Transform warpPoint; // A unique identifier for each warp point
    public Transform warpDestination; // The warp destination corresponding to this warp point
}

public class WarpManager : MonoBehaviour
{
    [SerializeField]
    private List<WarpPointData> warpPointDataList = new List<WarpPointData>();
    
    public float fadeDuration = 2.0f; // フェードアウトにかかる時間
    public Image fadeImage;
    private bool isFading = false;
    public Dictionary<Transform, Transform> warpPointsDictionary;
    public PlayerController playerController;
    public WarpUIController warpUIController;
    private bool isPlayerInRange;

    // Initialize the warp points
    private void Awake()
    {
        warpPointsDictionary = new Dictionary<Transform, Transform>();

        // Populate the warp points dictionary
        foreach (WarpPointData warpPointData in warpPointDataList)
        {
            warpPointsDictionary.Add(warpPointData.warpPoint, warpPointData.warpDestination);
        }
    }
    

    public Image GetFadeImage(){
        return fadeImage;
    }
    public float GetDurationTime(){
        return fadeDuration;
    }
    
    public void WarpAction_performed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Debug.Log("移動");
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        if(isPlayerInRange){
            if (!isFading)
            {
                StartCoroutine(FadeOutRoutine());
            }
        }
    }

    private Transform warpDestination;
    public void SetWarpDestination(Transform warpDestination){
        this.warpDestination = warpDestination;
        isPlayerInRange = true;
        Debug.Log("セット");
    }

    public void OutOfWarpDestination(){
        this.warpDestination = null;
        isPlayerInRange = false;
    }

    private IEnumerator FadeOutRoutine()
    {
        if (warpDestination != null)
            {
                isPlayerInRange = false;
                playerController.SetCanMove(false);
                isFading = true;
                float elapsedTime = 0f;
                Color startColor = fadeImage.color;

                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Clamp01(elapsedTime / fadeDuration));
                    yield return null;
                }

                fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 1);
                
                PlayerController.instance.WarpTo(warpDestination);
                warpUIController.HideDestination();
                //warpUIController.HideDestination();
                Debug.Log("移動");
                yield return new WaitForSeconds(1);
                playerController.SetCanMove(true);
                playerController.gatherUI.SetActive(false);

                elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Clamp01(1-(elapsedTime / fadeDuration)));
                    yield return null;
                }
                fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0);
                //yield return StartCoroutine(FadeIn());
                isFading = false;
            }
        
    }
}

