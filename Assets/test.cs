using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        Debug.Log($"wheel = {wheel} || mouseX = {mouseX} || mouseY = {mouseY}");



    }
}
