using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject PlayerHitParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            playerAnimator.SetTrigger("Hit");
            Player.player.currentHP -= Mathf.Clamp((10) - (Player.player.Defense / 2), 1, 20);
            var obj = Instantiate(PlayerHitParticle);
            obj.transform.position = transform.position;
            var particle01 = obj.transform.GetChild(0);
            var particle02 = obj.transform.GetChild(1);

            particle01.transform.position = transform.position;
            particle02.transform.position = transform.position;

            particle01.GetComponent<ParticleSystem>().Play();
            particle02.GetComponent<ParticleSystem>().Play();
            Destroy(obj, 1.0f);
        }
    }
}
