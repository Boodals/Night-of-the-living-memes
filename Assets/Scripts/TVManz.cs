﻿using UnityEngine;
using System.Collections;

public class TVManz : EnemyBase
{
    public float m_passiveListenRadius;
    public float m_alertListenRadius;
    public float m_chaseSpeed;
    public float m_alertTime;
    public float m_susiciousTime;
    public float m_incappedTime;
    public float m_maxSusSearchAngle;
    public float m_torchOnViewboxMultiplier;
    public float m_wanderRadius;
    public float m_killRange;

    public Vector3 m_curViewbox;
    public Vector3 m_viewBoxPassive;
    public Vector3 m_viewBoxAlert;
    public Vector3 m_viewBoxChase;

    public AudioClip m_alertOneShot;
    public AudioClip m_susOneShot;
    public AudioClip m_chaseOneShot;
    public AudioClip m_incappOneShot;
    public AudioClip m_attackingOneShot;
    public AudioSource m_staticSource;
    public AudioSource m_oneShotSource;

    protected Vector3 m_anchor;
    protected Vector3 m_target;
    protected Quaternion m_susStartRot;

    protected PlayerScript m_player;
    protected NavMeshAgent m_navAgent;
    protected BoxCollider m_viewbox;
    public Transform m_headTransform;

    protected float m_defaultSpeed;
    protected float m_listenRadius;
    protected bool m_playerInViewbox;
    protected float m_timer;

    //state stuff
    protected StateContoller m_SC;
    protected const short PASSIVE = 1;
    protected const short ALERT = 2;
    protected const short CHASING = 4;
    protected const short SUSPICIOUS = 8;
    protected const short INCAPPED = 16;
    protected const short ATTACKING = 32;

    public Animator myAnim;
    public AudioClip[] footsteps;

    // Use this for initialization
    void Start()
    {
        //references
        //
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

        //sounds
        //
        m_staticSource.Play();
    }
 
    // Update is called once per frame
    void Update()
    {
        m_SC.update();
        m_timer += Time.deltaTime;
        if (m_player.flashlightToggledThisFrame)
        {
            if (m_player.flashlightOn)
                m_viewbox.size = m_curViewbox * m_torchOnViewboxMultiplier;
            else
                m_viewbox.size = m_curViewbox;
        }

        myAnim.SetFloat("MovementIntensity", m_navAgent.velocity.magnitude/3);
    }

    public void sleep()
    {
        m_SC.transition(INCAPPED);
    }

    public bool canSeePlayer()
    {
        //can do a timer here if necessary
        bool r = m_playerInViewbox  && !Physics.Linecast(transform.position, m_player.GetCamera().transform.position);
        return r;
    }

    public bool canHearPlayer()
    {
        bool r = m_player.GetCurrentNoiseLevel() > 0 && (m_player.gameObject.transform.position - transform.position).magnitude <= m_player.GetCurrentNoiseLevel() + m_listenRadius;
        if (r) Debug.Log("Heard player!");
        return r;
    }

    protected void setNewDestination()
    {
        m_target = m_anchor + (Random.insideUnitSphere * m_wanderRadius);
        m_target.y = m_anchor.y;
        NavMeshHit hit;
        NavMesh.SamplePosition(m_target, out hit, m_wanderRadius, int.MaxValue);
   
        m_target = hit.position;
        m_navAgent.SetDestination(m_target);
        Debug.DrawLine(transform.position, m_target, Color.red, 20.0f);
    }

    protected void lookAround()
    {
        float angle = m_maxSusSearchAngle;
        if (m_timer < m_susiciousTime / 2)
        {
            angle *= Mathf.Sin(m_timer * Mathf.PI / (m_susiciousTime / 2));
        }
        else
        {
            angle *= -Mathf.Sin((m_timer - (m_susiciousTime/2)) * Mathf.PI / (m_susiciousTime / 2));
        }
        transform.rotation = m_susStartRot;
        transform.Rotate(new Vector3(0, angle, 0));
    }

