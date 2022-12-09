using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WebCamPhotoCamera))]
public class PointsCalculator : MonoBehaviour
{
    private WebCamPhotoCamera webCamPhotoCamera;
    private float pointCalculationTimer = 1f;
    [SerializeField] private float acceptDistance = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        webCamPhotoCamera = GetComponent<WebCamPhotoCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        pointCalculationTimer -= Time.deltaTime;
        if (pointCalculationTimer <= 0)
        {
            pointCalculationTimer = 1f;
            int correntPoints = 0;
            for (int i = 0; i < webCamPhotoCamera.bonesArrayVideo.Length; i++)
            {
                Bone webcamBone = webCamPhotoCamera.bonesArrayWebcam[i];
                Bone videoBone = webCamPhotoCamera.bonesArrayVideo[i];
                if (Vector3.Distance(webcamBone.transform.position, videoBone.transform.position) < acceptDistance)
                {
                    correntPoints++;
                }
            }
            float correntPercent = (float) correntPoints / webCamPhotoCamera.bonesArrayVideo.Length * 100;
            Debug.Log(correntPercent);
            //какать здесь
        }   
    }
}
