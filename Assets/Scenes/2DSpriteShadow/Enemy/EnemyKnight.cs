using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnight : EnemyBase
{
    [System.Serializable]
    public class EnemyAnimators
    {
        [SerializeField] Animator bladeAnimator = null;
        public Animator BladeAnimator => bladeAnimator;
        [SerializeField] Animator legsAnimator = null;
        public Animator LegsAnimator => legsAnimator;
        [SerializeField] Animator knightAnimator = null;
        public Animator KnightAnimator => knightAnimator;
    }
    [SerializeField] EnemyAnimators enemyAnimators;

    protected override void AnimateFactory(EEnemyAnimState eEnemyAnimState)
    {
        switch (eEnemyAnimState)
        {
            case EEnemyAnimState.Trace:
                enemyAnimators.LegsAnimator.Play("LegsWalk");
                break;
            case EEnemyAnimState.AttackState_1:
                enemyAnimators.BladeAnimator.SetInteger("AttackState", 1);
                enemyAnimators.BladeAnimator.SetTrigger("Attack");
                break;
            case EEnemyAnimState.AttackState_2:
                enemyAnimators.BladeAnimator.SetInteger("AttackState", 2);
                enemyAnimators.BladeAnimator.SetTrigger("Attack");
                break;
            case EEnemyAnimState.Dead:
                enemyAnimators.LegsAnimator.enabled = false;
                enemyAnimators.BladeAnimator.enabled = false;

                GetComponent<RandomExplode>().RandomExlpode();
                Player.player.AddExp(10);
                break;
            case EEnemyAnimState.Hit:
                enemyAnimators.KnightAnimator.Play("KnightHit");
                break;
            case EEnemyAnimState.Jump:

                break;
            default:
                break;
        }
    }

}
