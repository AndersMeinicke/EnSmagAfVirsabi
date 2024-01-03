using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


/// <summary>
/// Sets the look at source constraint to main camera (usefull for prefabs instatiated by script)
/// </summary>
/// 
[RequireComponent(typeof(IConstraint))]
public class SetConstraintToMain : MonoBehaviour
{
    private Transform cameraTrans;
    private IConstraint constraint;


    void Start()
    {
        //Set look at constraint programatically
        cameraTrans = Camera.main.transform;
        ConstraintSource constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = cameraTrans;
        constraintSource.weight = 1;

        constraint = GetComponent<IConstraint>();

        constraint.AddSource(constraintSource);
        constraint.constraintActive = true;
    }
}
