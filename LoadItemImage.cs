using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class LoadItemImage : MonoBehaviour
{
    public Image targetImage;
    public TextMeshProUGUI explaintxt,itemNametxt;
    public string png_adress;


    void Start() {
        //LoadImageAndSetToImageComponent("ささやきの柳");
    }

    public void LoadImageAndSetToImageComponent(string itemName,string explain) {
        string path = Path.Combine(png_adress,itemName+".png");
        
        if(File.Exists(path)) {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            
            if(texture.LoadImage(fileData)) {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                targetImage.sprite = sprite;
            }
            explaintxt.text = explain;
            itemNametxt.text = itemName;
        } else {
            Debug.LogError("File not found at: " + path);
        }
    }
}
