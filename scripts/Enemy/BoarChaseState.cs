using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BoarChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("chase");
        currentEnemy.currentSpeed = currentEnemy.ChaseSpeed;
        currentEnemy.anim.SetBool("run", true);
    }  
    public override void LogicUpdate()
    {
        if(currentEnemy.loseTimeCounter <= 0) 
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        moveDir = (target - currentEnemy.transform.position).normalized;


        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.loseTimeCounter = currentEnemy.loseTime;
        currentEnemy.anim.SetBool("run", false);
    }
}