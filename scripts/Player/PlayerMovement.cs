using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region ��������
    [Header("�¼�")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO backToMenuEvent;


    public PlayerInputControl inputControl;
    public Vector2 inputDirection;
    private PhysicsCheck physicscheck;
    private PlayerAnimation playeranim;
    private CapsuleCollider2D capsuleCollider;
    private Vector2 origincalOffset;
    private Vector2 oridinalSize;
    private Rigidbody2D rb;
    private Character character;
    
    [Header("��������")]
    public float speed;
    private float runSpeed;

    private float speedLimit; 

    public float jumpForce;

    private float walkSpeed;

    public float slideSpeed;

    public float wallJumpForce;

    public int slidePowerCost;
    [Header("״̬")]
    public bool isCrouch;
    
    public bool isHurt;

    public float HurtForce;

    public float slideDistence;

    public bool isDead;

    public bool isAttack;

    public bool wallJump;

    public bool isSlide;

    [Header("�������")]
    public PhysicsMaterial2D normal;

    public PhysicsMaterial2D wall;

    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicscheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playeranim = GetComponent<PlayerAnimation>();   
        character = GetComponent<Character>();  

        origincalOffset = capsuleCollider.offset;
        oridinalSize = capsuleCollider.size;

        inputControl = new PlayerInputControl();

        inputControl.GamePlay.Jump.started += Jump;

        //����
        inputControl.GamePlay.Slide.started += Slide;

        #region ǿ����·

        //��Ҫ��speedlimit��

        walkSpeed = speed;
        runSpeed = walkSpeed * 2;
        speedLimit = speed * 2;
        inputControl.GamePlay.MoveButton.performed += ctx => 
        {
            if (physicscheck.isGround)
                speed = runSpeed;
        };

        inputControl.GamePlay.MoveButton.canceled += ctx =>
        {
            if (physicscheck.isGround)
                speed = walkSpeed;
        };
        #endregion


        //������
        inputControl.GamePlay.Attack.started += PlayerAttack;

        //��Ϸ������ɽ����ƶ�
        inputControl.Enable();
    }


    private void OnEnable()
    {

        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }



    private void OnDisable()
    {
        inputControl?.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }



    private void Update()
    {
        if(speed > speedLimit) speed = speedLimit;  
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
            Move();


    }



    //����
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.name);
    //}
    //��������ֹͣ����
    private void OnLoadDataEvent()
    {
        isDead = false; 
    }    
    
    private void OnLoadEvent(GameSceneSO sO, Vector3 vector, bool arg3)
    {
        inputControl.GamePlay.Disable();    
    }
    //���ؽ�����������
    private void OnAfterSceneLoadEvent()
    {
        inputControl.GamePlay.Enable();
    }
    public void Move()
    {
        if(!isCrouch && !wallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime,rb.velocity.y);
        
        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        //��ת��ɫ
        transform.localScale = new Vector3(faceDir, 1, 1);

        isCrouch = inputDirection.y < -0.5f && physicscheck.isGround;

        if (isCrouch)//�޸���ײ���С
        {
            capsuleCollider.offset = new Vector2(-0.05f,0.85f);
            capsuleCollider.size = new Vector2(0.7f,1.7f);
        }
        else 
        {
            capsuleCollider.offset = origincalOffset;
            capsuleCollider.size = oridinalSize;
        }
    }


    private void Slide(InputAction.CallbackContext obj)
    {
        if (!isSlide && physicscheck.isGround && character.currentPower >= slidePowerCost)
        {
            isSlide = true;

            var targetPos = new Vector3(transform.position.x + slideDistence * transform.localScale.x, transform.position.y);


            gameObject.layer = LayerMask.NameToLayer("enemy");
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (!physicscheck.isGround)
                break;

            if ((physicscheck.touchLeftWall && transform.localScale.x < 0f) || (physicscheck.touchRightWall && transform.localScale.x > 0f))
            {
                isSlide = false;
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        } while (MathF.Abs(target.x - transform.position.x) > 0.1f);
        isSlide = false;
        gameObject.layer = LayerMask.NameToLayer("player");
    }
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("jump");
        if (physicscheck.isGround)
        {
            if(physicscheck.isGround)rb.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);

            //��ϻ���
            isSlide=false;
            StopAllCoroutines();    
        }
        else if (physicscheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2f) * wallJumpForce,ForceMode2D.Impulse);
            //transform.localScale = new Vector3(transform.localScale.x, 1,1);    
            wallJump = true;
        }


    }
    #region unity event
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * HurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();

    }

    public void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (physicscheck.isGround)
        {
            playeranim.PlayerAttack();
            isAttack = true;
        }
    }

    #endregion

    public void CheckState()
    {
        if (isDead || isSlide)
            gameObject.layer = LayerMask.NameToLayer("enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("player");

        capsuleCollider.sharedMaterial = physicscheck.isGround ? normal : wall;

        if(physicscheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);


        if(wallJump && rb.velocity.y < 0)
            wallJump = false;   
    }

}
