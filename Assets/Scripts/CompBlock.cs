using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CompBlock : MonoBehaviour
{
	public const float AABB_DIMENSIONS_F = 1f;

	public float FrustumScreenVerticalPosNormalized { get; set; }
}
