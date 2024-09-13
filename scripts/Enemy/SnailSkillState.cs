using UnityEngine;
public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;   
        currentEnemy.currentSpeed = currentEnemy.ChaseSpeed;
        currentEnemy.anim.SetBool("walk", false);
        currentEnemy.anim.SetBool("hide", true);
        currentEnemy.anim.SetTrigger("skill");


        currentEnemy.loseTimeCounter = currentEnemy.loseTime;

        currentEnemy.GetComponent<Character>().invulnerable = true;
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.loseTimeCounter;
    }    
    public override void LogicUpdate()
    {
        if(currentEnemy.loseTimeCounter<=0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }

        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.loseTimeCounter;
    }
    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false);
        currentEnemy.GetComponent<Character>().invulnerable = true;
    }
}
