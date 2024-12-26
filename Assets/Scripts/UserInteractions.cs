using UnityEngine;

public class UserInteractions : MonoBehaviour
{
    // I want to click in the sim and get back the density at that point using calculateDensity
    public Simulation simulation;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(simulation.CalculateDensity(mousePosition));
        }
    }
}