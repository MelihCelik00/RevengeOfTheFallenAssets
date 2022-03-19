using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Params")] 
    [SerializeField] private float _patrolRange = 5f;
    [SerializeField] private float _moveSpeed = 6f;

    private Vector2 _initialPosition;
    private Vector2 _minPatrolPosition;
    private Vector2 _maxPatrolPosition;
    private Vector2 _destinationPoint;

    private void Awake()
    {
        _initialPosition = transform.position;

        _minPatrolPosition = _initialPosition + Vector2.left * _patrolRange;
        _maxPatrolPosition = _initialPosition + Vector2.right * _patrolRange;

        SetDestination(_maxPatrolPosition);

    }

    private void SetDestination(Vector2 destination)
    {
        _destinationPoint = destination;
    }

    private void Update()
    {
        if (Math.Abs(Vector2.Distance(transform.position,_maxPatrolPosition)) < 0.1f)
        {
            SetDestination(_minPatrolPosition);
        }
        else if (Math.Abs(Vector2.Distance(transform.position,_minPatrolPosition)) < 0.1f)
        {
            SetDestination(_maxPatrolPosition);
        }

        transform.position = Vector2.MoveTowards(transform.position, _destinationPoint, Time.deltaTime * _moveSpeed);

    }
}
