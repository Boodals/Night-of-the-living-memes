using UnityEngine;
using System.Collections;

public class PatrolManz : TVManz
{

    public Transform[] m_patrolPoints;
    public bool m_circular;
    [SerializeField]
    private int m_curTarget;
    [SerializeField]
    private int m_dir;

    private const short PATROL = 1;

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();

        //nav agent
        //
        Debug.Assert(m_patrolPoints.Length > 1, "Need at least two patrol points!");
        m_navAgent = GetComponent<NavMeshAgent>();
        //for each point, align with navmesh...
        foreach (Transform t in m_patrolPoints)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(t.position, out hit, float.MaxValue, int.MaxValue))
            {
                t.position = hit.position;
            }
            else
            {
                Debug.Log("Cannot align patrol point with navmesh");
            }
        }
        m_dir = -1;
        m_curTarget = 0;
        transform.position = m_patrolPoints[0].position;
        m_defaultSpeed = m_navAgent.speed;
        
        //viewbox
        //
        m_viewbox = GetComponent<BoxCollider>();

        //state controller
        //
        m_SC = new StateContoller(this);
        m_SC.transition(PATROL);

        //start patrol
        //
        setNextPatrolTarget();
    }

    public void setNextPatrolTarget()
    {
        if (m_circular)
        {
            m_curTarget = ++m_curTarget < m_patrolPoints.Length ? m_curTarget : 0;
        }
        else
        {
            if (m_curTarget <= 0 || m_curTarget >= m_patrolPoints.Length - 1)
            {
                m_dir *= -1;
            }
            m_curTarget += m_dir;
        }
        m_target = m_patrolPoints[m_curTarget].position;
        m_navAgent.SetDestination(m_target);
    }

    [Update(PATROL)]
    protected void patrolUpdate()
    {
        if ((m_target - transform.position).magnitude <= 2.0f/*radius of enemy*/)
        {
            //if not circular and at either end..
            if (!m_circular && (m_curTarget <= 0 || m_curTarget >= m_patrolPoints.Length - 1))
            {
                m_SC.transition(SUSPICIOUS);
            }
            else
            {
                setNextPatrolTarget();
            }
        }

        if (canSeePlayer() || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
    }


    [Transition(SUSPICIOUS, PATROL)]
    private void susToPatrol()
    {
        //Debug.Log("sus to patrol");
        setNextPatrolTarget();
        anyToPatrol();
    }

    [TransitionOverride(StateContoller.ANY_STATE, PATROL)]
    private void anyToPatrol()
    {
        m_viewbox.size = m_viewBoxPassive;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;
        myAnim.SetBool("Frozen", false);
    }
}

//old, working
//public Transform m_patrolPoint;
//private bool m_targetingAnchor;
//private const short PATROL = 1; //overwrite passive
//private bool m_prevTargetingAnchor;


//// Use this for initialization
//void Start()
//{
//    m_player = GameObject.Find("Player").GetComponent<PlayerScript>();
//    m_prevTargetingAnchor = true;

//    //nav agent
//    //
//    m_navAgent = GetComponent<NavMeshAgent>();
//    m_anchor = transform.position;
//    m_target = m_anchor;
//    NavMeshHit hit;
//    if (NavMesh.SamplePosition(m_patrolPoint.position, out hit, float.MaxValue, int.MaxValue)) m_patrolPoint.position = hit.position;
//    m_targetingAnchor = true;
//    m_defaultSpeed = m_navAgent.speed;

//    //viewbox
//    //
//    m_viewbox = GetComponent<BoxCollider>();

//    //state controller
//    //
//    m_SC = new StateContoller(this);
//    m_SC.transition(PATROL);
//    changePatrolTarget();
//}

//public void changePatrolTarget()
//{
//    if (m_targetingAnchor)
//    {
//        m_target = m_patrolPoint.position;
//        m_targetingAnchor = false;
//    }
//    else
//    {
//        m_target = m_anchor;
//        m_targetingAnchor = true;
//    }

//    m_navAgent.SetDestination(m_target);
//}

//[Update(PATROL)]
//protected void patrolUpdate()
//{
//    if ((m_target - transform.position).magnitude <= 2.0f/*radius of enemy*/)
//    {
//        m_SC.transition(SUSPICIOUS);
//    }

//    if (canSeePlayer() || canHearPlayer())
//    {
//        m_SC.transition(ALERT);
//    }
//}


//[Transition(SUSPICIOUS, PATROL)]
//private void susToPatrol()
//{

//    changePatrolTarget();
//    anyToPatrol();
//}

//[TransitionOverride(StateContoller.ANY_STATE, PATROL)]
//private void anyToPatrol()
//{
//    m_viewbox.size = m_viewBoxPassive;
//    m_listenRadius = m_passiveListenRadius;
//    m_navAgent.speed = m_defaultSpeed;
//    m_timer = 0.0f;
//    myAnim.SetBool("Frozen", false);
//}