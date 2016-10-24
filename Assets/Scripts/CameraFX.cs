using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFX : MonoBehaviour
{

	public Transform nyanCat;
	public float distanceToNearestEnemy;

    public float ambOccPulseMin = 0.7f;
    public float ambOccPulseMax = 1f;
    public float ambOccPulseSpeed = 1.4f;
    
    public float vignetteMinDist = 100f;
    public float vignetteMult = 75f;
    public float vignetteLerpMin = 0.2f;
    public float vignetteLerpMax = 0.4f;


    public float grainMinDist = 40f;
    public float grainMult = 30f;
    public float grainLerpMin = 0.3f;
    public float grainLerpMax = 0.7f;

    private void LateUpdate()
	{
		//GetComponent<ScreenSpaceAmbientOcclusion>().m_Radius = Mathf.Lerp(ambOccPulseMin, ambOccPulseMax, Mathf.InverseLerp(-1f, 1f, Mathf.Sin(Time.time * ambOccPulseSpeed)));


		if(nyanCat.gameObject.activeInHierarchy)
		{

			float distSqr = (nyanCat.position - transform.position).sqrMagnitude;

			GetComponentInChildren<VignetteAndChromaticAberration>().intensity = Mathf.Lerp(vignetteLerpMin, vignetteLerpMax, (vignetteMinDist - distSqr) / vignetteMult);

			//GetComponentInChildren<NoiseAndScratches>().grainIntensityMax = Mathf.Lerp(grainLerpMin, grainLerpMax, (grainMinDist - distSqr) / grainMult);
			//GetComponentInChildren<NoiseAndScratches>().grainIntensityMin = GetComponentInChildren<NoiseAndScratches>().grainIntensityMax / 2f;
		}
		else
		{
			GetComponentInChildren<VignetteAndChromaticAberration>().intensity = vignetteLerpMin;

			//GetComponentInChildren<NoiseAndScratches>().grainIntensityMax = grainLerpMin;
			//GetComponentInChildren<NoiseAndScratches>().grainIntensityMin = GetComponentInChildren<NoiseAndScratches>().grainIntensityMax / 2f;
		}
	}

}
