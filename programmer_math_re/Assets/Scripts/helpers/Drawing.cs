using UnityEditor;
using UnityEngine;

public class Drawing
{
    public static void draw_vector_from_to_position (Vector3 _vector_starting_point, Vector3 _draw_vector, Color _color, float _thickness = 0.02f)
    {
        Color original = Handles.color;

        Handles.color = _color;

        Handles.DrawLine(_vector_starting_point, _vector_starting_point + _draw_vector, _thickness);

        Handles.ConeHandleCap(0, _vector_starting_point + _draw_vector - _draw_vector.normalized * HandleUtility.GetHandleSize(_draw_vector) * 0.19f, 
        Quaternion.LookRotation(_draw_vector), 0.3f*HandleUtility.GetHandleSize(_vector_starting_point + _draw_vector), EventType.Repaint);

        Handles.color = original;
    }

    public static void draw_rectangle_from_center_position(Vector3 _center_position, float _width, float _height, Color _color, float _thickness = 0.02f)
    {
        Color original = Handles.color;

        Handles.color = _color;

        Vector3 bottom_left = new Vector3(_center_position.x - _width / 2, _center_position.y - _height / 2, 0);
        Vector3 top_right = new Vector3(_center_position.x + _width / 2, _center_position.y + _height / 2, 0);

        Handles.DrawLine(bottom_left, new Vector3(bottom_left.x + _width, bottom_left.y, bottom_left.z));
        Handles.DrawLine(bottom_left, new Vector3(bottom_left.x , bottom_left.y + _height, bottom_left.z));

        Handles.DrawLine(top_right, new Vector3(top_right.x - _width, top_right.y, top_right.z));
        Handles.DrawLine(top_right, new Vector3(top_right.x, top_right.y - _height, top_right.z));

        Handles.color = original;
    }
}
