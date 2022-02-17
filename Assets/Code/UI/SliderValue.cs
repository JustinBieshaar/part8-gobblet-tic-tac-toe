using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;

    private void Start() {
        if (_slider == null) {
            _slider = GetComponent<Slider>();
        }

        _slider.onValueChanged.AddListener((v) => _text.text = v.ToString());
        _text.text = _slider.value.ToString();
    }
}