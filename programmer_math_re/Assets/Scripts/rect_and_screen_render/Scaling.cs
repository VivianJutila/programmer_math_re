using UnityEngine;

public class Scaling : MonoBehaviour
{

    // Main screen inspector elements
    public int screen_width = 1920;
    public int screen_height = 1080;
    public Vector2Int screen_position = Vector2Int.zero;


    // Popup inspector elements
    [Range(0, 100)]
    public int popup_coverage_percentage_x = 0;
    [Range(0, 100)]
    public int popup_coverage_percentage_y = 0;
    public Vector2Int popup_screen_center_offset = Vector2Int.zero;


    // Health bar inspector elements
    private enum corner_options{top_right, top_left, bottom_right, bottom_left};

    [SerializeField] private corner_options healthbar_display_corner;

    [Range(0, 100)]
    public int hpbar_x_offset_from_corner = 0;
    [Range(0, 100)]
    public int hpbar_y_offset_from_corner = 0;
    [Range(0, 100)]
    public int hpbar_size_prcentage_height = 0;
    [Range(0, 100)]
    public int hpbar_size_prcentage_width = 0;

    private float hpbar_height = 0;
    private float hpbar_width = 0;


    private void OnDrawGizmos()
    {
        UpdateValues();

        DrawVectorToScreen();

        DrawRectScreen();

        DrawPopup();

        DrawHealthBar(healthbar_display_corner);
    }

    void UpdateValues() 
    {
        hpbar_width = screen_width * (hpbar_size_prcentage_width / 100f);
        hpbar_height = screen_height * (hpbar_size_prcentage_height / 100f);
    }

    void DrawHealthBar(corner_options _dp_corner)
    {
        if (_dp_corner != null) 
        {
            if (_dp_corner == corner_options.top_right) 
            {
                Vector3 top_right = new Vector3(screen_width/2, screen_height/2, 0);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position, 
                top_right, 
                Color.cyan, 
                0.02f);
                
                Drawing.draw_vector_from_to_position((Vector3Int)screen_position + 
                top_right, 
                new Vector3((float)((screen_width - hpbar_width / 2) * -(hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * -(hpbar_y_offset_from_corner / 100f)), 
                0), 
                Color.gold, 
                0.02f);

                Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + 
                top_right + 
                new Vector3((float)((screen_width - hpbar_width / 2) * -(hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * -(hpbar_y_offset_from_corner / 100f)), 
                0), 
                hpbar_width, 
                hpbar_height,
                Color.gold, 
                0.02f);
            }
            else if (_dp_corner == corner_options.top_left) 
            {
                Vector3 top_left = new Vector3(-screen_width/2, screen_height/2, 0);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position, 
                new Vector3(-screen_width/2,
                screen_height/2, 0), 
                Color.cyan, 
                0.02f);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position + 
                top_left, 
                new Vector3((float)((screen_width - hpbar_width / 2) * (hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * -(hpbar_y_offset_from_corner / 100f)), 
                0), 
                Color.gold, 
                0.02f);

                Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + 
                top_left + 
                new Vector3((float)((screen_width - hpbar_width / 2) * (hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * -(hpbar_y_offset_from_corner / 100f)), 
                0), 
                hpbar_width, 
                hpbar_height,
                Color.gold, 
                0.02f);
            }
            else if (_dp_corner == corner_options.bottom_right) 
            {
                Vector3 bottom_right = new Vector3(screen_width/2, -screen_height/2, 0);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position, 
                new Vector3(screen_width/2, 
                -screen_height/2, 
                0), 
                Color.cyan, 
                0.02f);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position + 
                bottom_right,
                new Vector3((float)((screen_width - hpbar_width / 2) * -(hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * (hpbar_y_offset_from_corner / 100f)), 
                0), 
                Color.gold, 
                0.02f);

                Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + 
                bottom_right + 
                new Vector3((float)((screen_width - hpbar_width / 2) * -(hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * (hpbar_y_offset_from_corner / 100f)), 
                0), 
                hpbar_width, 
                hpbar_height,
                Color.gold, 
                0.02f);
            }
            else if (_dp_corner == corner_options.bottom_left) 
            {
                Vector3 bottom_left = new Vector3(-screen_width/2, -screen_height/2, 0);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position,
                new Vector3(-screen_width/2, 
                -screen_height/2,
                0), 
                Color.cyan, 
                0.02f);

                Drawing.draw_vector_from_to_position((Vector3Int)screen_position + 
                bottom_left, 
                new Vector3((float)((screen_width - hpbar_width / 2) * (hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * (hpbar_y_offset_from_corner / 100f)), 
                0), 
                Color.gold, 
                0.02f);

                Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + 
                bottom_left + 
                new Vector3((float)((screen_width - hpbar_width / 2) * (hpbar_x_offset_from_corner / 100f)), 
                (float)((screen_height - hpbar_height / 2) * (hpbar_y_offset_from_corner / 100f)), 
                0), 
                hpbar_width, 
                hpbar_height,
                Color.gold, 
                0.02f);
            }
            else 
            {
                Debug.Log("how did you even find a fifth option?!?");
            }
        } 
        else 
        {
            Debug.Log("no corner selected!");
        }
    }

    void DrawVectorToScreen()
    {
        Drawing.draw_vector_from_to_position(Vector3.zero, (Vector3Int)screen_position, Color.coral, 0.02f);
    }

    void DrawRectScreen()
    {
        Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position, screen_width, screen_height, Color.bisque, 0.02f);
    }

    void DrawPopup() {

        if (screen_position.x - screen_width / 2 < screen_position.x + popup_screen_center_offset.x - ((screen_width / 2) * popup_coverage_percentage_x / 100) &&
        screen_position.x + screen_width / 2 > screen_position.x + popup_screen_center_offset.x + ((screen_width / 2) * popup_coverage_percentage_x / 100) &&
        screen_position.y - screen_height / 2 < screen_position.y + popup_screen_center_offset.y - ((screen_height / 2) * popup_coverage_percentage_y / 100) &&
        screen_position.y + screen_height / 2 > screen_position.y + popup_screen_center_offset.y + ((screen_height / 2) * popup_coverage_percentage_y / 100)) 
        {

            Drawing.draw_vector_from_to_position((Vector3Int)screen_position, (Vector3Int)popup_screen_center_offset, Color.green, 0.02f);
            Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + (Vector3Int)popup_screen_center_offset, (screen_width * popup_coverage_percentage_x) / 100, (screen_height * popup_coverage_percentage_y) / 100, Color.green, 0.02f);

        }
        else 
        {

            Drawing.draw_vector_from_to_position((Vector3Int)screen_position, (Vector3Int)popup_screen_center_offset, Color.red, 0.02f);
            Drawing.draw_rectangle_from_center_position((Vector3Int)screen_position + (Vector3Int)popup_screen_center_offset, (screen_width * popup_coverage_percentage_x) / 100, (screen_height * popup_coverage_percentage_y) / 100, Color.red, 0.02f);

        }
    }
}
