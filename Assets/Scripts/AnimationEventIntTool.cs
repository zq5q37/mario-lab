using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntTool : MonoBehaviour
{
    public int parameter;
    public UnityEvent<int> useInt;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerIntEvent()
    {

        useInt.Invoke(parameter); // safe to invoke even without callbacks

    }
}