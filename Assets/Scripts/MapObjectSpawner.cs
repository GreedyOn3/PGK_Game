using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(LevelMapGenerator))]
public class MapObjectSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask;
    [Header("Prefabs")]
    [SerializeField] private GameObject barrelPrefab;
    [Header("Settings")]
    [SerializeField] private int barrelAmount = 15;

    LevelMapGenerator _mapGenerator;
    Bounds _mapBounds;

    public void Initialize(LevelMapGenerator mapGenerator)
    {
        _mapGenerator = mapGenerator;

        float width = _mapGenerator.mapSize * _mapGenerator.horizontalSpacing;
        float height = (_mapGenerator.GetMaxElevation() + 1) * _mapGenerator.verticalSpacing;
        float posXZ = (width - _mapGenerator.horizontalSpacing) / 2f;

        _mapBounds = new Bounds(new Vector3(posXZ, height / 2f, posXZ), new Vector3(width, height, width));

        if (!barrelPrefab) return;
        for(int i = 0; i < barrelAmount; i++)
        {
            Vector3 pos = Vector3.zero;
            Vector3 normal = Vector3.up;

            if (TryGetValidRandomPosition(barrelPrefab, out pos, out normal) && pos != Vector3.zero)
            {
                float yOffset = 0f;
                if (barrelPrefab.TryGetComponent<BoxCollider>(out BoxCollider col))
                    yOffset = col.size.y / 2f * barrelPrefab.transform.localScale.y;

                Instantiate(barrelPrefab, pos + (normal * yOffset), Quaternion.FromToRotation(Vector3.up, normal));
            }
        }
    }

    public bool TryGetRandomPosition(out Vector3 hitPosition, out Vector3 hitNormal)
    {
        Vector3 randomOrigin = new Vector3(
            Random.Range(_mapBounds.min.x, _mapBounds.max.x),
            _mapBounds.max.y,
            Random.Range(_mapBounds.min.z, _mapBounds.max.z)
        );

        if (Physics.Raycast(randomOrigin, Vector3.down, out RaycastHit hit, _mapBounds.size.y, groundMask))
        {
            hitPosition = hit.point;
            hitNormal = hit.normal;
            return true;
        }

        hitPosition = Vector3.zero;
        hitNormal = Vector3.up;
        return false;
    }

    public bool CheckCollision(Vector3 position, GameObject prefab)
    {
        if (prefab.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
        {
            Vector3 extents = boxCollider.size / 2f;
            extents = Vector3.Scale(extents, prefab.transform.localScale);
            return Physics.CheckBox(position, extents, Quaternion.identity, obstacleMask);
        } 
        else if (prefab.TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
        {
            float sphereOffset = (capsuleCollider.height / 2f) - capsuleCollider.radius;

            Vector3 point1 = position + Vector3.up * sphereOffset;
            Vector3 point2 = position + Vector3.down * sphereOffset;

            return Physics.CheckCapsule(point1, point2, capsuleCollider.radius, obstacleMask);
        }
        return false;
    }

    public bool TryGetValidRandomPosition(GameObject prefab, out Vector3 hitPosition, out Vector3 hitNormal)
    {
        int attempts = 0;
        bool isBlocked = false;

        do
        {
            TryGetRandomPosition(out hitPosition, out hitNormal);
            isBlocked = CheckCollision(hitPosition, prefab);
        } while (isBlocked && attempts < 15);

        return !isBlocked;
    }

    private void OnDrawGizmosSelected()
    {
        LevelMapGenerator mapGenerator = GetComponent<LevelMapGenerator>();
        Gizmos.color = Color.yellow;

        float width = mapGenerator.mapSize * mapGenerator.horizontalSpacing;
        float height = (mapGenerator.GetMaxElevation() + 1) * mapGenerator.verticalSpacing;
        float posXZ = (width - mapGenerator.horizontalSpacing) / 2f;

        Gizmos.DrawWireCube(new Vector3(posXZ, height/2f, posXZ), new Vector3(width, height, width));
    }
}
