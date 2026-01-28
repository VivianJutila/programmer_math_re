using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;

public class easing_animations : MonoBehaviour
{
	[SerializeField]
	private UnityEngine.UI.Button title_animation_play_button;

	[SerializeField]
	private UnityEngine.UI.Button inventory_animation_play_button;

	[SerializeField]
	private UnityEngine.UI.Button progress_bar_animation_play_button;

	[SerializeField]
	private UnityEngine.UI.Image title_image;

	[SerializeField]
	private UnityEngine.UI.Image progress_bar_image;

	[SerializeField]
	private UnityEngine.UI.Image inventory_image;

	private EasingFunction.Ease inventory_ease = EasingFunction.Ease.EaseOutQuart;
	private EasingFunction.Ease progress_bar_ease = EasingFunction.Ease.EaseInSine;
	private EasingFunction.Ease title_ease = EasingFunction.Ease.EaseOutBounce;

	private EasingFunction.Function easing_functioner;

	private bool inv_reverse = true;
	private bool title_reverse = true;
	private bool pb_reverse = true;

	private float inv_interpolation_value = 0f;
	private float title_interpolation_value = 0f;
	private float pb_interpolation_value = 0f;

	private Vector3 title_animation_starting_vector;
	private Vector3 title_animation_end_vector;
	private Vector3 inventory_animation_starting_vector;
	private Vector3 inventory_animation_end_vector;
	private Vector3 pb_animation_starting_vector;
	private Vector3 pb_animation_end_vector;


	public void Start()
	{
		title_image.transform.localPosition += new Vector3(0, 200, 0);
		title_animation_starting_vector = title_image.transform.localPosition;
		title_animation_end_vector = title_animation_starting_vector + new Vector3(0, -200, 0);
		inventory_image.transform.localPosition += new Vector3(-800, 0, 0);
		inventory_animation_starting_vector = inventory_image.transform.localPosition;
		inventory_animation_end_vector = inventory_animation_starting_vector + new Vector3(800, 0, 0);

		title_animation_play_button.onClick.AddListener(TitleDropAnimation);

		progress_bar_animation_play_button.onClick.AddListener(ProgressBarAnimation);

		inventory_animation_play_button.onClick.AddListener(InventorySlideAnimation);
	}

	public void Update()
	{
		if (inv_reverse && inv_interpolation_value > 0f)
		{
			inv_interpolation_value -= Time.deltaTime * 2f;
		} 
		else if (!inv_reverse  && inv_interpolation_value < 1f)
		{
			inv_interpolation_value += Time.deltaTime * 2f;
		}

		if (title_reverse && title_interpolation_value > 0f)
		{
			title_interpolation_value -= Time.deltaTime / 4f;
		}
		else if (!title_reverse && title_interpolation_value < 1f)
		{
			title_interpolation_value += Time.deltaTime / 4f;
		}

		if (pb_reverse && pb_interpolation_value > 0f)
		{
			pb_interpolation_value -= Time.deltaTime;
		}
		else if (!pb_reverse && pb_interpolation_value < 1f)
		{
			pb_interpolation_value += Time.deltaTime;
		}

		if (easing_functioner != null)
		{
			title_image.transform.localPosition = ReturnInterpolationPoint(title_animation_starting_vector, title_animation_end_vector, easing_functioner(0f, 1f, title_interpolation_value));

			inventory_image.transform.localPosition = ReturnInterpolationPoint(inventory_animation_starting_vector, inventory_animation_end_vector, easing_functioner(0f, 1f, inv_interpolation_value));
		}
	}

	void TitleDropAnimation()
	{
		easing_functioner = EasingFunction.GetEasingFunction(title_ease);

		title_reverse = !title_reverse;
	}

	void ProgressBarAnimation()
	{
		easing_functioner = EasingFunction.GetEasingFunction(progress_bar_ease);


	}

	void InventorySlideAnimation()
	{
		easing_functioner = EasingFunction.GetEasingFunction(inventory_ease);

		inv_reverse = !inv_reverse;
	}

	Vector3 ReturnInterpolationPoint(Vector3 _point_a, Vector3 _point_b, float _interpolation_value)
	{
		Mathf.Clamp(_interpolation_value, 0f, 1f);
		return (1 - _interpolation_value) * _point_a + _interpolation_value * _point_b;
	}
}
