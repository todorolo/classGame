using UnityEngine;

public class ProceduralWalk : MonoBehaviour
{   // Target objects for the foot to look for + the animation curve the target will follow
    public Transform backLeftFootTarget;
    public Transform backRightFootTarget;
    public Transform frontLeftFootTarget;
    public Transform frontRightFootTarget;
    public AnimationCurve backHorizontalCurve;
    public AnimationCurve frontHorizontalCurve;
    public AnimationCurve backVerticalCurve;
    public AnimationCurve frontVerticalCurve;
    // Checkpoint for step positions
    private Vector3 backLeftTargetOffset;
    private Vector3 backRightTargetOffset;
    private Vector3 frontLeftTargetOffset;
    private Vector3 frontRightTargetOffset;
    // Need these in order for the ground snapping to not overwrite picking up the foot for a step
    private float backLeftLegLast = 0;
    private float backRightLegLast = 0;
    private float frontLeftLegLast = 0;
    private float frontRightLegLast = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Remembers where foot just ended
        backLeftTargetOffset = backLeftFootTarget.localPosition;
        backRightTargetOffset = backRightFootTarget.localPosition;
        frontLeftTargetOffset = frontLeftFootTarget.localPosition;
        frontRightTargetOffset = frontRightFootTarget.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        //// Foot movement, the numbers are specifically for quadropedal movement, use 0 and 1 for bipedal movement with 0.5 difference between them for vertical
        float backLeftLegForwardMovement = backHorizontalCurve.Evaluate(Time.time - 0.4f);
        float backRightLegForwardMovement = backHorizontalCurve.Evaluate(Time.time - 0.95f);
        float frontLeftLegForwardMovement = frontHorizontalCurve.Evaluate(Time.time - 0.25f);
        float frontRightLegForwardMovement = frontHorizontalCurve.Evaluate(Time.time -0.7f);

        backLeftFootTarget.localPosition = backLeftTargetOffset + 
            this.transform.InverseTransformVector(backLeftFootTarget.forward) * backLeftLegForwardMovement +
            this.transform.InverseTransformVector(backLeftFootTarget.up) * backVerticalCurve.Evaluate(Time.time + 0.1f);
        backRightFootTarget.localPosition = backRightTargetOffset + 
            this.transform.InverseTransformVector(backRightFootTarget.forward) * backRightLegForwardMovement +
            this.transform.InverseTransformVector(backRightFootTarget.up) * backVerticalCurve.Evaluate(Time.time - 0.45f);
        frontLeftFootTarget.localPosition = frontLeftTargetOffset + 
            this.transform.InverseTransformVector(frontLeftFootTarget.forward) * frontLeftLegForwardMovement +
            this.transform.InverseTransformVector(frontLeftFootTarget.up) * frontVerticalCurve.Evaluate(Time.time + 0.25f);
        frontRightFootTarget.localPosition = frontRightTargetOffset + 
            this.transform.InverseTransformVector(frontRightFootTarget.forward) * frontRightLegForwardMovement +
            this.transform.InverseTransformVector(frontRightFootTarget.up) * frontVerticalCurve.Evaluate(Time.time - 0.2f);

        float backLeftLegDirection = backLeftLegForwardMovement - backLeftLegLast;
        float backRightLegDirection = backRightLegForwardMovement - backRightLegLast;
        float frontLeftLegDirection = frontLeftLegForwardMovement - frontLeftLegLast;
        float frontRightLegDirection = frontRightLegForwardMovement - frontRightLegLast;

        // Checks for ground and snaps foot to it
        RaycastHit hit;
        if(backLeftLegDirection < 0 &&
            Physics.Raycast(backLeftFootTarget.position + backLeftFootTarget.up, -backLeftFootTarget.up, out hit, Mathf.Infinity))
        {
            backLeftFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(backLeftLegDirection);
        }
        if (backRightLegDirection < 0 &&
            Physics.Raycast(backRightFootTarget.position + backRightFootTarget.up, -backRightFootTarget.up, out hit, Mathf.Infinity))
        {
            backRightFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(backRightLegDirection);
        }
        if (frontLeftLegDirection < 0 &&
            Physics.Raycast(frontLeftFootTarget.position + frontLeftFootTarget.up, -frontLeftFootTarget.up, out hit, Mathf.Infinity))
        {
            frontLeftFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(frontLeftLegDirection);
        }
        if (frontRightLegDirection < 0 &&
            Physics.Raycast(frontRightFootTarget.position + frontRightFootTarget.up, -frontRightFootTarget.up, out hit, Mathf.Infinity))
        {
            frontRightFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(frontRightLegDirection);
        }

        backLeftLegLast = backLeftLegForwardMovement;
        backRightLegLast = backRightLegForwardMovement;
        frontLeftLegLast = frontLeftLegForwardMovement;
        frontRightLegLast = frontRightLegForwardMovement;
    }
}
