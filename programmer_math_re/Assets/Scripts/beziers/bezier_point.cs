using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class bezier_point : MonoBehaviour
{
    [SerializeField]
    [UnityEngine.Range(0f, 0.5f)]
    private float sphere_size;

    [SerializeField]
    private List<Transform> controller_transforms = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.floralWhite;
        Gizmos.DrawSphere(transform.position, sphere_size);

        foreach (Transform t in controller_transforms)
        {
            if (t != null)
            {
                if (t.hasChanged)
                {
                    OnControlMove(t);
                    t.hasChanged = false;
                }
                Gizmos.color = Color.lightCoral;
                Gizmos.DrawLine(transform.position, t.position);
                Gizmos.color = Color.coral;
                Gizmos.DrawSphere(t.position, sphere_size);
            }
        }
    }

    public Vector3 Get_1st_Control_Location()
    {
        if (controller_transforms != null && controller_transforms[0] != null)
        {
            return controller_transforms[0].transform.position;
        }
        else
        {
            throw new System.Exception("There is no 1st anchor...");
        }
    }

    public Vector3 Get_2nd_Control_Location()
    {
        if (controller_transforms != null && controller_transforms[1] != null)
        {
            return controller_transforms[1].transform.position;
        }
        else
        {
            throw new System.Exception("There is no 2nd anchor...");
        }
    }

    private void OnControlMove(Transform _control)
    {
        if (controller_transforms[0] != _control)
        {
			controller_transforms[0].transform.position = transform.position + (-(_control.position - transform.position));
		}
        else
        {
            controller_transforms[1].transform.position = transform.position + (-(_control.position - transform.position));

		}
    }
}
