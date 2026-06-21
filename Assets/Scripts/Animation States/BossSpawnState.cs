using UnityEngine;

public class BossSpawnState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossController controller = animator.GetComponentInParent<BossController>();
        if (controller != null)
            controller.SetSpawning(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossController controller = animator.GetComponentInParent<BossController>();
        if (controller != null)
            controller.SetSpawning(false);
    }
}
