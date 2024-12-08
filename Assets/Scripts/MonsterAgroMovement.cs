using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAgroMovement : MonoBehaviour
{
    public Transform player;
    Vector3 directionToPlayer;
    public float monsterSpeed;
    public int erraticScale;
    public float alertDistance = 8f;
    public float escapeDistance = 6f;
    public float destroyDistance;

    public enum States
    {
        Idle,
        Alert,
        Escape,
        Target

    }

    public States state = States.Idle;



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
                Debug.Log("Entering Idle state");
                StartCoroutine(IdleState());
                break;
            case States.Alert:
                Debug.Log("Entering Alert state");
                StartCoroutine(AlertState());
                break;
            case States.Escape:
                Debug.Log("Entering Escape state");
                StartCoroutine(EscapeState());
                break;
            case States.Target:
                Debug.Log("Entering Target state");
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

    bool posBump = false;
    float bumpAmount = 0.015f;

    float targetAngle = 0;

    IEnumerator IdleState()
    {
        while (state == States.Idle)
        {
            if (Random.Range(0,moveRotScale) >= 10)
            {
                // Move
                transform.position += transform.forward * monsterSpeed/10f * Time.deltaTime;
                if (posBump)
                {
                    transform.position += new Vector3(0,bumpAmount,0);
                } else
                {
                    transform.position += new Vector3(0, -bumpAmount, 0);
                }
                posBump = !posBump;
                
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
            rotationDirection = Mathf.Clamp(rotationDirection, 0, 100);

            // If player close enough to alert, go to alert state
            if(Vector3.Distance(transform.position, player.position) < alertDistance)
            {
                state = States.Alert;
            }

            yield return null;
        }
        NextState();
    }

    IEnumerator AlertState()
    {
        Vector3 saveScale = transform.localScale;
        float startTime = Time.time;

        while (state == States.Alert)
        {
            float wave = Mathf.Sin(Time.time * 30f) * 0.1f + 1f;
            float wave2 = Mathf.Cos(Time.time * 30f) * 0.1f + 1f;
            transform.localScale = new Vector3(wave, wave2, wave);

            float shimmy = Mathf.Sin(Time.time * 30f) * 0.9f + 0.3f;

            transform.position += transform.right * shimmy * Time.deltaTime;

            Vector3 playerDirection = player.position - transform.position;
            playerDirection.y = 0f; // locks rotation to y axis
            playerDirection.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f), monsterSpeed * 25f * Time.deltaTime);

            // If player too close then go to target state
            if (Vector3.Distance(transform.position, player.position) < escapeDistance)
            {
                state = States.Target;
            }

            // If player too far then go back to idle state
            if (Vector3.Distance(transform.position, player.position) > alertDistance)
            {
                state = States.Idle;
            }

            // If player alerting for too long then go to target state
            if (Time.time - startTime > 3f)
            {
                state = States.Target;
            }

            //If moving to Escape state - do a 180
            if(state == States.Escape)
            {
                transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
            }


            yield return null;
        }
        transform.localScale = saveScale;
        NextState();
    }

    IEnumerator EscapeState()
    {
        float startTime = Time.time;
        Vector3 saveScale = transform.localScale;

        while (state == States.Escape)
        {
            float wave = Mathf.Sin(Time.time * 30f) * 0.1f + 1f;
            float wave2 = Mathf.Cos(Time.time * 30f) * 0.1f + 1f;
            transform.localScale = new Vector3(wave, wave2, wave);

            float shimmy = Mathf.Sin(Time.time * 30f) * 0.9f + 0.3f;

            transform.position += transform.right * shimmy * Time.deltaTime;

            transform.position += transform.forward * monsterSpeed / 5f * Time.deltaTime;

            if (Time.time - startTime > 3f)
            {
                state = States.Idle;
            }
            yield return null;
        }
        transform.localScale = saveScale;
        NextState();
    }

    IEnumerator TargetState()
    {
        // Make a new monster type that is agro. This is the passive one. Target wont be used here
        
        float startTime = Time.time;
        Vector3 saveScale = transform.localScale;

        while (state == States.Target)
        {
            float wave = Mathf.Sin(Time.time * 45f) * 0.15f + 1f;
            float wave2 = Mathf.Cos(Time.time * 20f) * 0.15f + 1f;
            transform.localScale = new Vector3(wave, wave2, wave);

            float shimmy = Mathf.Sin(Time.time * 45f) * 0.9f + 0.3f;

            transform.position += transform.right * shimmy * Time.deltaTime;

            transform.position += transform.forward * monsterSpeed / 5f * Time.deltaTime;

            if (Time.time - startTime > 3f)
            {
                state = States.Idle;
            }
            yield return null;
        }
        transform.localScale = saveScale;
        NextState();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy monster when attacked in range
        if (Vector3.Distance(transform.position, player.position) <= destroyDistance && Input.GetMouseButton(0))
        {
            // Destroy the object
            Destroy(gameObject);
            Debug.Log("MONSTER DESTROYED");
        }
    }
}
