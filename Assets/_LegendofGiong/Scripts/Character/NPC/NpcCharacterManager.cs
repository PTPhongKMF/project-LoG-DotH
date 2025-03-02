using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcCharacterManager : CharacterMovementManager {
    public NpcCombatManager npcCombatManager;
    public NpcLocomotionManager npcLocomotionManager;

    public NavMeshAgent navMeshAgent;

    [SerializeField] private Vector3 defaultSpawnPosition = Vector3.zero;

    [SerializeField] private NpcAIState currentState;

    public NpcIdleState idleState;
    public NpcPursueTargetState pursueTargetState;
    public NpcCombatStanceState combatStanceState;
    public NpcAttackState attackState;

    protected override void Awake() {
        base.Awake();

        npcCombatManager = GetComponent<NpcCombatManager>();
        npcLocomotionManager = GetComponent<NpcLocomotionManager>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        idleState = Instantiate(idleState);
        pursueTargetState = Instantiate(pursueTargetState);

        currentState = idleState;
    }

    protected override void Update() {
        base.Update();

        npcCombatManager.HandleRecoveryTimer(this);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        ProcessStateMachine();
    }

    private void ProcessStateMachine() {
        NpcAIState nextState = currentState?.Tick(this);

        if (nextState != null) {
            currentState = nextState;
        }

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (npcCombatManager.currentTarget != null) {
            npcCombatManager.targetsDirection = npcCombatManager.currentTarget.transform.position - transform.position;
            npcCombatManager.viewableAngle = WorldLayerUtilityManager.Instance.GetAngleOfTarget(transform, npcCombatManager.targetsDirection);
            npcCombatManager.distanceFromTarget = Vector3.Distance(transform.position, npcCombatManager.currentTarget.transform.position);
        }

        if (navMeshAgent.enabled) {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance) {
                IsMoving = true;
            } else {
                IsMoving = false;
            }
        } else {
            IsMoving = false;
        }
    }

    public void Spawn(Vector3? position = null) {
        Vector3 spawnPosition = position ?? defaultSpawnPosition;
        transform.position = spawnPosition;
    }

    public void Despawn() {
        Destroy(gameObject);
    }
}
