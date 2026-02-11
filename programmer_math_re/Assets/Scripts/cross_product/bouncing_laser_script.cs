using UnityEditor;
using UnityEngine;

public class bouncing_laser_script : MonoBehaviour
{
    [SerializeField]
    [Range(1, 250)]
    private int max_bounces;

    [SerializeField]
    [Range(0f, 360f)]
    [Tooltip("Looking straight down is 0 degrees, turning clockwise in a degree between 0 to 360")]
    private float clockwise_degree_turn;

    [SerializeField]
    private bool draw_reflection_normals;

    private Vector3 _current_origin;
    private Vector3 _current_direction;

    private void OnDrawGizmos()
    {
		_current_origin = transform.position;

		Quaternion _rotator = Quaternion.AngleAxis(clockwise_degree_turn, Vector3.forward);
        _current_direction = _rotator * Vector3.down;

        for(int i = 0; i < max_bounces; i++)
        {
            DrawRayReflect(_current_origin, _current_direction);
        }
    }

    private Vector3 CalculateReflectionAngle(Vector3 _directional_vector, Vector3 _normal_vector)
    {
        _normal_vector = _normal_vector.normalized;

        Vector3 _result_vector = _directional_vector - 2*(GetDot(_directional_vector, _normal_vector))*_normal_vector;

        return _result_vector;
    }

    private void DrawRayReflect(Vector3 _origin, Vector3 _direction)
    {
        Ray _ray = new Ray(_origin, _direction);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit)) 
        {
            Handles.color = Color.coral;
			Handles.DrawLine(_origin, _hit.point);

			_current_origin = _hit.point;
            _current_direction = CalculateReflectionAngle(_direction, _hit.normal);

            if (draw_reflection_normals)
            {
                Drawing.draw_vector_from_to_position(_hit.point, _hit.normal, Color.gold, 0.02f);
            }
		}
    }

    private float GetDot(Vector3 _first, Vector3 _second)
    {
        float dot = _first.x * _second.x + _first.y * _second.y + _first.z * _second.z;

        return dot;
    }

}
