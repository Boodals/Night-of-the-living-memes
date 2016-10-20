using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public static GunScript gunSingleton;

    public static int ammo = 3;
    bool canFire = true;

    Animator anim;
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;

    bool waitingToReleaseRT = false;

    // Use this for initialization
    void Awake()
    {
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
