using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class bezier_path : MonoBehaviour
{
    [SerializeField]
    private List<bezier_point> bezier_path_points;

    private void OnDrawGizmos()
    {
        if (bezier_path_points != null) 
        {
            for (int i = 0; i < bezier_path_points.Count - 1; ++i)
            {
                if (bezier_path_points[i] != null)
                {
                    Handles.DrawBezier(bezier_path_points[i].transform.position,
                    bezier_path_points[i + 1].transform.position,
                    bezier_path_points[i].Get_2nd_Control_Location(),
                    bezier_path_points[i + 1].Get_1st_Control_Location(),
                    Color.darkGoldenRod,
                    null,
                    3f);
                }
            }
        }
    }
}
