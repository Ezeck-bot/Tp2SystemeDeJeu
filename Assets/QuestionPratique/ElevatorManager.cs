using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Transformer ce script en Singleton. Il ne doit pas subsister après un chargement de scène. (2 pts)

// Ajouter une state machine. Elle peut être dans la même classe ou dans une classe à part. (8 pts)
// Voici ce qu’il doit être possible de faire avec les states de l’ascenseur :
// - Idle : l’ascenseur est à l’arrêt, il attend une commande pour se déplacer
// - MovingUp : l’ascenseur est en mouvement vers un étage supérieur
// - MovingDown : l’ascenseur est en mouvement vers un étage inférieur
// - Doors : l’ascenseur attend 2 secondes dans ce state pour simuler l’ouverture des portes

// Consignes : (10 pts)
// - L’ascenseur doit commencer dans le state Idle
// - Le state Doors doit être déclenché à l’arrivée à l’étage demandé et doit durer 2 secondes avant de retourner dans le state Idle
// - SEUL le state Idle peut recevoir une commande pour se déplacer ; les autres states doivent ignorer les commandes de déplacement
// - Deux requêtes de déplacement à gérer à la fois : une active et une en attente. La requête en attente doit être traitée dès que l’ascenseur retourne dans le state Idle
// - Passer d’un étage à un autre doit prendre 1 seconde par étage

// Debug.Logs ou print (3 pts)
// - Je veux un Debug pour chaque changement d’étage afin de pouvoir suivre facilement
// - Un Debug.Log au début et à la fin du state Doors pour montrer que les portes se sont ouvertes et fermées
// - Un Debug.Log à la fin du state Idle pour savoir ce qui s’y passe

// Propreté du code (4 pts)
// - Pas de duplication de code
// - Pas de magic numbers / strings
// - Normes de programation
// - Clarté etc etc


public class ElevatorManager : MonoBehaviour
{
    public int m_currentFloor = 1;
    public int[] m_requestedFloor = {0,0};
    public static ElevatorManager Instance { get; private set; }

    private Dictionary<Type, BaseState> m_stateDict = new();
    private BaseState m_currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        m_stateDict[typeof(StateIdle)] = new StateIdle(this);
        m_stateDict[typeof(StateMovingUp)] = new StateMovingUp(this);
        m_stateDict[typeof(StateMovingDown)] = new StateMovingDown(this);
        m_stateDict[typeof(StateDoors)] = new StateDoors(this);
        OnStateChange(typeof(StateIdle));
        

    }

    private void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.Update();
        }
    }

    public void OnStateChange(Type stateType)
    {
        BaseState newState = m_stateDict[stateType];
        if (m_currentState != null)
        {
            m_currentState.OnExit();
        }

        m_currentState = newState;
        m_currentState.OnEnter();
    }

    public abstract class BaseState
    {
        protected readonly ElevatorManager m_elevatorManager;


        public BaseState(ElevatorManager elevatorManager)
        {
            m_elevatorManager = elevatorManager;
        }

        public abstract void OnEnter();
        public abstract void Update();
        public abstract void OnExit();
    }

    public class StateIdle : BaseState
    {
        public StateIdle(ElevatorManager elevatorManager) : base(elevatorManager)
        {

        }

        public override void OnEnter()
        {
            if (ElevatorManager.Instance.m_requestedFloor[1] != 0)
            {
                ElevatorManager.Instance.m_requestedFloor[0] = ElevatorManager.Instance.m_requestedFloor[1];
                ElevatorManager.Instance.m_requestedFloor[1] = 0;
                ElevatorManager.Instance.MoveElevator(ElevatorManager.Instance.m_requestedFloor[0]);
            }
        }
        public override void Update()
        {

        }
        public override void OnExit()
        {
            Debug.Log("Elevator is moving to the " + ElevatorManager.Instance.m_requestedFloor[0] + " floor");
        }
    }
    public class StateMovingUp : BaseState
    {
        public StateMovingUp(ElevatorManager elevatorManager) : base(elevatorManager)
        {

        }

        public override void OnEnter()
        {
            ElevatorManager.Instance.StartMoveUp();
        }
        public override void Update()
        {
            
        }
        public override void OnExit()
        {
            m_elevatorManager.m_requestedFloor[0] = 0;
        }

    }
    public class StateMovingDown : BaseState
    {
        public StateMovingDown(ElevatorManager elevatorManager) : base(elevatorManager)
        {

        }

        public override void OnEnter()
        {
            ElevatorManager.Instance.StartMoveDown();
        }
        public override void Update()
        {

        }
        public override void OnExit()
        {

        }
    }
    public class StateDoors : BaseState
    {
        public StateDoors(ElevatorManager elevatorManager) : base(elevatorManager)
        {

        }

        public override void OnEnter()
        {
            ElevatorManager.Instance.m_requestedFloor[0] = 0;
            Debug.Log("Doors Oppening");
            ElevatorManager.Instance.StartDoor();
        }
        public override void Update()
        {

        }
        public override void OnExit()
        {
            Debug.Log("Doors Closing");
        }
    }
    public void StartMoveUp()
    {
        StartCoroutine(MoveUp(1));
    }
    public void StartMoveDown()
    {
        StartCoroutine(MoveDown(1));
    }
    public void StartDoor()
    {
        StartCoroutine(MoveDoor(2));
    }
    public IEnumerator MoveUp(float time)
    {
        while(m_currentFloor != m_requestedFloor[0])
        {
            yield return new WaitForSeconds(time);
            m_currentFloor++;
            Debug.Log("Current Floor: " + m_currentFloor);
        }
        OnStateChange(typeof(StateDoors));
        yield return null;
    }
    public IEnumerator MoveDown(float time)
    {
        while (m_currentFloor != m_requestedFloor[0])
        {
            yield return new WaitForSeconds(time);
            m_currentFloor--;
            Debug.Log("Current Floor: " + m_currentFloor);
        }
        OnStateChange(typeof(StateDoors));
        yield return null;
    }
    public IEnumerator MoveDoor(float time)
    {
        yield return new WaitForSeconds(time);
        ElevatorManager.Instance.OnStateChange(typeof(StateIdle));
        yield return null;
    }
    public void MoveElevator(int newFloor)
    {
        if (m_requestedFloor[0] == 0) 
        {
            m_requestedFloor[0] = newFloor;
        }
        else if(m_requestedFloor[1] == 0)
        {
            m_requestedFloor[1] = newFloor;
        }
        else
        {
            return;
        }

        if (m_requestedFloor[0] >  m_currentFloor) 
        {
            OnStateChange(typeof(StateMovingUp));
        }
        else if(m_requestedFloor[0] < m_currentFloor)
        {
            OnStateChange(typeof(StateMovingDown));
        }
        else
        {
            m_requestedFloor[0] = 0;
            return;
        }
    }
}

