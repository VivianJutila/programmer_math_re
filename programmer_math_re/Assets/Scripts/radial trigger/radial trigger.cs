using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class radialtrigger : MonoBehaviour
{
    [SerializeField]
    [UnityEngine.Range(0.1f, 20f)]
    private float trigger_radius;

    [SerializeField]
    [UnityEngine.Range(0f, 360f)]
    private float field_of_view;

    [SerializeField]
    [UnityEngine.Range(0.1f, 30f)]
    private float cheese_block_height;

    private enum Trigger_Type
    {
        Cheese,
        Slice,
        Circle
    }

	[SerializeField]
    private Trigger_Type trigger_type;

	[SerializeField]
    private List<GameObject> target_list = new List<GameObject>();

    [SerializeField]
    private GameObject look_direction_object;

    private bool target_hit = false;
    private bool looking_at_any = false;

    private void OnDrawGizmos()
    {
        Quaternion rotator = Quaternion.AngleAxis(field_of_view /  2f, Vector3.forward);
        List<Vector3> _target_vector_on_correct_plane = new List<Vector3>();
        List<float> dots = new List<float>();
        int _cur_index = 0;

        foreach (GameObject _target in target_list)
        {
            float _target_dot = 0f;

            if (look_direction_object != null && _target != null)
            {
                _target_dot = Vector3.Dot((new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z) - transform.position).normalized, (new Vector3(look_direction_object.transform.position.x, look_direction_object.transform.position.y, transform.position.z) - transform.position).normalized);

                dots.Add(_target_dot);
            }

            if (_target != null)
                {
                    _target_vector_on_correct_plane.Add(new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z));
                    Color _color = ReturnColorOnFalseOrTrue((_target_vector_on_correct_plane[_cur_index] - transform.position).magnitude > trigger_radius || _target_dot < Mathf.Cos((field_of_view / 2) * Mathf.Deg2Rad));

                    if (target_hit != true)
                    {
                        target_hit = (_target_vector_on_correct_plane[_cur_index] - transform.position).magnitude < trigger_radius;
                    }

                    Drawing.draw_vector_from_to_position(transform.position, _target_vector_on_correct_plane[_cur_index], _color, 0.02f);
                    _cur_index++;
                }

        }

        if (look_direction_object != null)
        {
            Drawing.draw_vector_from_to_position(transform.position, look_direction_object.transform.position, Color.coral, 0.02f);

            Vector3 _look_at_rotated = rotator * look_direction_object.transform.position;
            Vector3 _look_at_rotated_inverse = Quaternion.Inverse(rotator) * look_direction_object.transform.position;

            Drawing.draw_vector_from_to_position(transform.position, _look_at_rotated.normalized * trigger_radius, Color.lightCoral, 0.02f);
            Drawing.draw_vector_from_to_position(transform.position, _look_at_rotated_inverse.normalized * trigger_radius, Color.lightCoral, 0.02f);
            
            Handles.color = Color.lightCoral;
            Handles.DrawWireArc(transform.position, Vector3.forward, _look_at_rotated_inverse, field_of_view, trigger_radius);
        }

        Handles.color = ReturnColorOnFalseOrTrue(target_hit);

        Handles.DrawWireDisc(transform.position, Vector3.forward, trigger_radius);

        foreach (float _dot in dots)
        {
            Debug.Log(_dot);
        }
    }

    Color ReturnColorOnFalseOrTrue(bool _statement)
    {
        Color _return_color;
        _return_color = _statement ? Color.green : Color.red;

        return _return_color;
    }

    private void DrawCheeseTrigger()
    {

    }

    private void DrawSliceTrigger()
    {

    }

    private void DrawSphereTrigger()
    {

    }
}
