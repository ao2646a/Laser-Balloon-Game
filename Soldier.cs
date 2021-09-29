using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the soldier that shoots the balloons.
/// A laser is cast from the LaserStart object to where the mouse position on the screen is when the left mouse button is pressed.
/// Use a RayCast to check if the laser hits any balloons.
/// </summary>
public class Soldier : MonoBehaviour {

	[Tooltip("GameObject at the position where the laser starts")]
	public GameObject laserStart;
	[Tooltip("Crosshair GameObject")]
	public GameObject crosshair;
	[Tooltip("LineRenderer of the laser graphics")]
	public LineRenderer laserLineRenderer;
	[Tooltip("The layer of the balloons, for easy raycasting only against balloons")]
	public LayerMask balloonLayerMask;

	void Update () {
		// use GetMouseWorldPosition() and UpdateCrosshair() to make the chrosshair move with the mouse
		//crosshairposition = mousePosition
	Vector3 facing = crosshair.transform.position - transform.position;
	UpdateCrosshair(GetMouseWorldPosition());

		if (facing.x !=0 || facing.y != 0)
        {
			facing.Normalize();
			float angle = Mathf.Atan2(facing.y, facing.x);
			float offset = 90;
			transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - offset, Vector3.forward);
        }

		if (Input.GetMouseButton(0))
        {
            laserLineRenderer.enabled = true;
			Vector2 origin = laserStart.transform.position;
			Vector2 target = crosshair.transform.position;
			Vector2 direction = target - origin;
			RaycastHit2D hit = Physics2D.Raycast(origin, direction, direction.magnitude);
			if (hit.collider != null)
			{
				hit.collider.gameObject.GetComponent<Balloon>().Pop();
			}
		}
        else
        {
			laserLineRenderer.enabled = false;
        }
		
	}

    /// <summary>
    /// Grabs the world position of the mouse with z = 0.
    /// </summary>
    /// <returns>World position of mouse as Vector3</returns>
    Vector3 GetMouseWorldPosition() {
        // this gets the current mouse position (in screen coordinates) and transforms it into world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // the camera is on z = -10, so all screen coordinates are on z = -10. To be on the same plane as the game, we need to set z to 0
        mouseWorldPos.z = 0;

        return mouseWorldPos;
    }

	/// <summary>
	/// Updates the crosshair position and the line renderer from the laser to point from laserStart to the crosshair
	/// </summary>
	void UpdateCrosshair(Vector3 newCrosshairPosition){
		crosshair.transform.position = newCrosshairPosition;
		laserLineRenderer.SetPosition (0, laserStart.transform.position);
		laserLineRenderer.SetPosition (1, crosshair.transform.position);
	}
}
