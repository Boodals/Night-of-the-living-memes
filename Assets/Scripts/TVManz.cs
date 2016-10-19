﻿using UnityEngine;
using System.Collections;

public class TVManz : EnemyBase
{
    public float m_passiveListenRadius;
    public float m_alertListenRadius;
    public float m_chaseSpeed;
    public float m_alertTime;

    public Vector3 m_viewBoxPassive;
    public Vector3 m_viewBoxAlert;
    public Vector3 m_viewBoxChase;

    public Vector3 m_anchor;
    public Vector3 m_target;
    public float m_wanderRadius;

    protected PlayerScript m_player;
    protected NavMeshAgent m_navAgent;
    protected BoxCollider m_viewbox;

    protected float m_defaultSpeed;
    protected float m_listenRadius;
    protected bool m_playerInViewbox;
    protected float m_alertTimer;
    //state stuff
    protected StateContoller m_SC;
    protected const short PASSIVE = 1;
    protected const short ALERT = 2;
    protected const short CHASING = 4;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.Find("Player").GetComponent<PlayerScript>();

        //nav agent
        //
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anchor = transform.position;
        m_target = m_anchor;
        m_defaultSpeed = m_navAgent.speed;

        //viewbox
        //
        m_viewbox = GetComponent<BoxCollider>();
        
        //state controller
        //
        m_SC = new StateContoller(this);
        m_SC.transition(PASSIVE);
    }

    // Update is called once per frame
    void Update()
    {
        //if (canHearPlayer() && m_SC.currentState() != CHASING)
        //{
        //    m_navAgent.SetDestination(m_player.gameObject.transform.position);
        //    m_SC.transition(ALERT);
        //    //Debug.Log("Heard the player!");
        //}
        m_SC.update();
    }

    public bool canSeePlayer()
    {
        //can do a timer here if necessary
        bool r = !Physics.Linecast(transform.position, m_player.GetCamera().transform.position);
        return r;
    }

    public bool canHearPlayer()
    {
        return m_player.GetCurrentNoiseLevel() >= 0 && (m_player.gameObject.transform.position - transform.position).magnitude <= m_player.GetCurrentNoiseLevel() + m_listenRadius;
    }

    public void setNewDestination()
    {
        m_target = m_anchor + (Random.insideUnitSphere * m_wanderRadius);
        NavMeshHit hit;
        NavMesh.SamplePosition(m_target, out hit, m_wanderRadius, int.MaxValue);

        m_target = hit.position;
        m_navAgent.SetDestination(m_target);
    }

    [Update(PASSIVE)]
    protected void passiveUpdate()
    {
      
        if ((m_target - transform.position).magnitude <= 0.5f/*radius of enemy*/)
        {
            setNewDestination();
        }
        if (canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
        if (m_playerInViewbox)
        {
            m_SC.transition(ALERT);
        }
    }
    
    [Update(ALERT)]
    protected void alertUpdate()
    {
        if (m_playerInViewbox)
        {
            if (canSeePlayer())
            {
                m_SC.transition(CHASING);
            }
        }
        else if (m_alertTimer >= m_alertTime)
        {
            m_SC.transition(PASSIVE);
        }
        m_alertTimer += Time.deltaTime;
    }
    
    [Update(CHASING)]
    protected void chasingUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
        if (!m_playerInViewbox || !canSeePlayer())
        {
            m_SC.transition(ALERT);
        }
    }

    [Transition(StateContoller.ANY_STATE, ALERT)]
    protected void anyToAlert()
    {
        m_viewbox.size = m_viewBoxAlert;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_alertTimer = 0.0f;
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
    }

    [Transition(StateContoller.ANY_STATE, PASSIVE)]
    protected void anyToPassive()
    {

        setNewDestination();

        m_viewbox.size = m_viewBoxPassive;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
    }

    [Transition(StateContoller.ANY_STATE, CHASING)]
    protected void anyToChasing()
    {
        m_viewbox.size = m_viewBoxChase;
        m_navAgent.speed = m_chaseSpeed;
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            m_playerInViewbox = true;
        }
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            m_playerInViewbox = false;
        }
    }
}
