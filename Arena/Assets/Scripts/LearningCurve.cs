using UnityEngine;

public class LearningCurve : MonoBehaviour
{
    public int currentAge=30;
    public int addedAge=1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ComputeAge();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ComputeAge()
    {
        Debug.Log(currentAge+addedAge);
    }
}
