using UnityEngine;

[CreateAssetMenu(fileName = "DecorationInfo", menuName = "Scriptable Objects/Decoration Info")]
public class DecorationInfo : ScriptableObject, IWeighted
{
    public GameObject prefab;
    [field: SerializeField] public float Weight { get; set; }
}
