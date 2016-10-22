using UnityEngine;
using System.Collections;

public class PatrolManz : TVManz
{
    public Transform m_patrolPoint;
    private bool m_targetingAnchor;
    private const short PATROL = 1; //overwrite passive
  
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();

        //nav agent
        //
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anchor = transform.position;
        m_target =  m_anchor;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(m_patrolPoint.position, out hit, float.MaxValue, int.MaxValue)) m_patrolPoint.position = hit.position;
        m_targetingAnchor = true;
        m_defaultSpeed = m_navAgent.speed;

        //viewbox
        //
        m_viewbox = GetComponent<BoxCollider>();

        //state controller
        //
        m_SC = new StateContoller(this);
        m_SC.transition(PATROL);
    }

    public void changePatrolTarget()
    {
        if (m_targetingAnchor)
        {
            m_target = m_patrolPoint.position;
            m_targetingAnchor = false;
        }
        else
        {
            m_target = m_anchor;
            m_targetingAnchor = true;
        }

        m_navAgent.SetDestination(m_target);
    }

    [Update(PATROL)]
    protected void patrolUpdate()
    {
        if ((m_target - transform.position).magnitude <= 2.0f/*radius of enemy*/)
        {
            changePatrolTarget();
        }

        if (m_playerInViewbox || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
    }

    [Transition(StateContoller.ANY_STATE, PATROL)]
    private void anyToPatrol()
    {
        changePatrolTarget();
        m_viewbox.size = m_viewBoxPassive;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;
        myAnim.SetBool("Frozen", false);
    }
}

