using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chat : MonoBehaviour
{
    public class ItemExplain{
        public string itemName;
        public string explain;
    }
    private ItemExplain itemExplain;

    private string apiKey; // NOTE: 入力したままコミットやリポジトリの公開などをしないこと

    OpenAIChatCompletionAPI chatCompletionAPI;
    public StableDiffusionText2Image stable;
    public string tmp;
    private string intoJapanese = @"これからいうゲームのアイテム名の英単語とそのアイテムの
    タグをいうのでこのアイテムに適した日本語名を一言で教えてください。
    また、その名前からイメージしてそのアイテムについて50字以上100字以内で説明してください。
    これらの情報をjson形式で表示して。
    {
        ""itemName""
        ""explain"":(50字以上100字以内)
    }";
    private string extraChara = @"I am having trouble naming an item for a game, so please come up with one fictional name idea for the object I am about to mention. Also, to be able to illustrate this name, please use only 8 English words to describe the color, shape, similarity, and other characteristics of the object.";
    private string returnstr;
    List<OpenAIChatCompletionAPI.Message> context;

    void Awake()
    {
        apiKey = SaveAPI.LoadPlayerData().chatgpt_api;
        chatCompletionAPI = new OpenAIChatCompletionAPI(apiKey);
        context = new List<OpenAIChatCompletionAPI.Message>()
        {
            new OpenAIChatCompletionAPI.Message(){role = "system", content = extraChara}
        };
    }

    void Start(){
        //apiKey = SaveAPI.LoadPlayerData().chatgpt_api;
        //GPTReply(tmp);
    }

    public void GPTReply(string addContent){
        StartCoroutine(ChatCompletionRequest(addContent));
    }

    IEnumerator ChatCompletionRequest(string addContent)
    {
        context = new List<OpenAIChatCompletionAPI.Message>()
        {
            new OpenAIChatCompletionAPI.Message(){role = "system", content = extraChara}
        };
        var message = new OpenAIChatCompletionAPI.Message() { role = "user", content = addContent };
        context.Add(message);

        var request = chatCompletionAPI.CreateCompletionRequest(
            new OpenAIChatCompletionAPI.RequestData() { messages = context }
        );

        yield return request.Send();

        if (request.IsError) throw new System.Exception(request.Error);

        var message2 = request.Response.choices[0].message;
        Debug.Log(message2.content);
        
        
        StartCoroutine(IntoJapanese(message2.content));
        yield return null;
        stable.Generate("1 "+message2.content+",simple");
    }

    IEnumerator IntoJapanese(string itemName){
        context = new List<OpenAIChatCompletionAPI.Message>()
        {
            new OpenAIChatCompletionAPI.Message(){role = "system", content = intoJapanese}
        };
        var message = new OpenAIChatCompletionAPI.Message() { role = "user", content = itemName };
        context.Add(message);

        var request = chatCompletionAPI.CreateCompletionRequest(
            new OpenAIChatCompletionAPI.RequestData() { messages = context }
        );

        yield return request.Send();

        if (request.IsError) throw new System.Exception(request.Error);

        var message2 = request.Response.choices[0].message;
        itemExplain = JsonUtility.FromJson<ItemExplain>(message2.content);
        Debug.Log(message2.content);
        SaveItem.SaveItemData(new SaveItem.ItemData(itemExplain.itemName,itemExplain.explain));
    }

    public ItemExplain GetItemExplain(){
        return itemExplain;
    }
}