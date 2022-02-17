using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    [SerializeField] private Slider _rowsSlider;
    [SerializeField] private Slider _thicknessSlider;

    [SerializeField] private GameObject _planePrefab;
    [SerializeField] private GameObject _borderPrefab;
    [SerializeField] private GameObject _hitBoxPrefab;

    void Start() {
        GameManager.Instance.SetBoard(this);
        _rowsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _thicknessSlider.onValueChanged.AddListener(OnSliderValueChanged);

        Generate((int)_rowsSlider.value, _thicknessSlider.value);
    }

    void OnSliderValueChanged(float value = 0) {
        Generate((int)_rowsSlider.value, _thicknessSlider.value);
    }

    public void Generate(int rows, float thickness) {
        transform.Clear();
        GameManager.Instance.Clear();
        GameManager.Instance.Set(rows);

        GenerateField(rows, thickness, _planePrefab);
    }

    private void GenerateField(int rows, float thickness, GameObject parent) {
        var size = Vector3.Scale(parent.GetComponent<MeshFilter>().mesh.bounds.size,
            parent.transform.localScale);

        //generate borders
        for (int i = 1; i < rows; i++) {
            var totalSize = size.x - thickness * (rows - 1);
            var offset = totalSize / rows;
            var position = offset * i + thickness * i - thickness / 2.0f - size.x / 2.0f;

            var borderX = Instantiate(_borderPrefab, transform);
            borderX.transform.localScale = new Vector3(thickness, thickness, size.x);
            borderX.transform.localPosition = new Vector3(position, thickness / 2, 0);

            var borderY = Instantiate(_borderPrefab, transform);
            borderY.transform.localScale = new Vector3(size.x, thickness, thickness);
            borderY.transform.localPosition = new Vector3(0, thickness / 2, position);
        }

        // generate hitboxes
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < rows; j++) {
                var totalSize = size.x - thickness * (rows - 1);
                var offset = totalSize / rows;
                var positionX = thickness * i + offset * i + offset / 2.0f - size.x / 2.0f;
                var positionY = thickness * j + offset * j + offset / 2.0f - size.x / 2.0f;

                var hitbox = Instantiate(_hitBoxPrefab, transform);
                var localScale = new Vector3(offset,
                    thickness,
                    offset);
                hitbox.transform.localScale = localScale;
                hitbox.transform.localPosition = new Vector3(positionX,
                    localScale.y / 2,
                    positionY);

                GameManager.Instance.AddHitBox(hitbox.GetComponent<HitBox>(), i, j);
            }
        }
    }

    public void Reset() {
        OnSliderValueChanged();
    }
}