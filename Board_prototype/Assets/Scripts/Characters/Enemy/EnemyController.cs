using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    private SignalBus signalBus;
    public void setupEnemy(SignalBus _signalBus, int _healthAmount)
    {
        signalBus = _signalBus;
        healthAmount = _healthAmount;
    }

    Animator anim;

    [SerializeField] private int healthAmount;


    void Awake()
    {
        anim = GetComponent<Animator>();

        jump();
    }

    public void recieveDamage()
    {
        stopJump();

        Die();
    }

    public void Idle()
    {
        anim.SetTrigger("idle");
    }
    public void Die()
    {
        anim.SetTrigger("die");

        Destroy(gameObject, 0.5f);
    }
    public void Hurt()
    {
        anim.SetTrigger("hurt");
    }

    public void jump()
    {
        anim.SetBool("isJump", true);
    }
    public void stopJump()
    {
        anim.SetBool("isJump", false);
    }

}



