using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero : MovableUnit
{
    private Animator animator;
    private Rigidbody rigidbody;
    [SerializeField] private float speed;
    public GameObject playerInput;
    private Texture2D cursorIn;
    private Texture2D cursorOut;
    // Start is called before the first frame update
    void Start()
    {
        cursorOut = playerInput.GetComponent<PlayerInput>().cursor1;
        cursorIn = playerInput.GetComponent<PlayerInput>().cursor2;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GoTo(Vector3 destPoint)
    {
        Vector3 newPosition = Vector3.MoveTowards(rigidbody.position, destPoint, speed * Time.fixedDeltaTime);
        rigidbody.MovePosition(newPosition);
        
    }

    public override void Attack(MovableUnit target)
    {
        GoTo(target.transform.position);

    }

    private void OnMouseEnter()
    {
        //Debug.Log("cursorIn in");
        Cursor.SetCursor(cursorIn, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursorOut, Vector2.zero, CursorMode.Auto);
    }
}
