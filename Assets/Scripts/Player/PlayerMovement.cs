using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static bool running = true;
    public Camera cam;

    NavMeshAgent agent;
    Rigidbody m_Rigidbody;

    public enum PlayerState
    {
        Idle,
        MovingLeft,
        MovingRight,
        MovingUp,
        MovingDown
    }

    public PlayerState State{
        get{
            return state;
        }
    }
    [SerializeField] PlayerState state;

    [SerializeField, Range(0f,0.1f)] float minVelocityFoUpdateState = 0.1f;
 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(running){
            if(Input.GetMouseButton(0)){
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit)){
                    agent.SetDestination(hit.point);
                }
            }
        }
        else{
            agent.SetDestination(transform.position);
        }
        
        UpdatePlayerMovementState();
    }

    void UpdatePlayerMovementState(){
        Vector3 velocity = agent.velocity;
        
        // up
        if(velocity.z > minVelocityFoUpdateState && velocity.z > Mathf.Abs(velocity.x)){
            state = PlayerState.MovingUp;
        }
        //up but left or right
        else if(velocity.z > minVelocityFoUpdateState && velocity.z < Mathf.Abs(velocity.x)){
            if(velocity.x > minVelocityFoUpdateState){
                state = PlayerState.MovingRight;
            }
            else{
                state = PlayerState.MovingLeft;
            }
        }
        else if(velocity.z < -minVelocityFoUpdateState && -velocity.z > Mathf.Abs(velocity.x)){
            state = PlayerState.MovingDown;
        }
        //down
        else if(velocity.z < -minVelocityFoUpdateState && -velocity.z < Mathf.Abs(velocity.x)){
            if(velocity.x > minVelocityFoUpdateState){
                state = PlayerState.MovingRight;
            }
            else{
                state = PlayerState.MovingLeft;
            }
        }
        //idle
        else{
            state = PlayerState.Idle;
        }
    }
}
