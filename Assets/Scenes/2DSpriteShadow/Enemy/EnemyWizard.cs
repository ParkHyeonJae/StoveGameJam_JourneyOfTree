using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizard : EnemyBase
{
    [System.Serializable]
    public class EnemyAnimators
    {
        [SerializeField] Animator wizardAnimator = null;
        public Animator WizardAnimator => wizardAnimator;
    }
    [SerializeField] EnemyAnimators enemyAnimators;
    private EnemySkillSpawner enemySkillSpawner;
    public EnemySkillSpawner SkillSpawner
    {
        get
        {
            if (enemySkillSpawner == null)
                enemySkillSpawner = GetComponent<EnemySkillSpawner>();
            return enemySkillSpawner;
        }
    }


    protected override void AnimateFactory(EEnemyAnimState eEnemyAnimState)
    {
        switch (eEnemyAnimState)
        {
            case EEnemyAnimState.Idle:
                enemyAnimators.WizardAnimator.SetTrigger("idle");
                break;
            case EEnemyAnimState.Trace:
                enemyAnimators.WizardAnimator.SetBool("isRun", true);
                break;
            case EEnemyAnimState.AttackState_1:
                enemyAnimators.WizardAnimator.SetTrigger("attack");
                //SkillSpawner.Spawn();
                break;
            case EEnemyAnimState.Dead:
                //enemyAnimators.WizardAnimator.enabled = false;
                enemyAnimators.WizardAnimator.SetTrigger("die");
                GetComponent<RandomExplode>().RandomExlpode();
                Player.player.AddExp(10);

                break;
            case EEnemyAnimState.Hit:
                enemyAnimators.WizardAnimator.SetTrigger("hurt");
                break;
            case EEnemyAnimState.Jump:
                enemyAnimators.WizardAnimator.SetBool("isJump", true);
                break;
            default:
                enemyAnimators.WizardAnimator.SetBool("isJump", false);
                break;
        }
    }
}
