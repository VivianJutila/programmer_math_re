using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

[InitializeOnLoad]
public class object_placement : MonoBehaviour
{
    [SerializeField]
    private bool place_object;

    [SerializeField]
    private bool draw_rotation_vectors;

    [SerializeField]
    private bool do_right_check;

    [SerializeField]
    private GameObject placeable_prefab;

    [SerializeField]
    private GameObject right_checker_game_object;

    private static GameObject _current_prefab_instance;

    [SerializeField]
    [Range(-180, 180f)]
    [Tooltip("The amount of degrees the object is rotated clockwise on the front axis")]
    private float frontal_rotation_modifier = 0;

	[SerializeField]
	[Range(0f, 360f)]
	[Tooltip("The amount of degrees the object is rotated clockwise on the down axis")]
	private float down_rotation_modifier = 0;

    private void OnDrawGizmos()
    {
        Ray modifier_ray = MakeRayFromRotationModifiers(frontal_rotation_modifier, Vector3.forward, down_rotation_modifier, Vector3.up);
        RaycastHit _hit;

        if (Physics.Raycast(modifier_ray, out _hit))
        {
            Color _orig = Handles.color;
            Handles.color = Color.gold;
            Handles.DrawLine(transform.position, _hit.point);
            Handles.color = _orig;

            if (draw_rotation_vectors)
            {
				DrawAxisOnList(_hit.point, ComputeAxisOnRaycastHit(_hit, modifier_ray.direction));
			}

            if (place_object)
            {
                PlaceObjectOnList(_hit.point, ComputeAxisOnRaycastHit(_hit, modifier_ray.direction), placeable_prefab);
            }
            else if (!place_object && _current_prefab_instance != null)
            {
                DestroyImmediate(_current_prefab_instance);
                _current_prefab_instance = null;
            }

            if (do_right_check && _current_prefab_instance != null)
            {
                Quaternion _rotator = _current_prefab_instance.transform.rotation;

                Color handle_color = Color.white;

                Vector3 _c_t = _current_prefab_instance.transform.position;
                Vector3 _r_t = right_checker_game_object.transform.position;

                if (Vector3.Dot(_rotator * Vector3.right, (new Vector3(_r_t.x - _c_t.x, 0, _r_t.z - _c_t.z)).normalized) > 0)
                {
                    handle_color = Color.green;
                }
                else
                {
                    handle_color = Color.red;
                }

                Handles.color = handle_color;

                Handles.DrawLine(_current_prefab_instance.transform.position, right_checker_game_object.transform.position);

                Handles.color = _orig;
            }
        } 
        else
        {
            if (_current_prefab_instance != null)
            {
                DestroyImmediate(_current_prefab_instance);
                _current_prefab_instance = null;
            }
        }
    }

    private Ray MakeRayFromRotationModifiers(float _angle1, Vector3 _axis1, float _angle2, Vector3 _axis2)
    {
        Quaternion rotator1 = Quaternion.AngleAxis(_angle1, _axis1);
        Quaternion rotator2 = Quaternion.AngleAxis(_angle2, _axis2);

        Ray result_ray = new Ray(transform.position, (rotator2 * (rotator1 * Vector3.down)));

        return result_ray;
    }

    private Dictionary<Color, Vector3> ComputeAxisOnRaycastHit(RaycastHit _hit, Vector3 _ray_direction)
    {
        Dictionary<Color, Vector3> axis = new Dictionary<Color, Vector3>();

        axis.Add(Color.green, _hit.normal.normalized);
        Vector3 _temp_right = Vector3.Cross(_ray_direction, axis[Color.green]).normalized;

        axis.Add(Color.blue, Vector3.Cross(axis[Color.green], _temp_right).normalized);

        axis.Add(Color.red, Vector3.Cross(axis[Color.green], axis[Color.blue]).normalized);

        return axis;
    }

    private void DrawAxisOnList(Vector3 _origin, Dictionary<Color, Vector3> _axis_list)
    {
        foreach (KeyValuePair<Color, Vector3> _key_value in _axis_list)
        {
            Drawing.draw_vector_from_to_position(_origin, _key_value.Value, _key_value.Key, 0.02f);
        }
    }

    private void PlaceObjectOnList(Vector3 _world_position, Dictionary<Color, Vector3> _axis_list, GameObject _prefab)
    {
        if (_current_prefab_instance == null && _prefab != null)
        {
            _current_prefab_instance = Instantiate(_prefab);
        }

        if (_current_prefab_instance != null)
        {
            _current_prefab_instance.transform.position = _world_position;
			_current_prefab_instance.transform.rotation = Quaternion.LookRotation(_axis_list[Color.blue]);
		}
    }
 }
