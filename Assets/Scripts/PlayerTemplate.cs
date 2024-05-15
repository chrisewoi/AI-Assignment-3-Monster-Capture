using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTemplate : MonoBehaviour
{
    private Vector3 _moveDir;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] private float _walkSpd, _jumpSpd, _grv;
    private CapsuleCollider _playerCollider;
    private Rigidbody _rb;
    private float distToGround;
    //the collider size to use for grounded checks
    private Vector3 _groundCheckBox = new(1f, 0.1f, 1f);
    public bool IsGrounded { get => Physics.BoxCast(transform.position, _groundCheckBox, Vector3.down, Quaternion.identity, distToGround); }

    public Vector3 Location => transform.position;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _playerCollider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        distToGround = _playerCollider.bounds.extents.y;// + 0.01f;
    }

    private void Update()
    {
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        _moveDir = GetInput(false);

        if (IsGrounded)
        {
            _moveDir.y = Mathf.Clamp(_moveDir.y, 0, float.PositiveInfinity);
            CheckJump();
        }
        else
            _moveDir.y -= _grv * Time.deltaTime;

        transform.Translate(_moveDir * Time.deltaTime);
    }

    private Vector3 GetInput(bool flatten)
    {
        return new Vector3(Input.GetAxis("Horizontal") * _walkSpd, flatten ? 0 : _moveDir.y, Input.GetAxis("Vertical") * _walkSpd);
    }

    private void CheckJump()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        _moveDir.y = _jumpSpd;
    }

}
