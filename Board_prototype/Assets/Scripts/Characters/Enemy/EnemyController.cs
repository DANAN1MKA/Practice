using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    private SignalBus signalBus;
    public void setupEnemy(SignalBus _signalBus, int _healthAmount)
    {
        signalBus = _signalBus;
        healthAmount = _healthAmount;


        signalBus.Subscribe<CheracterAttackSignal>(recieveDamage);
    }

    Animator anim;

    private int healthAmount = 50;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void recieveDamage(CheracterAttackSignal signal)
    {
        //TODO: отладка
        Debug.Log(healthAmount);

        healthAmount -= signal.damageAmount;
        if (healthAmount < 1) Die();
        else Hurt();
    }

    public void Idle()
    {
        anim.SetTrigger("idle");
    }
    public void Die()
    {
        anim.SetTrigger("die");

        Destroy(gameObject, 3f);

        signalBus.Unsubscribe<CheracterAttackSignal>(recieveDamage);
    }
    public void Hurt()
    {
        anim.SetTrigger("hurt");
    }
}
