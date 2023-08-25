using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Menu : MonoBehaviour
{

    [SerializeField]private GameObject menu,player;
    private int homeButtonCount,itemButtonCount;
    public Transform homeButtonList;
    private AudioSource audioSource;
    public AudioClip select,close;
    public bool homeBool,item,status,picturebook,manual;
    public GameObject itemUI,manualUI;
    public GameObject itemNamePrefab;
    public Transform itemNameList;
    public LoadItemImage loadItemImage;
    

    void Start(){
        FocusButton(homeButtonList,homeButtonCount);
        audioSource = GetComponent<AudioSource>();
    }


    public void PushUp(InputAction.CallbackContext context){
        HomeSelect(-1,context);
    }

    public void PushDown(InputAction.CallbackContext context){
        HomeSelect(1,context);
    }

    private void HomeSelect(int i,InputAction.CallbackContext context){
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        if(homeBool){
            homeButtonCount = SelectButton(homeButtonList,homeButtonCount+i);
            FocusButton(homeButtonList,homeButtonCount);
            audioSource.PlayOneShot(select);
        }else if(item){
            itemButtonCount = SelectButton(itemNameList,itemButtonCount+i);
            FocusButton(itemNameList,itemButtonCount);
            audioSource.PlayOneShot(select);
            loadItemImage.LoadImageAndSetToImageComponent(SaveItem.LoadItemData()[itemButtonCount].itemName,SaveItem.LoadItemData()[itemButtonCount].itemExplain);
        }
    }

    private int SelectButton(Transform buttonList,int buttonCount){
        int amountButton = buttonList.childCount;
        int tmp = (amountButton + buttonCount) % amountButton;
        Debug.Log("セレクトボタン"+tmp);
        return tmp;
    }

    private void FocusButton(Transform buttonList,int buttonCount){
        for(int i = 0;i < buttonList.childCount;i++){
            if(i == buttonCount){
                buttonList.GetChild(i).localScale = new Vector3(1.1f,1.1f,1.1f);
            }else{
                buttonList.GetChild(i).localScale = Vector3.one;
            }
        }
    }

    public void OpenMenu(InputAction.CallbackContext context){
        if (!context.performed) return;
        Debug.Log("Open");
        menu.SetActive(true);
        player.SetActive(false);
        homeBool = true;
    }

    public void CloseMenu(InputAction.CallbackContext context){
        if(homeBool){
            if (!context.performed) return;
            
            player.SetActive(true);
            homeBool = false;
            audioSource.PlayOneShot(close);
            menu.SetActive(false);
        }
    }

    public void PushCircle(InputAction.CallbackContext context){
        if(!context.performed) return;
        if(homeBool){
            switch(homeButtonCount){
            case 0:
                ItemOpen();
                break;
            case 1:
                StatusOpen();
                break;
            case 2:
                PictureBookOpen();
                break;
            case 3:
                ManualOpen();
                break;
            }
        }
    }
    private void ItemOpen(){
        itemUI.SetActive(true);
        item = true;
        homeBool = false;
        foreach(Transform child in itemNameList){
            Destroy(child.gameObject);
        }
        foreach(SaveItem.ItemData itemData in SaveItem.LoadItemData()){
            GameObject itemPrefab = Instantiate(itemNamePrefab,itemNameList);
            itemPrefab.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
        }
        
    }
    private void StatusOpen(){

    }
    private void PictureBookOpen(){

    }
    private void ManualOpen(){
        manualUI.SetActive(true);
        manual = true;
        homeBool = false;
    }

    public void PushCross(InputAction.CallbackContext context){
        if(!context.performed) return;
        if(manual||item){
            manualUI.SetActive(false);itemUI.SetActive(false);
            homeBool = true;
            manual = false;item = false;
        }
    }

    
}
