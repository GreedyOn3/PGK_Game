using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    public int Count { get; set; }

    protected PlayerReferences _player;

    public void Init(PlayerReferences player)
    {
        _player = player;
        Count = 1;
    }
}
