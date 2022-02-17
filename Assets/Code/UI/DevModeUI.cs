using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeUI : MonoBehaviour {
    [SerializeField] private GameObject _devUI;

    void Start() {
        _devUI.SetActive(GameManager.Instance.DevMode);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            _devUI.SetActive(GameManager.Instance.ToggleDevMode());
        }
    }
}