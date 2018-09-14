using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEntry : MonoBehaviour {

	[SpaceAttribute(10)]
	[HeaderAttribute("Booleans")]
	public bool animateOnStart = false;
	public bool offset = false;

	[SpaceAttribute(10)]
	[HeaderAttribute("Editor Watch")]
	public bool runnableFromEditor = false;
	public bool runAnimation = false;
	private bool isRunning = false;

	[SpaceAttribute(10)]
	[HeaderAttribute("Timing")]
	public float delay = 0;
	public bool isDelayRelativeToParent = false;
	public float effectTime = 1;

	[SpaceAttribute(10)]
	[HeaderAttribute("Object To Animate")]
	public GameObject Target;

	[SpaceAttribute(10)]
	[HeaderAttribute("Transform")]
	public Transform startTransform;
	public Transform endTransform;
	public AnimationCurve rotationCurve;
	public AnimationCurve positionCurve;
	public AnimationCurve scaleCurve;

	private Vector3 endScale;
	private Quaternion endRotation;
	private Vector3 endPosition;

	private Vector3 startScale;
	private Quaternion startRotation;
	private Vector3 startPosition;

	void Awake () {
		SetupVariables();
	}
	// Use this for initialization
	void Start () {
		// SetupVariables();
		if(animateOnStart)
			StartCoroutine(Animation());
		if (runnableFromEditor)
			StartCoroutine(Watch());
	}

	public void startAnimation(){
		if(!isRunning){
			SetupVariables();
			StartCoroutine(Animation());
		}
		else
			Debug.Log("Can't start Animation, it is already running");
	}
	

	void SetupVariables() {
		startPosition = startTransform.localPosition;
		startRotation = startTransform.localRotation;
		startScale = startTransform.localScale;
		endScale = endTransform.localScale;
		endRotation = endTransform.localRotation;
		endPosition = endTransform.localPosition;
		
		// if(isDelayRelativeToParent){
		// 	var temp = Target.transform.parent.GetComponentInParent<AnimatedEntry>();
		// 	if(temp != null)
		// 		delay += temp.delay;
		// }
		
	}

	IEnumerator Watch(){
		while (!runAnimation | isRunning){
			yield return null;
		}
		runAnimation = false;
		
		SetupVariables();
		StartCoroutine(Animation());
		StartCoroutine(Watch());
	}

	IEnumerator Animation() {
		isRunning = true;
		Target.transform.localPosition = startPosition;
		Target.transform.localRotation = startRotation;
		Target.transform.localScale = startScale;
		
		yield return new WaitForSecondsRealtime(delay);

		float time = 0;
		float perc = 0;
		float lastTime = Time.realtimeSinceStartup;

		do {
			time += Time.realtimeSinceStartup - lastTime;
			lastTime = Time.realtimeSinceStartup;
			perc = Mathf.Clamp01(time/effectTime);
			Vector3 tempScale = Vector3.LerpUnclamped(startScale, endScale, scaleCurve.Evaluate(perc));
			Quaternion tempRotation = Quaternion.LerpUnclamped(startRotation, endRotation, rotationCurve.Evaluate(perc));
			Vector3 tempPosition = Vector3.LerpUnclamped(startPosition, endPosition, positionCurve.Evaluate(perc));
			Target.transform.localScale = tempScale;
			Target.transform.localRotation = tempRotation;
			Target.transform.localPosition = tempPosition;
			yield return null;
		} 
		while (perc < 1);

		Target.transform.localPosition = endPosition;
		Target.transform.localRotation = endRotation;
		Target.transform.localScale = endScale;
		isRunning = false;

		yield return null;
	}

}
