using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {

	public GameObject playerFPSView;
	public GameObject playerTopView;
	private Camera topViewCamera;
	private Camera FPSCamera;
	private SimpleSmoothMouseLook topViewController;
	private RigidbodyFirstPersonController FPSController;
	public GameObject Maze;
	private bool isInTopView;
	public Transform topViewTransform;
	private Transform camTransformBuffer;
	private AnimatedEntry Animator;
	private bool isRotationON;

	// Use this for initialization
	void Start () {
		isRotationON = false;
		FPSCamera = playerFPSView.GetComponentInChildren<Camera>();
		FPSController = playerFPSView.GetComponent<RigidbodyFirstPersonController>();
		
		topViewCamera = playerTopView.GetComponent<Camera>();
		topViewController = playerTopView.GetComponent<SimpleSmoothMouseLook>();
		
		topViewController.enabled = true;
		topViewCamera.enabled = false;
		FPSCamera.enabled = true;

		Animator = GetComponentInChildren<AnimatedEntry>();
		// Animator.Target = playerBody.gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
			CameraSwap();
        }

		if(isRotationON){
			Vector3 currentRotation = Maze.transform.rotation.eulerAngles;
			Maze.transform.Rotate(new Vector3(20,20,20) * Time.deltaTime, Space.World);
		}
		
	}

	public void CameraSwap(){
		
		//playerBody.isKinematic = !isInTopView;
		 // note that we are going to change isInTopView just below

		if (!isInTopView) {

            isInTopView = true;
			topViewCamera.enabled = true;
			//topViewController.enabled = true;

			FPSController.enabled = false;
			FPSCamera.enabled = false;
            // Saving current position & rotation in the maze
            //camTransformBuffer = playerBody.transform;

			// try to add: MouseLook.Init()? and put back the rotation
            AnimateCameraPositionAndRotation(FPSCamera.transform, topViewTransform);
        }
        else {

            isInTopView = false;
			topViewCamera.enabled = false;
			//topViewController.enabled = false;

			FPSController.enabled = true;
			FPSCamera.enabled = true;
            AnimateCameraPositionAndRotation(topViewTransform, FPSCamera.transform);

        }
        //playerBody.useGravity = !isInTopView;
	
	}

	void AnimateCameraPositionAndRotation(Transform fromTransform, Transform toTransform){
		Animator.startTransform = fromTransform;
		Animator.endTransform = toTransform;
		Animator.startAnimation();
	}

	public void toogleMazeRotation(){
		isRotationON = !isRotationON;
	}
}
