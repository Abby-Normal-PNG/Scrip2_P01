using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CollectibleBase : MonoBehaviour
{
    protected abstract void Collect(ThirdPersonMovement player);

    [SerializeField] float _movementSpeed = 1;
    protected float MovementSpeed { get { return _movementSpeed; } }

    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;

    protected Rigidbody _rb;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonMovement player = other.GetComponent<ThirdPersonMovement>();
        if (player != null)
        {
            Collect(player);
            //spawn particles/fx beause we need to disable object
            CollectFeedback();
            DisableSelf();
        }
    }

    protected virtual void CollectFeedback()
    {
        //particles
        if (_collectParticles != null)
        {
            Instantiate(_collectParticles, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
        //audio TODO - Consider Object Pooling
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }

    protected virtual void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
