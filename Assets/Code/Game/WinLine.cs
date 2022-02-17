using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLine : MonoBehaviour {
    [SerializeField] private LineRenderer _line;

    private void Start() {
        _line.enabled = false;

        GameManager.Instance.OnGameEnd += GameEnd;
    }

    private void GameEnd(bool end, int winner) {
        _line.enabled = end && winner >= 0;

        if (winner < 0) {
            return;
        }

        var pattern = GameManager.Instance.Pattern;
        var linePattern = new Vector3[pattern.Count];
        for (var i = 0; i < pattern.Count; i++) {
            linePattern[i] = pattern[i].transform.position;
        }

        _line.positionCount = pattern.Count;
        _line.SetPositions(linePattern);
    }
}