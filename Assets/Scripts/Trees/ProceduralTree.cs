using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTree : MonoBehaviour
{
    [SerializeField]
    [Range(0, 180)]
    float maxAngleToTree;
    [SerializeField]
    [Range(0, 180)]
    float maxAngleToUp;

    [SerializeField]
    [Range(0, 90)]
    float maxBendAngle;
    [SerializeField]
    [Range(0, 90)]
    float minAngleBetweenBranches;

    [SerializeField]
    float minBranchLength;
    [SerializeField]
    float maxBranchLength;

    [SerializeField]
    int maxGrowingAge;
    [SerializeField]
    int timeBetweenBranches;

    [SerializeField]
    int growIterations;
    [SerializeField]
    float secondsBetweenGrow;

    ProceduralBranch stem;

    void Start()
    {
        stem = new ProceduralBranch(transform.position, Vector3.up, maxAngleToTree, maxAngleToUp, maxBendAngle,minAngleBetweenBranches, minBranchLength, maxBranchLength, maxGrowingAge, timeBetweenBranches);
    }

    // Update is called once per frame
    float timeToNextGrow = 0;
    void Update()
    {
        stem.Update();
        if(timeToNextGrow > 0)
        {
            timeToNextGrow -= Time.deltaTime;
        }
        else
        {
            stem.Grow();
            timeToNextGrow = secondsBetweenGrow;
        }
    }
}
