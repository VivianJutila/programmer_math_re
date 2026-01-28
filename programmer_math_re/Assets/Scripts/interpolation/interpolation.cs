using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class interpolation : MonoBehaviour
{
    [SerializeField]
    private GameObject lerp_object_A;

    [SerializeField]
    private GameObject lerp_object_B;

    [SerializeField]
    private GameObject lerp_visualizer_object;

    [SerializeField]
    [Range(0f, 1f)]
    private float interpolation_variable;

    [SerializeField]
    [Range(0.1f, 20f)]
    private float rewind_point_time;

    [SerializeField]
    private bool library_easing;

    [SerializeField]
    private EasingFunction.Ease ease_type = EasingFunction.Ease.EaseInOutQuad;
    private EasingFunction.Function ease_function;

    private bool is_rewinding = false;
    private float current_time = 0f;

    private void Update()
    {
        if (ease_function != EasingFunction.GetEasingFunction(ease_type))
        {
            ease_function = EasingFunction.GetEasingFunction(ease_type);
        }

        if (!is_rewinding)
        {
            current_time += Time.deltaTime;

            if (current_time >= rewind_point_time)
            {
                is_rewinding=true;
            }
        }
        else
        {
            current_time -= Time.deltaTime;
            
            if (current_time <= 0f)
            {
                is_rewinding = false;
            }
        }
        if (lerp_visualizer_object != null && lerp_object_A != null && lerp_object_B != null)
        {
            if (!library_easing)
            {
                lerp_visualizer_object.transform.position = ReturnInterpolationPoint(lerp_object_A.transform.position, lerp_object_B.transform.position, easeInOutQuart(current_time / rewind_point_time));
            }
            else
            {
                lerp_visualizer_object.transform.position = ReturnInterpolationPoint(lerp_object_A.transform.position, lerp_object_B.transform.position, ease_function(0f, 1f,(current_time / rewind_point_time)));
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        DrawLerpObjectVectors();

        DrawLerpCurrentPositionVectors(lerp_object_A.transform.position, lerp_object_B.transform.position);
    }

    void DrawLerpObjectVectors()
    {
        Drawing.draw_vector_from_to_position(Vector3.zero, lerp_object_A.transform.position, Color.red, 0.02f);
        Drawing.draw_vector_from_to_position(Vector3.zero, lerp_object_B.transform.position, Color.green, 0.02f);

        Handles.DrawLine(lerp_object_A.transform.position, lerp_object_B.transform.position);
    }

    void DrawLerpCurrentPositionVectors(Vector3 _lerp_point_a, Vector3 _lerp_point_b)
    {
        Vector3 lerp_position_A = new Vector3((1 - interpolation_variable) * _lerp_point_a.x,
        (1 - interpolation_variable) * _lerp_point_a.y,
        (1 - interpolation_variable) * _lerp_point_a.z);

        Vector3 lerp_position_B = new Vector3((interpolation_variable) * _lerp_point_b.x,
        (interpolation_variable) * _lerp_point_b.y,
        (interpolation_variable) * _lerp_point_b.z);

        Drawing.draw_vector_from_to_position(Vector3.zero,
        lerp_position_A,
        Color.darkRed,
        0.02f);

        Drawing.draw_vector_from_to_position(lerp_position_A,
        lerp_position_B,
        Color.darkGreen,
        0.02f);

        Drawing.draw_vector_from_to_position(Vector3.zero,
        lerp_position_B,
        Color.darkGreen,
        0.02f);

        Drawing.draw_vector_from_to_position(lerp_position_B,
        lerp_position_A,
        Color.darkRed,
        0.02f);
    }

    Vector3 ReturnInterpolationPoint(Vector3 _point_a,  Vector3 _point_b, float _interpolation_value)
    {
        Mathf.Clamp(_interpolation_value, 0f, 1f);
        return (1 - _interpolation_value) * _point_a + _interpolation_value * _point_b;
    }

    float easeInOutQuart(float _number)  
    {
        return _number < 0.5 ? 8 * _number * _number * _number * _number : 1 - Mathf.Pow(-2 * _number + 2, 4) / 2;
    }
}
