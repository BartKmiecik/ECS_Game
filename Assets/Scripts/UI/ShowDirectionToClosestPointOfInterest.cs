using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDirectionToClosestPointOfInterest : MonoBehaviour
{
    [SerializeField]private List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
    private Camera cam;
    private int frameUpdateDeley = 2;
    private int currentDeley = 0;
    public RectTransform indicationArrow;
    public GameObject test;
    public float offset = 100;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        currentDeley += 1;
        if (currentDeley > frameUpdateDeley)
        {
            currentDeley -= frameUpdateDeley;
            Vector3 direction = pointsOfInterest[0].transform.position - cam.transform.position;
            Vector3 targetPosScreenPoint = cam.WorldToScreenPoint(direction.normalized * 100);
            bool isOffscreen = targetPosScreenPoint.x <= 0 || targetPosScreenPoint.x >= Screen.width ||
                targetPosScreenPoint.y <= 0 || targetPosScreenPoint.y >= Screen.height;


            float angle = Mathf.Atan2(direction.z, direction.x);
            test.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg *angle);
            //test.transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
            if (isOffscreen)
            {
                indicationArrow.gameObject.SetActive(true);
                Vector3 cappedTargetScreenPosition = targetPosScreenPoint;
                if (cappedTargetScreenPosition.x <= offset) cappedTargetScreenPosition.x = offset;
                if (cappedTargetScreenPosition.x >= Screen.width- offset) cappedTargetScreenPosition.x = Screen.width- offset;
                if (cappedTargetScreenPosition.y <= offset) cappedTargetScreenPosition.y = offset;
                if (cappedTargetScreenPosition.y >= Screen.height- offset) cappedTargetScreenPosition.y = Screen.height - offset;
                cappedTargetScreenPosition.z = 0;
                indicationArrow.position = cappedTargetScreenPosition;
                
                //indicationArrow.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle);
                //Vector3 pointerWorldPostion = cam.ScreenToWorldPoint(cappedTargetScreenPosition);
                //pointerWorldPostion.z = 0;
                //indicationArrow.localPosition = pointerWorldPostion;
            }
            else
            {
                indicationArrow.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("direction " + direction.normalized*100);
                Debug.Log("CamPos: " + cam.transform.position);
                Debug.Log("targetPosScreenPoint" + targetPosScreenPoint);
                Debug.Log("Indication " + indicationArrow.position);
            }
        }
    }

}
