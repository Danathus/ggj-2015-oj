using UnityEngine;

public class Bounder
{
	public Bounder (Renderer renderer)
	{
		_renderer = renderer;
	}

	public Vector3 Translate(Vector3 pos, Vector3 dir)
	{
		Bounds bounds = _renderer.bounds;
		Vector3 point = pos + dir;
		if (bounds.Contains (point)) {
			return dir;
		}
		point.x = Mathf.Clamp (point.x, bounds.min.x, bounds.max.x);
		point.y = Mathf.Clamp (point.y, bounds.min.y, bounds.max.y);
		point.z = Mathf.Clamp (point.z, bounds.min.z, bounds.max.z);

		return point - pos;
	}

	private Renderer _renderer;
}
