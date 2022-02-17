        using System;
        using DG.Tweening;
        using UnityEngine;

        public class Marker : MonoBehaviour {
            [SerializeField] private float _addYOnSelected = 1;
            [SerializeField] private float _addYOnHover = .2f;
            [SerializeField] private float _moveDuration = .5f;
            [SerializeField] private float _moveDurationY = .25f;
            [SerializeField] private bool _scaleOnPositioning = true;
            [SerializeField] private float _scaleDuration = .25f;

            public Action<bool, Marker> OnSelected;

            private int _turnType;
            private int _size;
            private bool _selected;
            private bool _isOverRuled;
            private Vector3 _cachePosition;
            private BoxCollider _collider;
            private HitBox _hitBox;

            private bool CheckTurn {
                get {
                    var turn = _turnType == GameManager.Instance.Turn;
                    if (!turn && _selected) {
                        Deselect();
                    }

                    return _turnType == GameManager.Instance.Turn;
                }
            }

            public int Size => _size;
            public int Type => _turnType;

            private void Start() {
                _cachePosition = transform.position;

                _collider = GetComponent<BoxCollider>();
            }

            public void Check() {
                if (_isOverRuled) {
                    _collider.enabled = false;
                    return;
                }

                _collider.enabled = CheckTurn;
            }

            private void OnMouseEnter() {
                if (!CheckTurn) {
                    return;
                }

                AddYPosition(_addYOnHover);
            }

            private void OnMouseExit() {
                if (!CheckTurn) {
                    return;
                }

                AddYPosition();
            }

            private void OnMouseUpAsButton() {
                if (!CheckTurn) {
                    return;
                }

                if (_selected) {
                    Deselect();
                }
                else {
                    AddYPosition(_addYOnSelected);

                    _selected = true;
                }

                OnSelected?.Invoke(_selected, this);
            }

            public void Deselect(bool move = true) {
                _selected = false;
                AddYPosition(move: move);
            }
            
            private void AddYPosition(float addY = 0, bool move = true) {
                var cachePosition = _cachePosition;
                if (_selected) {
                    cachePosition.y += _addYOnSelected;
                }

                if (!move) {
                    return;
                }

                var pos = cachePosition;
                pos.y += addY;

                transform.DOMoveY(pos.y, _moveDurationY);
                // no more; transform.position = pos;
            }
            
            public void SetPosition(Vector3 position, Transform parent = null, HitBox hitBox = null) {
                Deselect(false);
                _cachePosition = position;

                if (_hitBox != null) {
                    _hitBox.RemoveMarker(this);
                }

                _hitBox = hitBox;

                if (parent) {
                    transform.SetParent(parent);
                }

                transform.DOMoveX(position.x, _moveDuration);
                transform.DOMoveZ(position.z, _moveDuration).OnComplete(() => {
                    transform.DOMoveY(position.y, _moveDurationY);
                    if (_scaleOnPositioning) {
                        transform.DOScale(new Vector3(1, 6, 1), _scaleDuration);
                    }
                });

                OnSelected?.Invoke(_selected, this);
            }

            public void SetTurn(int turn) {
                _turnType = turn;
            }

            public void SetSize(int size) {
                _size = size;
            }

            public void OverRuled(bool overRuled) {
                _isOverRuled = overRuled;
            }
        }