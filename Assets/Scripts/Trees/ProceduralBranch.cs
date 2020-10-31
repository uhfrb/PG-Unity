using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBranch
{
    private List<ProceduralBranch> branches;
    private int age;
    private Vector3 stem;
    private Vector3 root;

    private float maxAngleToTree;
    private float maxAngleToUp;

    private float maxBendAngle;
    private float minAngleBetweenBranches;

    private float minBranchLength;
    private float maxBranchLength;

    private int maxGrowingAge;
    private int timeBetweenBranches;

    public ProceduralBranch(Vector3 _root, Vector3 _stem, float _maxAngleToTree, float _maxAngleToUp, float _maxBendAngle, float _minAngleBetweenBranches, float _minBranchLength, float _maxBranchLength, int _maxGrowingAge, int _timeBetweenBranches)
    {
        branches = new List<ProceduralBranch>();
        age = 0;
        stem = _stem;
        root = _root;

        maxAngleToTree = _maxAngleToTree;
        maxAngleToUp = _maxAngleToUp;
        maxBendAngle = _maxBendAngle;
        minAngleBetweenBranches = _minAngleBetweenBranches;
        minBranchLength = _minBranchLength;
        maxBranchLength = _maxBranchLength;
        maxGrowingAge = _maxGrowingAge;
        timeBetweenBranches = _timeBetweenBranches;
    }

    public void Update()
    {
        foreach(ProceduralBranch b in branches)
        {
            b.Update();
        }
        Debug.DrawRay(root, stem, Color.green);
    }

    public void Grow()
    {
        if (age <= maxGrowingAge)
        {
            foreach (ProceduralBranch b in branches)
            {
                b.Grow();
            }
            if (age > 0 && age % timeBetweenBranches == 0)
            {
                Vector2 directionOffset = Random.insideUnitCircle * maxBendAngle * 20f;
                Vector3 branchDirection = new Vector3(stem.x, stem.y, stem.z);
                if (Vector3.AngleBetween(branchDirection, Vector3.up) > 1f)
                {
                    Vector3 xAxis = Vector3.Cross(branchDirection, Vector3.up);
                    Vector3 yAxis = Vector3.Cross(branchDirection, xAxis);

                    branchDirection = GeometricUtil.Rotate(branchDirection, xAxis, directionOffset.x);
                    branchDirection = GeometricUtil.Rotate(branchDirection, yAxis, directionOffset.y);

                }
                else
                {
                    directionOffset /= 360f;
                    branchDirection = (branchDirection + new Vector3(directionOffset.x, 0, directionOffset.y)).normalized * branchDirection.magnitude;
                }
                bool add = true;
                foreach (ProceduralBranch b in branches)
                {
                    if (b.InConflict(branchDirection))
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    branches.Add(new ProceduralBranch(root + stem, branchDirection.normalized * minBranchLength, maxAngleToTree, maxAngleToUp, maxBendAngle, minAngleBetweenBranches, minBranchLength, maxBranchLength, maxGrowingAge, timeBetweenBranches));
            }

            if (stem.magnitude <= maxBranchLength)
            {
                float newLength = 1 + Random.Range(0, maxBranchLength / stem.magnitude);
                stem *= newLength;
            }
            age++;
        }
    }

    public bool InConflict(Vector3 otherStem)
    {
        if (Mathf.Abs(Vector3.Angle(stem, otherStem)) < minAngleBetweenBranches) return true;
        return false;
    }
}
