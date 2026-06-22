using UnityEngine;

public class ShopReactState : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ShopInteractable shop = animator.GetComponentInParent<ShopInteractable>();
        if(shop)
        {
            shop.PlayParticles();
            Destroy(shop.gameObject);
        }
    }
}
