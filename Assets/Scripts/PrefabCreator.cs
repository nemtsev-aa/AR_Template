using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class PrefabCreator : MonoBehaviour {
    [SerializeField] private GameObject _dronPrefab;
    [SerializeField] private Vector3 _prefabOffset;

    private GameObject _dron;
    private ARTrackedImageManager _aRTrackedImageManager;

    private void OnEnable() {
        _aRTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
        _aRTrackedImageManager.trackedImagesChanged += OnImageChanged;

    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj) {
        foreach (ARTrackedImage image in obj.added) {
            _dron = Instantiate(_dronPrefab, image.transform);
            _dron.transform.position += _prefabOffset;
        }
    }
}
