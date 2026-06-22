using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : InteractableBase
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject shopParticles;

    private int _purchaseCounter = 0;

    private void Start()
    {
        PlayParticles();
    }

    public override void OnInteract(PlayerReferences player)
    {
        LevelUpSystem levelUpSystem = LevelUpSystem.Instance;
        if (!levelUpSystem) return;

        List<LevelUpChoice> randomChoices = levelUpSystem.GenerateLevelUp(player.Inventory, 2);
        if (randomChoices.Count > 0)
            ShopUI.Instance.Show(this, randomChoices);
    }

    public void Finish()
    {
        InteractionEnabled = false;
        if (animator)
        {
            int reaction = 2;

            if (_purchaseCounter > 2)
                reaction = 3;
            else if (_purchaseCounter > 0)
                reaction = 1;

            animator.SetInteger("Reaction", reaction);
            animator.SetTrigger("React");
        } 
        else
        {
            PlayParticles();
            Destroy(gameObject);
        }
    }

    public void PlayParticles()
    {
        if (shopParticles != null)
            Instantiate(shopParticles, transform.position, Quaternion.identity);
    }

    public void IncreasePurchaseCounter() => _purchaseCounter++; 
}
