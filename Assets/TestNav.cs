using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestNav : MonoBehaviour
{
    public Camera _cam;
    public NavMeshAgent _agent;

    public Rigidbody _rb;

    public int value;


    // public IntBuffTarget Target;
    // void Start()
    // {
    //     _cam = Camera.main;
    //     _agent = GetComponent<NavMeshAgent>();
    //     _rb = GetComponent<Rigidbody>();
    // }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         _rb.AddForce(_rb.transform.up * value, ForceMode.Impulse);
    //         // RaycastHit hit;
    //         // if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit))
    //         // {
    //         //     _agent.SetDestination(hit.point);
    //         // }
    //     }
    // }
}
