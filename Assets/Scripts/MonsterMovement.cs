using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform player;
    Vector3 directionToPlayer;
    public float monsterSpeed;
    public int erraticScale;

    public enum States
    {
        Idle,
        Alert,
        Escape,
        Target

    }

    public States state = States.Idle;


    // bagdf


    // Start is called before the first frame update
    void Start()
    {
        NextState();
        directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();
        transform.Rotate(Vector3.up, Random.Range(0,360));
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
            case States.Escape:
                StartCoroutine(EscapeState());
                break;
            case States.Target:
                StartCoroutine(TargetState());
                break;
        }
    }


    // 0-100 where 0 is 100% chance of moving and 100 is 100% chance of rotating
    int moveRotScale = 50; // midpoint - 50

    // Same concept as moveRotScale
    int rotationDirection = 50;

    // 1 or -1 for multiplying with rotation
    int binaryRotModifier = 1;

    float targetAngle = 0;

    IEnumerator IdleState()
    {
        while (state == States.Idle)
        {
            if (Random.Range(0,moveRotScale) >= 10)
            {
                // Move
                transform.position += transform.forward * monsterSpeed/10f * Time.deltaTime;
            } 
            else
            {
                // Rotate
                targetAngle += monsterSpeed * 30f * binaryRotModifier * Time.deltaTime;


                float rotSpeed = 100f;
                transform.Rotate(Vector3.up, Mathf.Clamp(targetAngle ,-rotSpeed * Time.deltaTime, rotSpeed * Time.deltaTime));

                transform.position += transform.forward * monsterSpeed / 20f * Time.deltaTime;
            }


            // ****** Binary randomisers
            if(Random.Range(0,2) == 1)
            {
                moveRotScale+= erraticScale;
            } else
            {
                moveRotScale-= erraticScale;
            }

            if (Random.Range(0, 2) == 1)
            {
                rotationDirection += erraticScale / 5;
                binaryRotModifier = 1;
            }
            else
            {
                rotationDirection -= erraticScale / 5;
                binaryRotModifier = -1;
            }
            // ******

            moveRotScale = Mathf.Clamp(moveRotScale, 0, 100);
            Debug.Log(rotationDirection);
            rotationDirection = Mathf.Clamp(rotationDirection, 0, 100);

            yield return null;
        }
    }

    IEnumerator AlertState()
    {
        yield return null;
    }

    IEnumerator EscapeState()
    {
        yield return null;
    }

    IEnumerator TargetState()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
