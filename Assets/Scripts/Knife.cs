using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Knife : MonoBehaviour
{

    public Rigidbody rb;

    public Vector2 startSwipe;
    public Vector2 endSwipe;

    public float startSwipeTime;

    public float force = 5f;
    public float torque = 20f;

    void Start()
    {

    }

    void Update()
    {

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startSwipe = Camera.main.ScreenToViewportPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endSwipe = Camera.main.ScreenToViewportPoint(touch.position);
                Swipe();
            }
        }
#endif

#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
#endif
    }

    void Swipe()
    {
        rb.isKinematic = false;

        startSwipeTime = Time.time;

        Vector2 swipe = endSwipe - startSwipe;

        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(0f, 0f, -torque, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            rb.isKinematic = true;
        }
        else
        {
            Restart();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        float timeInAir = Time.time - startSwipeTime;
        if (!rb.isKinematic && timeInAir >= .1f)
        {
            Restart();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
