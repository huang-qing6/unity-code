using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicscheck;
    private PlayerMovement playermovement;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicscheck = GetComponent<PhysicsCheck>();
        playermovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicscheck.isGround);
        anim.SetBool("isCrouch", playermovement.isCrouch);
        anim.SetBool("isDead", playermovement.isDead);
        anim.SetBool("isAttack", playermovement.isAttack);
        anim.SetBool("onwall", physicscheck.onWall);
        anim.SetBool("isSlide", playermovement.isSlide);    
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
