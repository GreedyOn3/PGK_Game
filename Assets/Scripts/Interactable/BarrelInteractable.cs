using UnityEngine;

public class BarrelInteractable : InteractableBase
{
    [SerializeField] private float resourceChance = 0.5f;
    [Header("Resources")]
    [SerializeField] private ParticleSystem resourceParticles;
    [SerializeField] private int resourceAmount = 5;
    [Header("XP")]
    [SerializeField] private GameObject xpOrbPrefab;
    [SerializeField] private int xpOrbsAmount = 5;
    [SerializeField] private float pushForce = 2.5f;

    public override void OnInteract(PlayerReferences player)
    {
        if (Random.value < resourceChance)
        {
            PlayResourceParticles(player);
            player.Stats.resourceGathered += resourceAmount;
        }
        else
        {
            for (int i = 0; i < xpOrbsAmount; i++)
            {
                Rigidbody orbRigidbody = Instantiate(xpOrbPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
                Vector3 randomDir = Random.onUnitSphere;

                if(orbRigidbody) orbRigidbody.AddForce(randomDir*pushForce, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    void PlayResourceParticles(PlayerReferences player)
    {
        ParticleSystem particles = Instantiate(resourceParticles, transform.position, transform.rotation);
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();

        if (player.ResourceMaterial) renderer.material = player.ResourceMaterial;

        particles.Play();
    }
}
