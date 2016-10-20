using UnityEngine;
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

    public Vector3 m_curViewbox;
    public Vector3 m_viewBoxPassive;
    public Vector3 m_viewBoxAlert;
    public Vector3 m_viewBoxChase;

    public float m_wanderRadius;
    protected Vector3 m_anchor;
    protected Vector3 m_target;
    protected Quaternion m_susStartRot;

    protected PlayerScript m_player;
    protected NavMeshAgent m_navAgent;
    protected BoxCollider m_viewbox;

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
    protected const short INCAPPED = 16;//not implemented yet!

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

        //if (player.torchToggle && torchOn)
        //m_viewbox.size = m_curViewbox * m_torchOnViewboxMultiplier;
        //else if (torchtoggle && torchOff)
        //m_viewbox.size = m_curViewbox;
    }

    public bool canSeePlayer()
    {
        //can do a timer here if necessary
        bool r = !Physics.Linecast(transform.position, m_player.GetCamera().transform.position);
        return r;
    }

    public bool canHearPlayer()
    {
        bool r = m_player.GetCurrentNoiseLevel() >= 0 && (m_player.gameObject.transform.position - transform.position).magnitude <= m_player.GetCurrentNoiseLevel() + m_listenRadius;
        if (r) Debug.Log("can hear player");
        return r;
    }

    protected void setNewDestination()
    {
        m_target = m_anchor + (Random.insideUnitSphere * m_wanderRadius);
        NavMeshHit hit;
        NavMesh.SamplePosition(m_target, out hit, m_wanderRadius, int.MaxValue);

        m_target = hit.position;
        m_navAgent.SetDestination(m_target);
    }

    protected void lookAround()
    {
        //float angle = 0;
        ////if in second half of rot...
        //if (m_timer >= m_susiciousTime / 2)
        //{
        //    angle = Mathf.Sin(m_timer / (m_susiciousTime / 2)) * m_maxSusSearchAngle;
        //}
        //else
        //{
        //    angle = Mathf.Sin((m_timer  - m_susiciousTime /2)/ (m_susiciousTime / 2)) * -m_maxSusSearchAngle;
        //}
        //transform.rotation = m_susStartRot;
        //transform.Rotate(new Vector3(0, angle, 0));
    }

    [Update(PASSIVE)]
    protected void passiveUpdate()
    {
        if ((m_target - transform.position).magnitude <= 0.5f/*radius of enemy*/)
        {
            setNewDestination();
        }
        if (m_playerInViewbox || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
    }

    [Update(ALERT)]
    protected void alertUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
        if (m_playerInViewbox)
        {
            if (canSeePlayer())
            {
                m_SC.transition(CHASING);
            }
        }
        else if (m_timer >= m_alertTime)
        {
            //m_SC.transition(PASSIVE);
            m_SC.transition(SUSPICIOUS);
        }
        m_timer += Time.deltaTime;
    }

    [Update(SUSPICIOUS)]
    protected void susUpdate()
    {
        //rotate around, from side to side
        lookAround();

        if (m_playerInViewbox || canHearPlayer())
        {
            m_SC.transition(ALERT);
        }
        else if (m_timer >= m_susiciousTime)
        {
            m_SC.transition(PASSIVE);
        }

        m_timer += Time.deltaTime;
    }

    [Update(CHASING)]
    protected void chasingUpdate()
    {
        m_navAgent.SetDestination(m_player.gameObject.transform.position);
        if (!m_playerInViewbox && !canSeePlayer())
        {
            m_SC.transition(ALERT);
        }
    }

    [Update(INCAPPED)]
    protected void incappedUpdate()
    {
        //do nothing!
        //when timer is up, go back to passive
        if (m_timer >= m_incappedTime)
        {
            m_SC.transition(PASSIVE);
        }
        else m_timer += Time.deltaTime;
    }

    [Transition(StateContoller.ANY_STATE, INCAPPED)]
    protected void anyToIncapped()
    {
        Debug.Log("trans to incapped");
        m_timer = 0.0f;
        m_navAgent.SetDestination(transform.position);
    }

    [Transition(StateContoller.ANY_STATE, SUSPICIOUS)]
    protected void anyTosus()
    {
        Debug.Log("trans to sus");
        m_curViewbox = m_viewBoxAlert;
        m_viewbox.size = m_curViewbox;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;

        //stand still
        m_susStartRot = transform.rotation;
        m_navAgent.SetDestination(transform.position);
    }

    [Transition(StateContoller.ANY_STATE, ALERT)]
    protected void anyToAlert()
    {
        Debug.Log("trans to alert");
        m_curViewbox = m_viewBoxAlert;
        m_viewbox.size = m_curViewbox;
        m_listenRadius = m_alertListenRadius;
        m_navAgent.speed = m_defaultSpeed;
        m_timer = 0.0f;
   
    }

    [Transition(StateContoller.ANY_STATE, PASSIVE)]
    protected void anyToPassive()
    {
        Debug.Log("trans to passive");
        setNewDestination();

        m_curViewbox = m_viewBoxPassive;
        m_viewbox.size = m_curViewbox;
        m_listenRadius = m_passiveListenRadius;
        m_navAgent.speed = m_defaultSpeed;
    }

    [Transition(StateContoller.ANY_STATE, CHASING)]
    protected void anyToChasing()
    {
        Debug.Log("trans to chasing");
        m_curViewbox = m_viewBoxChase;
        m_viewbox.size = m_curViewbox;
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
