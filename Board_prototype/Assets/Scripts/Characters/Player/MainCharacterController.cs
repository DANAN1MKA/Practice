using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void stopRuning()
    {
        anim.SetBool("isRun", false);
    }
    public void run()
    {
        anim.SetBool("isRun", true);
    }
    public void attack()
    {
        anim.SetTrigger("attack");
    }
}
