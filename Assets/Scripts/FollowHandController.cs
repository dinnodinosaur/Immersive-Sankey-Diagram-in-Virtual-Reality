using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHandController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform handController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = handController.position;
        //transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

    }
}
