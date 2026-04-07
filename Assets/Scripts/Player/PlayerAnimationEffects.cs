using UnityEngine;

public class PlayerAnimationEffects : MonoBehaviour
{
    public PlayerReferences references;
    [Header("Particles")]
    public ParticleSystem[] footstepParticles = new ParticleSystem[2];

    void OnFootstep(int footIndex)
    {
        if (!references.Controller.IsGrounded() || (footIndex != 0 && footIndex != 1)) return;

        if(footstepParticles[footIndex] && references.Movement.GetCurrentSpeed() > references.Movement.maxGroundSpeed - 0.1f)
            footstepParticles[footIndex].Play();
    }
}
