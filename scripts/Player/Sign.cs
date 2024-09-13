using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
public class Sign : MonoBehaviour
{
    private PlayerInputControl player;
    public Transform playerTrans;
    public GameObject signSprite;
    private Interactable targetItem;
    public bool canPress;
    Animator animator;

    private void Awake()
    {
        animator = signSprite.GetComponent<Animator>();
        player = new PlayerInputControl();
        player.Enable();

    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        player.GamePlay.Confirm.started += OnConfirm;
    }
    private void OnDisable()
    {
        canPress = false;   
    }
    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;   
        signSprite.transform.localScale = playerTrans.localScale;
    }
    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }

    private void OnActionChange(object obj,InputActionChange actionChange)
    {
        if(actionChange == InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);
            var d = ((InputAction)obj).activeControl.device;

            
            switch (d.device)
            {
                case Keyboard:
                    animator.Play("keyboard");
                    break;
                case DualShockGamepad:
                    animator.Play("ps");
                    break;
            }
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            canPress = true;    
            targetItem = collision.GetComponent<Interactable>();  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPress = false;
    }

}
