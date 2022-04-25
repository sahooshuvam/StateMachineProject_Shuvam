using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameStateController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    public Transform player;
    State currentState;
    public float time = 0f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if(player==null)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
        }
        currentState = new Idle(this.gameObject, agent, animator, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
public class State
{
    public enum STATE
    {
        ATTACK, CHASE, IDLE, DEATH
    }
    public enum EVENTS
    {
        ENTER, UPDATE, EXIT
    }
    public STATE stateName;
    public EVENTS eventStage;

    public GameObject nPC;
    public NavMeshAgent agent;
    public Animator animator;
    public Transform playerPosition;
    public State nextState;
    public State(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _playerPosition)
    {
        this.nPC = _npc;
        this.playerPosition = _playerPosition;
        this.agent = _agent;
        this.animator = _animator;
        eventStage = EVENTS.ENTER;
    }
    public virtual void Enter()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void Update()
    {
        eventStage = EVENTS.UPDATE;
    }
    public virtual void Exit()
    {
        eventStage = EVENTS.EXIT;
    }
    public State Process()
    {
        if (eventStage == EVENTS.ENTER)
        {
            Enter();
        }
        if (eventStage == EVENTS.UPDATE)
        {
            Update();
        }
        if (eventStage == EVENTS.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }
    public bool CanSeePlayer()
    {
        if (Vector3.Distance(nPC.transform.position, playerPosition.position) < 15f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _playerPosition) : base(_npc, _agent, _animator, _playerPosition)
    {
        stateName = STATE.IDLE;
    }
    public override void Enter()
    {
        animator.SetTrigger("isIdle");
        base.Enter();

    }
    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Chase(nPC, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isIdle");
        base.Exit();
    }


}
public class Chase : State
{
    public Chase(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _playerPosition) : base(_npc, _agent, _animator, _playerPosition)
    {
        stateName = STATE.CHASE;
        agent.stoppingDistance = 5f;

    }
    public override void Enter()
    {
        animator.SetTrigger("isWalking");
        base.Enter();

    }
    public override void Update()
    {
        agent.SetDestination(playerPosition.position);


        if (!CanSeePlayer())
        {
            nextState = new Idle(nPC, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
        if (Vector3.Distance(nPC.transform.position, playerPosition.position) <= agent.stoppingDistance)
        {
            nextState = new Attack(nPC, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isWalking");
        base.Exit();
    }


}
public class Attack : State
{
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _playerPosition) : base(_npc, _agent, _animator, _playerPosition)
    {
        stateName = STATE.ATTACK;
    }
    public override void Enter()
    {
        animator.SetTrigger("isShooting");
        base.Enter();

    }
    public override void Update()
    {
        if (Vector3.Distance(nPC.transform.position, playerPosition.position) > agent.stoppingDistance + 1f)
        {
            nextState = new Idle(nPC, agent, animator, playerPosition);
            eventStage = EVENTS.EXIT;
        }
    }
    public override void Exit()
    {
        animator.ResetTrigger("isShooting");
        base.Exit();
    }


}
public class Death : State
{
    public Death(GameObject _npc, NavMeshAgent _agent, Animator _animator, Transform _playerPosition) : base(_npc, _agent, _animator, _playerPosition)
    {
        stateName = STATE.DEATH;
        agent.enabled = false;

    }
    public override void Enter()
    {
        animator.SetTrigger("isSleeping");
        base.Enter();

    }
    public override void Update()
    {


    }
    public override void Exit()
    {
        animator.ResetTrigger("isSleeping");
        base.Exit();
    }


}