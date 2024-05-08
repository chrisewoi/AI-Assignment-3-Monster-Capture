using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform player;
    Vector3 directionToPlayer;

    public enum States
    {
        Idle,
        Alert,

    }

    public States state = States.Idle;




    // Start is called before the first frame update
    void Start()
    {
        NextState();
        directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
    }

    void NextState()
    {
        switch (state)
        {
            case States.Idle:
                StartCoroutine(IdleState());
                break;
            case States.Alert:
                StartCoroutine(AlertState());
                break;
        }
    }

    IEnumerator IdleState()
    {
        while (state == States.Idle)
        {
            if (Random.Range(0,2) == 1)
            {
                transform.position += transform.forward * Time.deltaTime;
            } else
            {
                transform.RotateAround(Vector3.up, Time.deltaTime)
            }

            yield return null;
        }
    }

    IEnumerator AlertState()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
