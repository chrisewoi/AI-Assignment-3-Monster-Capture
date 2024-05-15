using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float rotateSpeed;
    public float maxRotation;
    bool startPos = true;
    private float knifeStart;
    private float currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        knifeStart = transform.rotation.z;
        currentRotation = 0; //knifeStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentRotation -= rotateSpeed * Time.deltaTime;
            transform.Rotate(0,0, -rotateSpeed * Time.deltaTime);
            Debug.Log("Mouse down triggered. currentRotation: " + currentRotation);
        }

        if (currentRotation < -maxRotation || Input.GetMouseButtonUp(0))
        {
            Debug.Log("reset triggered");
            transform.Rotate(0, 0, -currentRotation);
            currentRotation = 0;// knifeStart;
        }
    }

    
}
