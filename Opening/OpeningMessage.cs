using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SceneChanger))]
public class OpeningMessage : MonoBehaviour
{
    public float fadeDuration = 1.0f;

    public TextMeshProUGUI textComponent;
    private string[] messages = {"このゲームはChatGPTのAPIを使用します。","ご自身のChatGPT APIのAPIキーを取得して\n下のテキストボックスに入力してください。",
    "※注意\nこのゲームを遊ぶにはStable Diffusionを\nローカルで起動しておく必要があります。"
    ,"Stable Diffusionをダウンロードして、ご自身のパソコンで起動してください。"
    ,"またダウンロードしたファイルの中から「webui-user.bat」を右クリック>「その他のオプションを確認」> 「編集」で上の画像のように変更してください。"
    ,"ここからはゲームパッドを使用してのプレイになります。\n準備ができたら〇ボタンを押してください。"};
    private int messageCount;
    private AudioSource audioSource;
    public AudioClip next,transScene;
    public GameObject api_input,stableExplainImage;
    private TMP_InputField api;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeTextToFullAlpha(messages[messageCount]));
        api = api_input.GetComponent<TMP_InputField>();
        api.text = SaveAPI.LoadPlayerData().chatgpt_api;
    }

    private IEnumerator FadeTextToFullAlpha(string message)
    {
        Debug.Log(message);
        textComponent.text = message;
        audioSource.PlayOneShot(next);
        switch(messageCount){
            case 1:
                api_input.SetActive(true);
                break;
            case 2:
                api_input.SetActive(false);
                break;
            case 4:
                stableExplainImage.SetActive(true);
                break;
            case 5:
                stableExplainImage.SetActive(false);
                break;
            default:
                break;
        }
        if(messageCount == 1){
            api_input.SetActive(true);
        }else if(messageCount == 2){
            api_input.SetActive(false);
        }
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
        while (textComponent.color.a < 1.0f)
        {
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + (Time.deltaTime / fadeDuration));
            yield return null;
        }
    }

    public void NextMessage(InputAction.CallbackContext context){
        if (!context.performed) return;
        if(messageCount+1 < messages.Length){
            messageCount++;
            StartCoroutine(FadeTextToFullAlpha(messages[messageCount]));
        }else{
            //ゲームスタート
            audioSource.PlayOneShot(transScene);
            SceneChanger.LoadNextScene();
        }
    }

    public void BackMessage(InputAction.CallbackContext context){
        if (!context.performed) return;
        if(0 <= messageCount-1){
            messageCount--;
            StartCoroutine(FadeTextToFullAlpha(messages[messageCount]));
        }else{
            
        }
    }

    public void SetAPI(){
        SaveAPI.SaveApiData(new SaveAPI.ApiData(api.text));
    }
}
