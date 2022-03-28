using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;
    public int Speed = 10;
    public bool Fight = false;
    public float PunchSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Fight)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector3(-Speed, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector3(Speed, 0, 0);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
