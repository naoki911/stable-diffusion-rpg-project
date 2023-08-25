using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class HomeSelect : MonoBehaviour
{
    public Transform buttonList;
    private int homeButtonCount;
    private AudioSource soundsAudio;
    public AudioClip select;

    void Start(){
        soundsAudio = GetComponent<AudioSource>();
        FocusButton(homeButtonCount);
        
    }

    public void PushUp(InputAction.CallbackContext context){
        Select(-1,context);
    }

    public void PushDown(InputAction.CallbackContext context){
        Select(1,context);
    }
    private void Select(int i,InputAction.CallbackContext context){
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        homeButtonCount = SelectButton(homeButtonCount+i);
        FocusButton(homeButtonCount);
        soundsAudio.PlayOneShot(select);
    }

    public void DecisionButton(InputAction.CallbackContext context){
        if(!context.performed) return;
        switch(homeButtonCount){
            case 0:
                NewGame();
                break;
            case 1:
                LoadGame();
                break;
            case 2:
                Settings();
                break;
        }
    }

    private int SelectButton(int buttonCount){
        int amountButton = buttonList.childCount;
        int tmp = (amountButton + buttonCount) % amountButton;
        Debug.Log("セレクトボタン"+tmp);
        return tmp;
    }

    private void FocusButton(int buttonCount){
        for(int i = 0;i < buttonList.childCount;i++){
            Transform tmpButton = buttonList.GetChild(i);
            if(i == buttonCount){
                tmpButton.localScale = new Vector3(1.1f,1.1f,1.1f);
                tmpButton.GetComponent<TextMeshProUGUI>().enableVertexGradient = true;
            }else{
                tmpButton.localScale = Vector3.one;
                tmpButton.GetComponent<TextMeshProUGUI>().enableVertexGradient = false;
            }
        }
    }

    private void NewGame(){
        SceneChanger.LoadNextScene();
    }

    private void LoadGame(){

    }

    private void Settings(){

    }
}
