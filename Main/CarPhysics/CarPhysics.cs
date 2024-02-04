using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class CarPhysics : MonoBehaviour
{
    public Rigidbody carRB;

    public float maxDist;

    public float springStrenght;
    public float suspensionRestDist;
    public float dumping;

    public float carTopSpeed;
    public float tireBreakFactor;

    public float tireGripFactor;
    public float tireMass;

    [Header("CarSpecs:")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    public float ackermannAngleLeft;
    public float ackermannAngleRight;

    public Vector2 input;

    public AnimationCurve powerCurve;

    [SerializeField] Transform LBWheel;
    [SerializeField] Transform LFWheel;

    [SerializeField] Transform RBWheel;
    [SerializeField] Transform RFWheel;

    RaycastHit hitLB;
    RaycastHit hitRB;
    RaycastHit hitLF;
    RaycastHit hitRF;

    void LateUpdate()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        WheelRotation();

        LFWheel.localRotation = Quaternion.Euler(ackermannAngleLeft * Vector3.up);
        RFWheel.localRotation = Quaternion.Euler(ackermannAngleRight * Vector3.up);

        Ray rayLB = new Ray(LBWheel.position, -LBWheel.transform.up);
        if(Physics.Raycast(rayLB, out hitLB, maxDist))
        {
            CalculateSpring(hitLB, LBWheel);
            WheelDrive(LBWheel);
            WheelSteer(LBWheel);
        }

        Ray rayLF = new Ray(LFWheel.position, -LFWheel.transform.up);
        if(Physics.Raycast(rayLF, out hitLF, maxDist))
        {
            CalculateSpring(hitLF, LFWheel);
            WheelDrive(LFWheel);
            WheelSteer(LFWheel);

        }

        Ray rayRB = new Ray(RBWheel.position, -RBWheel.transform.up);
        if(Physics.Raycast(rayRB, out hitRB, maxDist))
        {
            CalculateSpring(hitRB, RBWheel);
            WheelDrive(RBWheel);
            WheelSteer(RBWheel);
        }

        Ray rayRF = new Ray(RFWheel.position, -RFWheel.transform.up);
        if(Physics.Raycast(rayRF, out hitRF, maxDist))
        {
            CalculateSpring(hitRF, RFWheel);
            WheelDrive(RFWheel);
            WheelSteer(RFWheel);
        }
    }

    private void WheelRotation()
    {
        if(input.x > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + rearTrack / 2) * input.x);
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - rearTrack / 2) * input.x);
        }
        else if (input.x < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - rearTrack / 2) * input.x);
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + rearTrack / 2) * input.x);
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }
    }

    private void CalculateSpring(RaycastHit raycastHit, Transform wheel)
    {
        Vector3 springDir = wheel.up;

        Vector3 tireWorldVel = carRB.GetPointVelocity(wheel.position);

        float offset = suspensionRestDist - raycastHit.distance;

        float vel = Vector3.Dot(springDir, tireWorldVel);

        float force = (offset * springStrenght) - (vel * dumping);

        carRB.AddForceAtPosition(force * springDir, wheel.position);
    }

    private void WheelSteer(Transform tire)
    {
        Vector3 steeringDir = tire.right;

        Vector3 tireWorldVel = carRB.GetPointVelocity(tire.transform.position);

        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

        float desiredVelChange = -steeringVel * tireGripFactor;

        float desiredAccel = desiredVelChange /Time.fixedDeltaTime;

        carRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel,tire.position);
    }

    private void WheelDrive(Transform wheel)
    {
        if (input.y > 0)
        {
            Vector3 accelDir = wheel.forward;

            float carSpeed = Vector3.Dot(carRB.transform.forward, carRB.velocity);

            float normalized = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

            float torque = powerCurve.Evaluate(normalized) * input.y;

            carRB.AddForceAtPosition(accelDir * torque, wheel.position);
        }
        else if(input.y < 0)
        {
            Vector3 steeringDir = wheel.forward;

            Vector3 tireWorldVel = carRB.GetPointVelocity(wheel.transform.position);

            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

            float desiredVelChange = -steeringVel * tireBreakFactor;

            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            carRB.AddForceAtPosition(steeringDir * tireMass * desiredAccel, wheel.position);
        }
    }


}
