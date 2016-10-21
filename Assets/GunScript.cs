using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunScript : MonoBehaviour
{
    public static GunScript gunSingleton;
    public GameObject bulletPrefab;

    int PooledAmount = 20;

    List<GameObject> bulletPool;

    public static int ammo = 3;
    bool canFire = true;

    Animator anim;
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;

    bool waitingToReleaseRT = false;

    // Use this for initialization
    void Awake()
    {
        bulletPool = new List<GameObject>();
        for (int i = 0; i < PooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bulletPrefab);
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
        anim = GetComponent<Animator>();
        gunSingleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxisRaw("RT"));

        if(canFire && GameManager.gameManagerSingleton.IsCurrentGameState(GameManager.GameStates.PLAYING) && !waitingToReleaseRT && ammo>0)
        {
            if(Input.GetAxisRaw("RT")>0.9f)
            {
                Fire();
            }
        }

        if(Input.GetAxisRaw("RT")<0.1f)
        {
            waitingToReleaseRT = false;
        }

        muzzleLight.intensity = Mathf.Lerp(muzzleLight.intensity, 0, 16 * Time.deltaTime);

        float mI = anim.GetFloat("MovementIntensity");

        if(PlayerScript.playerSingleton.myState==PlayerScript.State.Sprinting)
        {
            mI = Mathf.Lerp(mI, 1, 11 * Time.deltaTime);
        }
        else
        {
            mI = Mathf.Lerp(mI, 0, 5 * Time.deltaTime);
        }

        anim.SetFloat("MovementIntensity", mI);
    }

    public void Fire()
    {
        //Debug.Break();
        muzzleFlash.Play();
        canFire = false;
        anim.SetTrigger("Fire");
        muzzleLight.intensity = 8;
        ChangeAmmo(-1);
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if(!bulletPool[i].activeInHierarchy)
            {
                bulletPool[i].transform.position = gameObject.transform.position;
                bulletPool[i].transform.rotation = gameObject.transform.rotation;
                bulletPool[i].SetActive(true);
                break;
            }
        }
        waitingToReleaseRT = true;
    }

    public void StartReloading()
    {

    }

    public void ChangeAmmo(int amount)
    {
        ammo += amount;
        if (HUDScript.HUDsingleton)
        {
            HUDScript.HUDsingleton.UpdateAmmoTotal(ammo);
        }
    }

    public void Reload()
    {
        canFire = true;
    }
}
