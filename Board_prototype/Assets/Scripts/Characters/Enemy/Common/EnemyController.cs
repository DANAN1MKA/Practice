using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        jump();
    }
    public void die()
    {
        anim.SetTrigger("die");

        Destroy(gameObject, 0.3f);
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