    [Update(PASSIVE)]
    protected void passiveUpdate()
    { 
        if ((m_target - transform.position).magnitude <= 2.0f/*radius of enemy*/)
        {
            setNewDestination();
        }
        if (canSeePlayer() || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
    }

    [Update(ALERT)]
    protected void alertUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);

        if (canSeePlayer())
        {
            m_SC.transition(CHASING);
        }
        else if (m_timer >= m_alertTime)
        {
            m_SC.transition(SUSPICIOUS);
        }
    }

    [Update(SUSPICIOUS)]
    protected void susUpdate()
    {
        //rotate around, from side to side
        lookAround();

        if (canSeePlayer() || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
        else if (m_timer >= m_susiciousTime)
        {
            m_SC.transition(PASSIVE);
        }
    }

    [Update(CHASING)]
    protected void chasingUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
        if (!canSeePlayer())
        {
            m_SC.transition(ALERT);
        }
        else
        {
            if ((transform.position - m_player.GetCamera().transform.position).magnitude <= m_killRange)
            {

                m_SC.transition(ATTACKING);
            }
        }
    }

    [Update(INCAPPED)]
    protected void incappedUpdate()
    {
        //do nothing!
        //when timer is up, go back to passive
        if (m_timer >= m_incappedTime)
        {
            Debug.Log("trying to trans to passive");
            m_SC.transition(PASSIVE);
        }
    }
    [Update(ATTACKING)]
    protected void attackingUpdate()
    {
        myAnim.SetTrigger("Attack");
        transform.rotation = Quaternion.LookRotation(PlayerScript.playerSingleton.transform.position - transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        m_player.LookAtThis(m_headTransform);
    }

    [Transition(StateContoller.ANY_STATE, ATTACKING)]
    protected void anyToAttack()
    {
        //stand still
        Debug.Log("Die");
      
        m_navAgent.SetDestination(transform.position);
        m_player.StartDying();

        if (m_attackingOneShot != null) m_oneShotSource.PlayOneShot(m_attackingOneShot);
    }

    [Transition(StateContoller.ANY_STATE, INCAPPED)]
    protected void anyToIncapped()
    {
        myAnim.SetBool("Frozen", true);
        m_timer = 0.0f;
        m_navAgent.SetDestination(transform.position);

        if (m_incappOneShot != null) m_oneShotSource.PlayOneShot(m_incappOneShot);
    }

    [Transition(StateContoller.ANY_STATE, SUSPICIOUS)]
    protected void anyTosus()
    {
        m_curViewbox = m_viewBoxAlert;
        m_viewbox.size = m_curViewbox;
        if (m_player.flashlightOn) m_viewbox.size *= m_torchOnViewboxMultiplier;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;

        //stand still
        m_susStartRot = transform.rotation;
        m_navAgent.SetDestination(transform.position);

        if (m_susOneShot != null) m_oneShotSource.PlayOneShot(m_susOneShot);
    }

    [Transition(StateContoller.ANY_STATE, ALERT)]
    protected void anyToAlert()
    {
        m_curViewbox = m_viewBoxAlert;
        m_viewbox.size = m_curViewbox;
        if (m_player.flashlightOn) m_viewbox.size *= m_torchOnViewboxMultiplier;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;

        if (m_alertOneShot != null) m_oneShotSource.PlayOneShot(m_alertOneShot);
    }

    [Transition(StateContoller.ANY_STATE, PASSIVE)]
    protected void anyToPassive()
    {
        setNewDestination();

        m_curViewbox = m_viewBoxPassive;
        m_viewbox.size = m_curViewbox;
        if (m_player.flashlightOn) m_viewbox.size *= m_torchOnViewboxMultiplier;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        myAnim.SetBool("Frozen", false);

    }

    [Transition(StateContoller.ANY_STATE, CHASING)]
    protected void anyToChasing()
    {
        m_curViewbox = m_viewBoxChase;
        m_viewbox.size = m_curViewbox;
        if (m_player.flashlightOn) m_viewbox.size *= m_torchOnViewboxMultiplier;
        m_navAgent.speed = m_chaseSpeed;

        if (m_chaseOneShot != null) m_oneShotSource.PlayOneShot(m_chaseOneShot);
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
