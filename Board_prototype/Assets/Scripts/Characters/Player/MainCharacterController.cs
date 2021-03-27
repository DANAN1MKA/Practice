using UnityEngine;
using Zenject;

public class MainCharacterController : MonoBehaviour
{
    private SignalBus signalBus;

    public void setSignalBus(SignalBus _signalBus)
    {
        signalBus = _signalBus;
        signalBus.Subscribe<CheracterAttackSignal>(Attack);
        signalBus.Subscribe<MoveEnemyCompliteSignal>(stopRuning);
        //.Subscribe<NewEnemySignal>(Run);

        Run();
    }

    Animator anim;


    public void stopRuning()
    {
        anim.SetBool("isRun", false);
    }
    public void Run()
    {
        anim.SetBool("isRun", true);
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Idle()
    {
        anim.SetTrigger("idle");
    }
    public void Attack()
    {
        anim.SetTrigger("attack");
    }
    public void TripOver()
    {
        anim.SetTrigger("tripOver");
    }
    public void Hurt()
    {
        anim.SetTrigger("hurt");
    }
    public void Die()
    {
        anim.SetTrigger("die");
    }
    public void LookUp()
    {
        if (!anim.GetBool("isLookUp"))
        {
            anim.SetBool("isLookUp", true);
        }
        else
        {
            anim.SetBool("isLookUp", false);
        }
    }
    public void Jump()
    {
        if (!anim.GetBool("isJump"))
        {
            anim.SetBool("isJump", true);
        }
        else
        {
            anim.SetBool("isJump", false);
        }
    }
}
