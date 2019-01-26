using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_script : MonoBehaviour
{
    [SerializeField]
    float forceValue = 4.5f;


    Rigidbody2D myBody;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.AddForce(new Vector2(forceValue * 50, 50));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
