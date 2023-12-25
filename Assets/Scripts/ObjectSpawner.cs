using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager))] 
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPref;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Transform spawnedObjectTransform;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnTouch;
    }

    private void OnTouch(EnhancedTouch.Finger finger)
    {
        var screenCenter = new Vector2(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f);
        
        if(raycastManager.Raycast(screenCenter, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            if(spawnedObjectTransform == null)
                spawnedObjectTransform = SpawnPrefab().transform;

            PlacePrefab(raycastHits[0].pose.position);
        }
    }

    private GameObject SpawnPrefab() => Instantiate(spawnPref);

    private void PlacePrefab(Vector3 position) => spawnedObjectTransform.position = position;

    private void OnDestroy()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnTouch;
    }
}
