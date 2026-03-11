using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class road_generation : MonoBehaviour
{
    [SerializeField]
    private CrossSection cross_section;

    [SerializeField, UnityEngine.Range(0.01f, 10f)]
    private float cross_section_scalar;

    [SerializeField, UnityEngine.Range(0f, 1f)]
    private float t_value;

    [SerializeField, UnityEngine.Range(10, 250)]
    private int road_segment_count;

    [SerializeField]
    private bool loop;

    [SerializeField]
    private List<bezier_point> bezier_path_points;

    [SerializeField]
    private bool draw_bezier_points;

	[SerializeField]
	private bool draw_underlying_bezier_curve;

	private int bezier_segment_count;

    private void OnDrawGizmos()
    {
        if (!loop)
        {
            bezier_segment_count = bezier_path_points.Count - 1;
        }
        else
        {
            bezier_segment_count = bezier_path_points.Count;
        }

        if (!draw_bezier_points)
        {
            foreach (bezier_point _bez_p in bezier_path_points)
            {
                _bez_p.drawing = false;
            }
        } 
        else if (draw_bezier_points && bezier_path_points[0].drawing == false)
        {
			foreach (bezier_point _bez_p in bezier_path_points)
			{
				_bez_p.drawing = true;
			}
		}

            bezier_point _current_starting_point;
        bezier_point _current_ending_point;

        Vector3 _last_right_direction = Vector3.zero;
        Vector3 _last_point = Vector3.zero;

        for (float i = 0f; i < 1f; i += 1f / road_segment_count)
        {
            if (GetCurrentStartingAndEndPoint(i).TryGetValue("start", out _current_starting_point) && GetCurrentStartingAndEndPoint(i).TryGetValue("end", out _current_ending_point))
            {
                Vector3 _current_point = LERPing.LERP_4_WAY_VECTORs(i * bezier_segment_count - Mathf.FloorToInt(i * bezier_segment_count), _current_starting_point.transform.position, _current_starting_point.Get_2nd_Control_Location(), _current_ending_point.Get_1st_Control_Location(), _current_ending_point.transform.position);

                Vector3 _current_facing_direction = LERPing.LERP_4_WAY_GET_FORWARD(i * bezier_segment_count - Mathf.FloorToInt(i * bezier_segment_count), _current_starting_point.transform.position, _current_starting_point.Get_2nd_Control_Location(), _current_ending_point.Get_1st_Control_Location(), _current_ending_point.transform.position);

                Vector3 _current_right_direction = Vector3.Cross(Vector3.up, _current_facing_direction);

                Gizmos.color = Color.gold;

                if (_last_right_direction != Vector3.zero && _last_point != Vector3.zero)
                {
					for (int j = 0; j < cross_section.vertices.Length - 2; j += 2)
					{
                        Vector3 _p1 = _current_point + (_current_right_direction * cross_section.vertices[j].point.x + Vector3.up * cross_section.vertices[j].point.y) * cross_section_scalar;
                        Vector3 _p2 = _last_point + (_last_right_direction * cross_section.vertices[j].point.x + Vector3.up * cross_section.vertices[j].point.y) * cross_section_scalar;

						Handles.DrawLine(_p1, _p2);
					}

					Handles.DrawLine(
                    _current_point + (_current_right_direction * cross_section.vertices[cross_section.vertices.Length - 1].point.x + Vector3.up * cross_section.vertices[cross_section.vertices.Length - 1].point.y) * cross_section_scalar,

					_last_point + (_last_right_direction * cross_section.vertices[cross_section.vertices.Length - 1].point.x + Vector3.up * cross_section.vertices[cross_section.vertices.Length - 1].point.y) * cross_section_scalar
                    );
				}

                for(int j = 0; j < cross_section.vertices.Length - 2; j+=2)
                {
                    Vector3 _p1 = _current_point + (_current_right_direction * cross_section.vertices[j].point.x + Vector3.up * cross_section.vertices[j].point.y) * cross_section_scalar;
                    Vector3 _p2 = _current_point + (_current_right_direction * cross_section.vertices[j + 2].point.x + Vector3.up * cross_section.vertices[j + 2].point.y) * cross_section_scalar;

                    Handles.DrawLine(_p1, _p2);
                }

                Handles.DrawLine(_current_point + (_current_right_direction * cross_section.vertices[cross_section.vertices.Length-1].point.x + Vector3.up * cross_section.vertices[cross_section.vertices.Length - 1].point.y) * cross_section_scalar,
                    _current_point + (_current_right_direction * cross_section.vertices[0].point.x + Vector3.up * cross_section.vertices[0].point.y) * cross_section_scalar);

                _last_point = _current_point;
                _last_right_direction = _current_right_direction;
            }
        }

        if (bezier_path_points != null && bezier_path_points.Count > 1)
        {
            if (GetCurrentStartingAndEndPoint(t_value).TryGetValue("start", out _current_starting_point) && GetCurrentStartingAndEndPoint(t_value).TryGetValue("end", out _current_ending_point))
            {
                Vector3 _current_point = LERPing.LERP_4_WAY_VECTORs(t_value * bezier_segment_count - Mathf.FloorToInt(t_value * bezier_segment_count), _current_starting_point.transform.position, _current_starting_point.Get_2nd_Control_Location(), _current_ending_point.Get_1st_Control_Location(), _current_ending_point.transform.position);

                Vector3 _current_facing_direction = LERPing.LERP_4_WAY_GET_FORWARD(t_value * bezier_segment_count - Mathf.FloorToInt(t_value * bezier_segment_count), _current_starting_point.transform.position, _current_starting_point.Get_2nd_Control_Location(), _current_ending_point.Get_1st_Control_Location(), _current_ending_point.transform.position);

                Vector3 _current_right_direction = Vector3.Cross(Vector3.up, _current_facing_direction);

                Quaternion _current_forward = Quaternion.LookRotation(_current_facing_direction);

                Handles.PositionHandle(_current_point, _current_forward);

                Gizmos.color = Color.paleTurquoise;
                Gizmos.DrawSphere(_current_point, 0.2f);
            }
        }

        if (bezier_path_points != null && draw_underlying_bezier_curve)
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

        if (bezier_segment_count == bezier_path_points.Count - 1)
        {
            start_end_points.Add("start", bezier_path_points[Mathf.FloorToInt(_t_value * bezier_segment_count)]);
            start_end_points.Add("end", bezier_path_points[Mathf.CeilToInt(_t_value * bezier_segment_count)]);
        }
        else
        {
            if (_t_value * bezier_segment_count > bezier_path_points.Count - 1)
            {
                start_end_points.Add("start", bezier_path_points[bezier_path_points.Count - 1]);
                start_end_points.Add("end", bezier_path_points[0]);
            }
            else
            {
                start_end_points.Add("start", bezier_path_points[Mathf.FloorToInt(_t_value * bezier_segment_count)]);
                start_end_points.Add("end", bezier_path_points[Mathf.CeilToInt(_t_value * bezier_segment_count)]);
            }
        }

        return start_end_points;
    }
}
