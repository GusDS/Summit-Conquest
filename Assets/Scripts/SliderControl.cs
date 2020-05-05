using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void EndSliderDragEventHandler(float val);

[RequireComponent(typeof(Slider))]
public class SliderDrag : MonoBehaviour, IPointerUpHandler
{
    public event EndSliderDragEventHandler EndDrag;

    private float SliderValue
    {
        get
        {
            Debug.Log("EndieRetValue");
            return gameObject.GetComponent<Slider>().value;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (EndDrag != null)
        {
            EndDrag(SliderValue);
            Debug.Log("EndieDrag");
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderControl : Slider
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Debug.Log("Soltoooooó");

        Control control = GameObject.Find("Control").GetComponent<Control>();
        control.isPointerUp = true;
        Debug.Log("Soltó");
    }
}
*/
