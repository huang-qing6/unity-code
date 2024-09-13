using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider2D;
    PlayerMovement Player;
    Rigidbody2D rb;
    [Header("检测参数")]
    public bool manual;
    public bool isPlayer;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    public float checkRaduis;
    
    public LayerMask groundLayer;



    [Header("状态")]
    public bool isGround;

    public bool touchLeftWall;

    public bool touchRightWall;

    public bool onWall;
    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();   

        if (!manual) //不手动调整默认设置一个检测器
        {
            rightOffset = new Vector2((capsuleCollider2D.bounds.size.x + capsuleCollider2D.offset.x) / 2, capsuleCollider2D.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }


        if (isPlayer) 
        {
            Player = GetComponent<PlayerMovement>();    
        }
    }
    private void Update()
    {
        Check();
    }
    public void Check()
    {
        //检测地面
        if (onWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);
        }
        //墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis, groundLayer);
        //墙上
        if (isPlayer) 
            onWall = ((touchLeftWall && Player.inputDirection.x < 0f) || (touchRightWall && Player.inputDirection.x > 0f)) && rb.velocity.y < 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x, leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x, rightOffset.y), checkRaduis);
    }

}
