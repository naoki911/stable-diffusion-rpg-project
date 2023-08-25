using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public float speed = 6.0f;
    public float dashSpeed = 12.0f;
    private float currentSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 moveInput;

    // Animatorを参照するための変数
    private Animator animator;
    public Chat chat;

    public GameObject itemExplain,gatherUI;
    public TextMeshProUGUI helptxt;

    // CharacterControllerを参照するための変数を追加
    private CharacterController characterController;

    // 移動可能な状態かを確認するブール変数
    private bool canMove = true;
    private const float stickTiltThreshold = 0.1f;

    // Singleton Instance
    public static PlayerController instance;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = speed;

        // CharacterControllerコンポーネントを取得
        characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        // Update move direction based on whether the stick is tilted or not
        if (moveInput.magnitude >= stickTiltThreshold)
        {
            moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentSpeed = dashSpeed;
        }
        else if (context.canceled)
        {
            currentSpeed = speed;
        }
    }

    private float gravity = 9.81f; // 重力の強さ
    private Vector3 velocity; // 垂直方向の速度を保持する

    void Update()
    {
        if(canMove)
        {
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;

            forward.y = 0;
            right.y = 0;
            
            forward.Normalize();
            right.Normalize();
            
            Vector3 movement = forward * moveDirection.z + right * moveDirection.x;

            // 重力の影響を加算
            velocity.y -= gravity * Time.deltaTime;

            // CharacterControllerのMove関数を用いて移動を適用
            characterController.Move((movement * currentSpeed + velocity)* Time.deltaTime);
            
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(movement);
            }


            animator.SetBool("isWalking", currentSpeed == speed && movement.magnitude > 0);
            animator.SetBool("isDashing", currentSpeed == dashSpeed && movement.magnitude > 0);
            
        }
    }

    public void OnGetRide(InputAction.CallbackContext context){
        animator.SetTrigger("RideBike");
    }

    // 外部からcanMoveを設定するための関数
    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    // ワープ先を設定するメソッドを追加
    public void WarpTo(Transform warpDestination)
    {
        characterController.enabled = false;
        transform.position = warpDestination.position;
        characterController.enabled = true;
    }

    public void CloseItemExplain(InputAction.CallbackContext context){
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        if(itemExplain.active){
            itemExplain.SetActive(false);
            GatherFinish();
        }
    }

    private void GatherFinish(){
        Debug.Log("finish");
        animator.SetTrigger("GatherFinish");
        SetCanMove(true);
    }


    public void OnGatherItem(InputAction.CallbackContext context){
        // 押された瞬間でPerformedとなる
        if (!context.performed) return;
        if(nearItem){
            chat.GPTReply(tmpTag);
            if(tmpTag == "Flower")animator.SetTrigger("Gather");
            else if(tmpTag == "Stone")animator.SetTrigger("Mining");
            else if(tmpTag == "Wood")animator.SetTrigger("Farm");
            SetCanMove(false);
            gatherUI.SetActive(false);
        }
        Debug.Log("アイテムゲット！");
    }

    private GameObject cloneObj;
    public Material coverMaterial;
    private bool nearItem;
    private string tmpTag;

    private void OnTriggerEnter(Collider other) {
        GameObject obj = other.gameObject;
        if(obj.tag == "Flower"||obj.tag =="Stone"||obj.tag =="Wood"){
            cloneObj = Instantiate(obj,obj.transform.position,obj.transform.rotation,obj.transform.parent);
            cloneObj.tag = "Untagged";
            cloneObj.GetComponent<Renderer>().material = coverMaterial;
            nearItem = true;
            tmpTag = obj.tag;
            HelpUI("採取");
        }else if(obj.tag == "Warp"){
            HelpUI("移動");
        }
    }

    private void OnTriggerExit(Collider other) {
        GameObject obj = other.gameObject;
        if(obj.tag == "Flower"||obj.tag =="Stone"||obj.tag =="Wood"){
            Destroy(cloneObj);
            nearItem = false;
            gatherUI.SetActive(false);
        }else if(obj.tag == "Warp"){
            gatherUI.SetActive(false);
        }
    }

    private void HelpUI(string str){
        gatherUI.SetActive(true);
        helptxt.text = str;
    }
}