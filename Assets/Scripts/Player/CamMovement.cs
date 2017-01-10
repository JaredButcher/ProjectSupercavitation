using UnityEngine;
using System.Collections;

public class CamMovement : MonoBehaviour {
    public Camera Camera;
    public GameObject Icon;

    float Width;
    float MovementDelta;
    Vector3 DeltaLocation;
    GameLevelManager Manager;
    bool Moveing;
    Vector3 Target;
    float Timer;
    float TimerSet = .5f;
	
	void Update () {
        if (!Manager) {
            Manager = FindObjectOfType<GameLevelManager>();
        }
        if (Moveing) {
            if(Timer > 0) {
                Timer -= Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, Target, 1 - Timer / TimerSet);
            } else {
                transform.position = Target;
                Moveing = false;
            }
        } else {
            DeltaLocation = new Vector3(0, 0, 0);
            Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView + Input.GetAxis("Mouse ScrollWheel") * 50, 10, 170);
            Manager.SetUIScale(Mathf.Exp(Camera.fieldOfView * .03f) * .03f);
            Width = Mathf.Abs(Mathf.Tan(Mathf.Deg2Rad * Camera.fieldOfView / 2)) * 200;
            Icon.transform.localScale = new Vector3(Camera.aspect * Width, 0, Width);
            MovementDelta = Mathf.Pow(Camera.fieldOfView, 1.1f);
            if (Input.GetKey(KeyCode.W)) {
                DeltaLocation.z = MovementDelta;
            }
            if (Input.GetKey(KeyCode.S)) {
                DeltaLocation.z = DeltaLocation.z - MovementDelta;
            }
            if (Input.GetKey(KeyCode.D)) {
                DeltaLocation.x = MovementDelta;
            }
            if (Input.GetKey(KeyCode.A)) {
                DeltaLocation.x = DeltaLocation.x - MovementDelta;
            }
            DeltaLocation = transform.position + DeltaLocation;
            DeltaLocation.x = Mathf.Clamp(DeltaLocation.x, -1 * (Manager.MapSize / 2), Manager.MapSize / 2);
            DeltaLocation.z = Mathf.Clamp(DeltaLocation.z, -1 * (Manager.MapSize / 2), Manager.MapSize / 2);
            transform.position = DeltaLocation;
        }
    }

    public void MoveTo(Vector3 _Location) {
        Moveing = true;
        Target = new Vector3(_Location.x,1000,_Location.z);
        Timer = TimerSet;
    }
}
