using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Control : MonoBehaviour
{
    // Start is called before the first frame update
    public float RunSpeed;
    public float JumpSpeed;

    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private bool isGround;

    public bool isBegin = true;
    public bool isEnd = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Idle();
        Run();
        Filp();
        Jump();
        Attack();
    }
    void Idle()
    {
        if (! isBegin)
        {
            myAnim.SetBool("Idle", true);
        }
        else if(isEnd)
        {
            myAnim.SetBool("Idle", false);
        }
    }
    void Filp()
    {
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasXAxisSpeed)
        {
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);//人物反转，绕x,y,z轴转
            }
            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);//绕y轴180°
            }

        }
    }
    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");//人物左右移动
        Vector2 playerVel = new Vector2(moveDir * RunSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        bool playerHasXAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnim.SetBool("Run", playerHasXAxisSpeed);
        
        if (playerHasXAxisSpeed)
        {
            myAnim.SetBool("Stand", false);
        }
        else
        {
            myAnim.SetBool("Stand", true);
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CheckGround();
            if (isGround)
            {
                Vector2 jumpVel = new Vector2(0.0f, JumpSpeed);
                myRigidbody.velocity = Vector2.up * jumpVel;
                myAnim.SetBool("Stand", true);//用Stand代替jump；
            }
        }
    }
    void CheckGround()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
       // Debug.Log(isGround);
    }
    void Attack()
    {
        if (isBegin)
        {
            Hit();
        }
        else
        {
            Beat();
        }
    }
    void Hit()
    {
        if (Input.GetButtonDown("Attack"))
        {
            myAnim.SetTrigger("Hit");
            myAnim.SetBool("isBegin", true);
            isBegin = false;
        }        
    }
    void Beat()
    {
        if (Input.GetButtonDown("Attack") )
        {
            myAnim.SetTrigger("Beat");
        }
    }
}