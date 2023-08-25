using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveAPI
{
    [System.Serializable]
    public class ApiData
    {
        public string chatgpt_api;


        public ApiData(string chatgpt_api)
        {
            this.chatgpt_api = chatgpt_api;
        }
    }
    private static string ApiDataToJson(ApiData apiData)
    {
        return JsonConvert.SerializeObject(apiData, Formatting.Indented);
    }

    private static ApiData JsonToApiData(string json)
    {
        return JsonUtility.FromJson<ApiData>(json);
    }

    public static void SaveApiData(ApiData apiData)
    {
        string json = ApiDataToJson(apiData);
        string path = Path.Combine(Application.persistentDataPath,"API_Data.json");
        File.WriteAllText(path, json);
    }

    public static ApiData LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "API_Data.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonToApiData(json);
        }
        else
        {
            return new ApiData("");
        }
    }
}
