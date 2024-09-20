using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableUnit : MonoBehaviour
{
    public float MoveSpeed { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void GoTo(Vector3 destPoint);

    public abstract void Attack(MovableUnit target);
}
