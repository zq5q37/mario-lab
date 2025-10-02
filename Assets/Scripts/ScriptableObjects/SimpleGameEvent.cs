using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void { } // dummy class
// no arguments
[CreateAssetMenu(fileName = "SimpleGameEvent", menuName = "ScriptableObjects/SimpleGameEvent", order = 3)]
public class SimpleGameEvent : GameEvent<Void>
{
    // create new method that doesn't accept any argument
    // calls base' Raise with Void arg
    public void Raise() => Raise(new Void()); // automatically create new Void data instead
}