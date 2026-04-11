using UnityEngine;
using System.Collections;

public class ResourceObject : MonoBehaviour
{
    public Transform visuals;
    [Header("Particles")]
    public MeshRenderer particlesSource;
    public ParticleSystem breakParticles;
    public Material particleMaterial;
    public Color particleColor = Color.white;
    [Header("Information")]
    public ResourceData data;
    public float health = 12f;
    public int amountPerHit = 1;
    [Header("Shake Settings")]
    public float shakeDuration = 0.4f;
    public float shakeMagnitude = 0.2f;
    public float dampingSpeed = 2f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (visuals == null) visuals = transform;
        originalPosition = visuals.localPosition;
    }

    public int Damage(float val)
    {
        health -= val;

        if (health <= 0f)
        {
            float time = 0f;
            if (breakParticles && particlesSource)
            {
                gameObject.SetActive(false);
                time = PlayBreakParticles();
            }

            Destroy(gameObject, time);
        }
        else
            Shake();

        return amountPerHit;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    float PlayBreakParticles()
    {
        ParticleSystem particles = Instantiate(breakParticles);

        ParticleSystem.MainModule main = particles.main;
        ParticleSystem.ShapeModule shape = particles.shape;
        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();

        main.startColor = particleColor;
        shape.meshRenderer = particlesSource;
        if(particleMaterial) renderer.material = particleMaterial;

        particles.Play();

        return main.duration + main.startLifetime.constantMax;
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            if (Time.timeScale > 0f)
            {
                float damper = 1.0f - Mathf.Clamp01(elapsed / shakeDuration);
                float x = (Random.value * 2f - 1f) * shakeMagnitude * damper;
                float y = (Random.value * 2f - 1f) * shakeMagnitude * damper;

                visuals.localPosition = originalPosition + new Vector3(x, y, 0f);

                elapsed += Time.deltaTime * dampingSpeed;
            }
            yield return null;
        }

        visuals.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}