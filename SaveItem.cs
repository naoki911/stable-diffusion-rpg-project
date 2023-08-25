using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SaveItem
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public string itemExplain;

        public ItemData(string itemName, string itemExplain)
        {
            this.itemName = itemName;
            this.itemExplain = itemExplain;
        }
    }

    [System.Serializable]
    public class ItemDataList
    {
        public List<ItemData> items = new List<ItemData>();
    }

    private static string ApiDataListToJson(ItemDataList dataList)
    {
        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }

    private static ItemDataList JsonToApiDataList(string json)
    {
        return JsonUtility.FromJson<ItemDataList>(json);
    }

    public static void SaveItemData(ItemData apiData)
    {
        ItemDataList dataList = LoadItemDataList();
        dataList.items.Add(apiData);

        string json = ApiDataListToJson(dataList);
        string path = Path.Combine(Application.persistentDataPath, "Item_Data.json");
        File.WriteAllText(path, json);
    }

    public static List<ItemData> LoadItemData()
    {
        ItemDataList dataList = LoadItemDataList();
        return dataList.items;
    }

    private static ItemDataList LoadItemDataList()
    {
        string path = Path.Combine(Application.persistentDataPath, "Item_Data.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonToApiDataList(json);
        }
        else
        {
            return new ItemDataList();
        }
    }
}
