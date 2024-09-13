using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;   //����˭�����
        currentEnemy.currentSpeed = currentEnemy.NormalSpeed;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())//���ֵ��˽���׷��
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //TODO ���Ʊ�Ե����
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else 
        {
            currentEnemy.anim.SetBool("walk", true);
            //if(currentEnemy.rb.velocity.x==0) currentEnemy.rb.velocity =new Vector2(currentEnemy.currentSpeed, currentEnemy.rb.velocity.y);
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
    }


}
