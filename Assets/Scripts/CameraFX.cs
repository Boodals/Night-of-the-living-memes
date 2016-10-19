using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraFX : MonoBehaviour
{

	public Transform nyanCat;
	public float distanceToNearestEnemy;

	public float noiseOffset = 100f;
	public float noiseDist = 25f;

	public float vignetteOffset = 20f;
	public float vignetteDist = 15f;
	public float vignetteLerpMin = 0.2f;
	public float vignetteLerpMax = 0.4f;

	public float motBlurOffset = 50f;
	public float motBlurDist = 50f;
	public float motBlurLerpMin = 0f;
	public float motBlurLerpMax = 0.8f;

	private void LateUpdate()
	{
		float distSqr = (nyanCat.position - transform.position).sqrMagnitude;

		GetComponent<NoiseAndGrain>().intensityMultiplier = Mathf.Max((noiseOffset - distSqr) / noiseDist, 0f);

		GetComponent<VignetteAndChromaticAberration>().intensity = Mathf.Lerp(vignetteLerpMin, vignetteLerpMax, (vignetteOffset - distSqr) / vignetteDist);
		
		GetComponent<MotionBlur>().blurAmount = Mathf.Lerp(motBlurLerpMin, motBlurLerpMax, (motBlurOffset - distSqr) / motBlurDist);


		Vector3 euler = transform.rotation.eulerAngles + new Vector3(0f, Time.deltaTime * 30f, 0f);
		transform.rotation = Quaternion.Euler(euler);
	}

}
