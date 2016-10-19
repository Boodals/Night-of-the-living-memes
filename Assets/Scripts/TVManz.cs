﻿using UnityEngine;
using System.Collections;

public class TVManz : EnemyBase
{

    public float m_passiveListenRadius;
    public float m_alertListenRadius;
    public float m_chaseSpeed;

    public Vector3 m_viewBoxPassive;
    public Vector3 m_viewBoxAlert;
    public Vector3 m_viewBoxChase;

    public Vector3 m_anchor;
    public Vector3 m_target;
    public float m_wanderRadius;
    
    private PlayerScript m_player;
    private NavMeshAgent m_navAgent;
    private BoxCollider m_viewbox;

    private float m_defaultSpeed;
    private float m_listenRadius;
    private bool m_playerInViewbox;
    //state stuff
    protected StateContoller m_SC;
    private const short PASSIVE = 1;
    private const short ALERT = 2;
    private const short CHASING = 4;

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
        m_SC.update();

    }
    
    [Update(PASSIVE)]
    private void passiveUpdate()
    {
        if ((m_target - transform.position).magnitude <= 0.1f)
        {
            m_target = m_anchor + (Random.insideUnitSphere * m_wanderRadius);
            m_target.y = transform.position.y;

            m_navAgent.SetDestination(m_target);
        }

        if ((m_player.gameObject.transform.position - transform.position).magnitude <= m_player.GetCurrentNoiseLevel() + m_listenRadius)
        {
            m_navAgent.SetDestination(m_player.gameObject.transform.position);//shoddy job for now
            m_SC.transition(ALERT);
        }
        else if (m_playerInViewbox)
        {
            m_SC.transition(ALERT);
        }
    }
    
    [Update(ALERT)]
    private void alertUpdate()
    {
        //look around
        if (m_playerInViewbox)
        {
            //raycast to search for player (have on a timer if it's laggy)
            RaycastHit hit;
            if (Physics.Linecast(transform.position, m_player.transform.position, out hit))
            {
                if (hit.collider.tag == Tags.Player)
                {
                    Debug.Log("LOS drawn");
                    m_SC.transition(CHASING);
                }
            }
        }
    }

    [Update(CHASING)]
    private void chasingUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
        if (!m_playerInViewbox)
        {
            m_SC.transition(ALERT);
        }
        //should also check raycast ...

    }

    

    [Transition(StateContoller.ANY_STATE, ALERT)]
    private void anyToAlert()
    {
        Debug.Log("Transitioning to alert");
        m_viewbox.size = m_viewBoxAlert;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
    }

    [Transition(StateContoller.ANY_STATE, PASSIVE)]
    private void anyToPassive()
    {
        Debug.Log("Transitioning to passive");
        m_viewbox.size = m_viewBoxPassive;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
    }

    [Transition(StateContoller.ANY_STATE, CHASING)]
    private void anyToChasing()
    {
        Debug.Log("Chasing player");
        m_viewbox.size = m_viewBoxChase;
        m_navAgent.speed = m_chaseSpeed;
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            Debug.Log("I know you're there!");
            m_playerInViewbox = true;
        }
    }

    void OnTriggerEXit(Collider _other)
    {
        if (_other.tag == Tags.Player)
        {
            Debug.Log("Where did you go?!");
            m_playerInViewbox = false;
        }
    }
}
