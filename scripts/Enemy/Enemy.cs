using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))] 
public class Enemy : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public PhysicsCheck physicsCheck;


    [Header("基本参数")]
    public float NormalSpeed;
    public float ChaseSpeed;
    [HideInInspector] public float currentSpeed;
    public Vector3 faceDir;
    public Transform attacker;
    public float hurtForce;
    public Vector3 spwanPoint;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;

    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;

    public float loseTime;
    public float loseTimeCounter;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    protected BaseState currentState;
    protected BaseState chaseState;
    protected BaseState patrolState;
    protected BaseState skillState;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();   
        

        currentSpeed = NormalSpeed;
        //waitTimeCounter = waitTime;
        spwanPoint = transform.position;    
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);    
        
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate();         
        if (!isHurt && !isDead && !wait)
            Move();
    }

    private void OnDisable()
    {
        currentState.OnExit();  
    }


    public virtual void Move()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("hide-recover") && !anim.GetCurrentAnimatorStateInfo(0).IsName("premove"))
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    
    public void TimeCounter()
    {
        if(wait)
        {
            waitTimeCounter -= Time.deltaTime;

            if(waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x,1, 1);
            }
        }

        if (!FoundPlayer() && loseTimeCounter>0)
        {
            loseTimeCounter -= Time.deltaTime;
        }
    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position+(Vector3)centerOffset,checkSize,0,faceDir,checkDistance,attackLayer);
    }

    public void SwitchState(NPCState state)//不确定传入变量状态，用枚举型变量
    {
        
        var newState = state switch  //var可以获得state现在是什么，类似auto
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            NPCState.Skill => skillState,
            _ => null
        };
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);

    }

    public virtual Vector3 GetNewPoint()
    {
        return transform.position;
    }


    #region 事件执行
    public void OnTakeDamage(Transform attackerTrans)
    {
        attacker = attackerTrans;
        //转身
        if(attackerTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if(attackerTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);



        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackerTrans.position.x,0).normalized;

        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));    
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir*hurtForce , ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    
    public void OnDie()
    {
        gameObject.layer = 2;
        rb.velocity = Vector2.zero; 
        anim.SetBool("dead", true);
        isDead = true;
    }


    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    #endregion


    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }
}
