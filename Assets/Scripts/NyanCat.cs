using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NyanCat : MonoBehaviour
{

	public Transform targetTrans;

	public PhoneHUDScript phone;

	public float speedPerLevel = 0.5f;

	public float navigationUpdateInterval = 0.2f;


	public AudioClip spawnOneShot;
	public AudioClip killedPlayerOneShot;

	public AudioSource oneShotSource;


	private float navigationUpdateTimer;

    public Material glowMat;
    public Color[] glowColours;

	private void Awake()
	{
		phone.distance = Mathf.Infinity;
    }

	private void Start()
	{
		GetComponent<NavMeshAgent>().speed += speedPerLevel * GameManager.currentStage;

		if (spawnOneShot != null)
		{
			oneShotSource.PlayOneShot(spawnOneShot);
		}

		navigationUpdateTimer = Time.time;
	}

	private void Update()
	{

		if (Time.time - navigationUpdateTimer > navigationUpdateInterval)
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();

			agent.destination = targetTrans.position;

			navigationUpdateTimer = Time.time;
		}

        glowMat.SetColor("_EmissionColor", Color.Lerp(glowColours[0], glowColours[1], Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 8))));
		phone.distance = Vector3.Distance(targetTrans.position, transform.position);
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
				navigationUpdateTimer = Mathf.NegativeInfinity;

				GetComponent<NavMeshAgent>().Stop();

				if (killedPlayerOneShot != null)
				{
					oneShotSource.PlayOneShot(killedPlayerOneShot);
				}
			}
		}
	}
}
