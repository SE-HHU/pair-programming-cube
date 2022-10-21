using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Transform[] MovePoints = new Transform[2];
    public float WaitTimer;
    public float WalkSpeed;
    public float RunSpeed;
    [HideInInspector]
    public bool Dead;

    private float Speed;
    private bool playerDead = false;
    private float WaitTime;
    private State state;
    private State Laststate = State.Idle;
    private GameObject Player;
    private Vector3 TargetPosition;
    private int PointIndex;
    private Animator animator;

    public enum State
    {
        Idle,
        Walk,
        Run,
        Death,
        Eat,
    }

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = this.GetComponent<Animator>();
    }
    void Start()
    {
        TargetPosition = MovePoints[0].position;
    }
    void FixedUpdate()
    {
        if (state != State.Death)
        {
            if (Dead)
            {
                state = State.Death;
            }
            else
            {
                if ((Player.transform.position.x > MovePoints[0].position.x) && (Player.transform.position.x < MovePoints[1].position.x))
                {
                    Chase();
                }
                else
                {
                    MoveAround();
                }
            }
        }
    }
    void MoveAround()
    {
        float Distance = Vector3.Distance(TargetPosition, this.transform.position);
        if (Distance > 1.0f)
        {
            state = State.Walk;
            this.transform.LookAt(TargetPosition);
            this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        else
        {
            if (Laststate == State.Run)
            {
                TargetPosition = new Vector3(MovePoints[0].position.x, this.transform.position.y, this.transform.position.z);
                Laststate = State.Idle;
            }
            state = State.Idle;
            WaitTime -= Time.deltaTime;
            if (WaitTime <= 0.0f)
            {
                if (TargetPosition.x == MovePoints[0].position.x)
                {
                    TargetPosition = MovePoints[1].position;
                }
                else if (TargetPosition.x == MovePoints[1].position.x)
                {
                    TargetPosition = MovePoints[0].position;
                }
                WaitTime = WaitTimer;
            }
        }
        JudgeState();
    }
    void Chase()
    {
        if (!playerDead)
        {
            state = State.Run;
            TargetPosition = new Vector3(Player.transform.position.x, this.transform.position.y, this.transform.position.z);
            this.transform.LookAt(TargetPosition);
            this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        else
        {
            state = State.Eat;
        }
        Laststate = State.Run;
        JudgeState();
    }

    void JudgeState()
    {
        if (state == State.Idle)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Eat", false);
            Speed = 0.0f;
        }
        else if (state == State.Walk)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Eat", false);
            animator.SetBool("Walk", true);
            Speed = WalkSpeed;
        }
        else if (state == State.Run)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Eat", false);
            animator.SetBool("Walk", false);
            Speed = RunSpeed;
        }
        else if (state == State.Eat)
        {
            animator.SetBool("Eat", true);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            Speed = 0.0f;
        }
        else if (state == State.Death)
        {
            animator.SetBool("Dead", true);
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Eat", false);
            Speed = 0.0f;
            Destroy(this.gameObject, 3);
        }
    }
}