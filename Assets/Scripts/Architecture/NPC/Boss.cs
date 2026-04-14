using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] public ExperienceController exp;
    [SerializeField] public HpController hp;
    [SerializeField] public NavMeshAgent agent;
    public GameObject[] patrolPoint;
    [SerializeField] public GameObject player;
    public int currPoint = 0;
    private Dictionary<Type, BaseState> m_stateDict = new();
    private BaseState m_currentState;

    private void Awake()
    {
        m_stateDict[typeof(StateIdle)] = new StateIdle(this);
        m_stateDict[typeof(StatePatrol)] = new StatePatrol(this);
        m_stateDict[typeof(StatePursuit)] = new StatePursuit(this);
        m_stateDict[typeof(StateRetreat)] = new StateRetreat(this);
        m_stateDict[typeof(StateAttack)] = new StateAttack(this);
    }

    private void Update()
    {
        if(hp.m_currentLife <= 0)
        {
            OnStateChange(typeof(StateIdle));
        }
        else if(Vector3.Distance(transform.position, player.transform.position) < 20)
        {
            if(exp.m_currentLevel < 2)
            {
                if(Vector3.Distance(transform.position, player.transform.position) < 5)
                {
                    OnStateChange(typeof (StateAttack));
                }
                OnStateChange(typeof(StatePursuit));
            }
            else
            {
                OnStateChange(typeof(StateRetreat));
            }
        }
        else
        {
            OnStateChange(typeof(StatePatrol));
        }

        if (m_currentState != null)
        {
            m_currentState.Update();
        }
    }

    public void OnStateChange(Type stateType)
    {
        BaseState newState = m_stateDict[stateType];
        if (newState != m_currentState) 
        {
            if (m_currentState != null)
            {
                m_currentState.OnExit();
            }

            m_currentState = newState;
            m_currentState.OnEnter();
        }
        else { return; }

    }

    public abstract class BaseState
    {
        protected readonly Boss m_boss;


        public BaseState(Boss boss)
        {
            m_boss = boss;
        }

        public abstract void OnEnter();
        public abstract void Update();
        public abstract void OnExit();
    }

    public class StateIdle : BaseState
    {
        public StateIdle(Boss boss) : base(boss)
        {

        }

        public override void OnEnter()
        {
            m_boss.agent.ResetPath();
        }
        public override void Update()
        {

        }
        public override void OnExit()
        {
           
        }
    }
    public class StatePatrol : BaseState
    {
        public StatePatrol(Boss boss) : base(boss)
        {

        }

        public override void OnEnter()
        {
            
        }
            
        public override void Update()
        {
            m_boss.agent.SetDestination(m_boss.patrolPoint[m_boss.currPoint].transform.position);
            if (Vector3.Distance(m_boss.patrolPoint[m_boss.currPoint].transform.position, m_boss.transform.position) < 5)
            {
                m_boss.currPoint++;
                if (m_boss.currPoint > 3) 
                {
                    m_boss.currPoint = 0;
                }
            }
        }
        public override void OnExit()
        {
           
        }

    }
    public class StatePursuit : BaseState
    {
        public StatePursuit(Boss boss) : base(boss)
        {

        }

        public override void OnEnter()
        {
           
        }
        public override void Update()
        {
            m_boss.agent.SetDestination(m_boss.player.transform.position);
        }
        public override void OnExit()
        {

        }
    }
    public class StateRetreat : BaseState
    {
        public StateRetreat(Boss boss) : base(boss)
        {

        }

        public override void OnEnter()
        {
            Vector3 directionAway = m_boss.transform.position - m_boss.player.transform.position;
            m_boss.agent.SetDestination(m_boss.transform.position * 20);
        }
        public override void Update()
        {

        }
        public override void OnExit()
        {
            m_boss.agent.ResetPath();
        }
    }
    public class StateAttack : BaseState
    {
        public StateAttack(Boss boss) : base(boss)
        {

        }

        public override void OnEnter()
        {

        }
        public override void Update()
        {
            m_boss.hp.m_currentLife = m_boss.hp.m_currentLife - 10;
            m_boss.OnStateChange(typeof(StateRetreat));
        }
        public override void OnExit()
        {

        }
    }
}
