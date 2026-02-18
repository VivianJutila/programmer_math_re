using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class bezier_path : MonoBehaviour
{
    [SerializeField, UnityEngine.Range(0f, 1f)]
    private float t_value;

    [SerializeField]
    private bool loop;

    [SerializeField]
    private List<bezier_point> bezier_path_points;

    private int segment_count;

    private void OnDrawGizmos()
    {
        if (!loop)
        {
			segment_count = bezier_path_points.Count - 1;
		} 
        else
        {
            segment_count = bezier_path_points.Count;
        }

        bezier_point _current_starting_point;
        bezier_point _current_ending_point;

        if (bezier_path_points != null && bezier_path_points.Count > 1)
        {
			if (GetCurrentStartingAndEndPoint(t_value).TryGetValue("start", out _current_starting_point) && GetCurrentStartingAndEndPoint(t_value).TryGetValue("end", out _current_ending_point))
			{
				Vector3 _current_point = LERPing.LERP_4_WAY_VECTORs(t_value * segment_count - Mathf.FloorToInt(t_value * segment_count), _current_starting_point.transform.position, _current_starting_point.Get_2nd_Control_Location(), _current_ending_point.Get_1st_Control_Location(), _current_ending_point.transform.position);

				Gizmos.color = Color.paleTurquoise;
				Gizmos.DrawSphere(_current_point, 0.2f);
			}
		}

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
			if (loop)
			{
                Handles.DrawBezier(bezier_path_points[bezier_path_points.Count - 1].transform.position, bezier_path_points[0].transform.position, bezier_path_points[bezier_path_points.Count - 1].Get_2nd_Control_Location(), bezier_path_points[0].Get_1st_Control_Location(), Color.darkGoldenRod, null, 3f);
			}
		}
    }

    private Dictionary<string, bezier_point> GetCurrentStartingAndEndPoint(float _t_value)
    {
        Dictionary<string, bezier_point> start_end_points = new Dictionary<string, bezier_point>();

        if (segment_count == bezier_path_points.Count - 1)
        {
			start_end_points.Add("start", bezier_path_points[Mathf.FloorToInt(_t_value * segment_count)]);
			start_end_points.Add("end", bezier_path_points[Mathf.CeilToInt(_t_value * segment_count)]);
		} 
        else
        {
            if (_t_value * segment_count > bezier_path_points.Count - 1)
            {
				start_end_points.Add("start", bezier_path_points[bezier_path_points.Count - 1]);
				start_end_points.Add("end", bezier_path_points[0]);
			} 
            else
            {
				start_end_points.Add("start", bezier_path_points[Mathf.FloorToInt(_t_value * segment_count)]);
				start_end_points.Add("end", bezier_path_points[Mathf.CeilToInt(_t_value * segment_count)]);
			}
        }

        return start_end_points;
    }
}
