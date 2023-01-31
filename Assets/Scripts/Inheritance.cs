using System;
using UnityEngine;

public sealed class CanNotInherited
{
    
}

// Can not inherited from sealed class
// public class Inherited : CanNotInherited
// {
//     
// }

public class Animal
{
    public int   Legs  { get; set; } = 2;
    public float Weigh { get; set; }

    public void ShowLegs() { Debug.Log($"Legs: {this.Legs}"); }
}

public class Cat : Animal
{
    public string food;

    public Cat()
    {
        this.Legs = 4;
        this.food    = "Mouse";
    }

    public void Eat() { Debug.Log($"Cat eat {this.food}"); }
}

public class Inheritance : MonoBehaviour
{
    private void Start()
    {
        var cat = new Cat();
        cat.ShowLegs();             
        cat.Eat();
    }
}