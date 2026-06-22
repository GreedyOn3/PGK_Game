using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Animator bossAnimator;
    public GameObject attackPrefab;
    public List<GameObject> attackVisuals;

    [Header("Attack Settings")]
    public float attackRadius = 5f;
    public int attackDamage = 15;
    public float detectionRadius = 15f;
    public float attackCooldown = 3f;
    [Header("Attack Timings")]
    public float windUpTime = 1f;
    public float telegraphDuration = 2f;
    public float recoveryTime = 1.5f;

    [Header("Prediction Settings")]
    [Range(0f, 1f)]
    public float predictionWeight = 0.8f;
    public LayerMask playerLayer;

    private bool _isAttacking = false;
    private bool _isSpawning = true;
    private float _lastAttackTime = 0f;

    private PlayerReferences _player;
    private PlayerMovement _playerMovement;
    private PlayerController _playerController;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
        _playerMovement = _player.Movement;
        _playerController = _player.Controller;
    }

    private void Update()
    {
        if (!_isSpawning && !_isAttacking && _player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            if (distanceToPlayer <= detectionRadius && Time.time >= _lastAttackTime + attackCooldown)
                StartCoroutine(AttackSequenceRoutine());
        }
    }

    private IEnumerator AttackSequenceRoutine()
    {
        _isAttacking = true;

        // PHASE 1: START
        if (bossAnimator) bossAnimator.SetTrigger("AttackStart");
        yield return new WaitForSeconds(windUpTime);

        // PHASE 2: HOLD
        Vector3 targetPos = GetPredictedPlayerPosition();

        if (attackPrefab != null)
        {
            BossAttack attack = Instantiate(attackPrefab, targetPos, Quaternion.identity).GetComponent<BossAttack>();
            if (attack != null)
                attack.Initialize(attackDamage, telegraphDuration, attackRadius, attackVisuals);
        }
        yield return new WaitForSeconds(telegraphDuration);

        // PHASE 3: END
        if (bossAnimator) bossAnimator.SetTrigger("AttackExecute");
        yield return new WaitForSeconds(recoveryTime);

        _lastAttackTime = Time.time;
        _isAttacking = false;
    }

    private Vector3 GetPredictedPlayerPosition()
    {
        if (_playerMovement == null) 
            return transform.position + transform.forward * 5f;

        Vector3 currentPos = _playerMovement.transform.position;
        Vector3 currentHorizontalVel = _playerMovement.GetHorizontalVelocity();

        Vector3 predictedPos = currentPos + (currentHorizontalVel * telegraphDuration);
        Vector3 finalPos = Vector3.Lerp(currentPos, predictedPos, predictionWeight);

        if (Physics.Raycast(finalPos + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 50f, _playerMovement.groundMask))
            finalPos = hit.point;
        else if (_playerController != null && _playerController.IsGrounded())
            finalPos.y = currentPos.y;

        return finalPos;
    }

    public void SetSpawning(bool val) => _isSpawning = val;
    public bool IsAttacking() => _isAttacking;
    public bool IsSpawning() => _isSpawning;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
