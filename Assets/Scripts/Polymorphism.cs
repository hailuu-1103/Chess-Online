using UnityEngine;

public abstract class Quadrangle
{
    public virtual void CalculateArea()
    {
        Debug.Log("The area of quadrangle is not defined.");
    }

    public virtual void CalculateArea(int edge)
    {
        Debug.Log("The area of quadrangle is not defined.");
    }
    
    // Overloading
    public virtual void CalculateArea(int horizontalEdge, int verticalEdge)
    {
        Debug.Log("The area of quadrangle is not defined.");
    }
}

public class Square : Quadrangle
{
    // Overriding
    public override void CalculateArea()
    {
        Debug.Log("The area of the square is not defined.");
    }

    public override void CalculateArea(int edge)
    {
        Debug.Log($"The area of the square is : {edge * edge}");
    }

    public override void CalculateArea(int horizontalEdge, int verticalEdge)
    {
        Debug.Log($"The area of the square is : {horizontalEdge * horizontalEdge}");
    }
}

public class Rectangle : Quadrangle
{
    public override void CalculateArea()
    {
        Debug.Log("The area of the rectangle is not defined.");
    }

    public override void CalculateArea(int edge)
    {
        Debug.Log("The area of the rectangle is not defined.");
    }

    public override void CalculateArea(int horizontalEdge, int verticalEdge)
    {
        Debug.Log($"The area of the rectangle is : {horizontalEdge * verticalEdge}");
    }
}

public class Polymorphism : MonoBehaviour
{
    private void Start()
    {
        Quadrangle myRect = new Rectangle();
        myRect.CalculateArea();
        myRect.CalculateArea(5, 7);
        Quadrangle mySquare = new Square();
        mySquare.CalculateArea();
        mySquare.CalculateArea(4);
    }
}