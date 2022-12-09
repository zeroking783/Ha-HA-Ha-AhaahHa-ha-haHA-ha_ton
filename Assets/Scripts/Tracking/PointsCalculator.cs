using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WebCamPhotoCamera))]
public class PointsCalculator : MonoBehaviour
{
    private WebCamPhotoCamera webCamPhotoCamera;
    private float pointCalculationTimer = 1f;
    [SerializeField] private float acceptDistance = 0.1f;
    [SerializeField] private float successRate = 0.5f;
    public float life = 1f;
    [SerializeField] private Image kaifometr;
    
    
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
            float correntPercent = (float) correntPoints / webCamPhotoCamera.bonesArrayVideo.Length;
            Debug.Log(correntPercent);
            if (correntPercent >= successRate)
            {
                life += 0.05f;
            }
            else
            {
                life -= 0.05f;
            }
            kaifometr.fillAmount = life;
        }   
    }
}
