using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float mouseRotationSpeed = 400f;
    private float horizontalInput;
    private float mouseHorizontalInput;
    private Control control;

    void Start()
    {
        control = GameObject.Find("Control").GetComponent<Control>();
    }

    void Update()
    {
        if (control.inputKeyboard && control.isActionOn)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        }
        if (control.inputMouse && control.isActionOn)
        {
            mouseHorizontalInput = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseHorizontalInput * mouseRotationSpeed * Time.deltaTime);
        }
    }
}
