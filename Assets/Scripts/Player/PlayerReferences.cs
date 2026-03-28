using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerXp xp;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerCamera playerCamera;

    public PlayerHealth Health => health;
    public PlayerController Controller => controller;
    public PlayerMovement Movement => movement;
    public PlayerStats Stats => stats;
    public PlayerXp Xp => xp;
    public PlayerInventory Inventory => inventory;
    public PlayerCamera Camera => playerCamera;
}
