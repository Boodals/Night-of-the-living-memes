using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NyanCat : MonoBehaviour
{

	public Transform targetTrans;
	public PhoneHUDScript phone;

	public float m_baseSpeed = 0.7f;
    public float m_maxSpeed = 3.0f;
    public float m_accIncreasePerLevel = 0.0021f; //roughly 25 seconds faster to get player speed
    private float m_acc = 0.01278f; //(playerspeed - m_baseSpeed) / timeUntillNyanspeedIsPlayerSpeed (180)
    private float navigationUpdateInterval = 0.2f;
    
	public AudioClip spawnOneShot;
	public AudioClip killedPlayerOneShot;
	public AudioSource oneShotSource;

	private float m_timer;

    public Material glowMat;
    public Color[] glowColours;

    private NavMeshAgent m_navAgent;
    private bool m_attacking;

    //for debugging...
    [SerializeField]
    private float m_curSpeed;

	private void Awake()
	{
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.speed = m_baseSpeed;
        m_acc = m_accIncreasePerLevel * (GameManager.currentStage + 1);

        m_attacking = false;

        phone.distance = Mathf.Infinity;
        targetTrans = GameObject.Find("Player").transform;
    }

	private void Start()
	{
        m_navAgent.speed = m_baseSpeed;
        if (spawnOneShot != null)
		{
			oneShotSource.PlayOneShot(spawnOneShot);
		}

        HUDScript.HUDsingleton.DisplayNyanNotif();

        m_timer = 0.0f;
	}

	private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > navigationUpdateInterval && !m_attacking)
		{
            m_navAgent.destination = targetTrans.position;
            m_timer -= navigationUpdateInterval;
		}

        //accelerate
        if (m_navAgent.speed >= m_maxSpeed)
        {
            m_navAgent.speed = m_maxSpeed;
        }
        else
        {
            m_navAgent.speed += m_acc * Time.deltaTime;
        }
        

        glowMat.SetColor("_EmissionColor", Color.Lerp(glowColours[0], glowColours[1], Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 8))));
		phone.distance = Vector3.Distance(targetTrans.position, transform.position);

        m_curSpeed = m_navAgent.speed;
    }

	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerScript ps = other.GetComponent<PlayerScript>();

			if(ps != null)
			{
				ps.LookAtThis(transform.Find("LookAtPoint"));
                ps.StartDying();

                //Stop the navigation updates
                m_attacking = true;

				m_navAgent.Stop();

				if (killedPlayerOneShot != null)
				{
					oneShotSource.PlayOneShot(killedPlayerOneShot);
				}
			}
		}
	}
}
