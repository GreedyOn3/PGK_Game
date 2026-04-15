public class FreezeEffect : Effect
{
    private Movement _movement;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        if (_movement != null)
            _movement.enabled = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_movement != null)
            _movement.enabled = false;
    }

    private void OnDestroy()
    {
        if (_movement != null)
            _movement.enabled = true;
    }
}
