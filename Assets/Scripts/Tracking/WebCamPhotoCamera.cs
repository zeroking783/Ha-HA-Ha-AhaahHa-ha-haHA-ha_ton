 using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WebCamPhotoCamera : MonoBehaviour
{
    public Renderer PlayerScreen;
    WebCamTexture webCamTexture;
    public RenderTexture videoTexture;
    [FormerlySerializedAs("bonesArray")] public Bone[] bonesArrayWebcam;
    public Bone[] bonesArrayVideo;
    private float timer = 0.5f;
    private bool flazho4ek = false;
    private ZeroMQInstance zmqInstanceWebcam;
    private ZeroMQInstance zmqInstanceVideo;
    
    void Start()
    {
        zmqInstanceWebcam = new ZeroMQInstance(GetPointsWebcam, 5555);
        zmqInstanceWebcam.Start();
        zmqInstanceVideo = new ZeroMQInstance(GetPointsVideo, 5556);
        zmqInstanceVideo.Start();
        webCamTexture = new WebCamTexture();
        
        //Set resolurion to webCamTexture
        webCamTexture.requestedHeight = 500;
        webCamTexture.requestedWidth = 500;
        //Set resolution to videoTexture
        
        PlayerScreen.material.mainTexture =
            webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        webCamTexture.Play();
    }

    public void p(string f) => f += '1';
    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.1f;

            if (flazho4ek)
            {
                //получить с вебкамеры байты
                Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
                photo.SetPixels(webCamTexture.GetPixels());
                photo.Apply();
                byte[] bytes = photo.EncodeToPNG();
    
                //отправить по сокету
                zmqInstanceWebcam.Send(bytes);
            }
            else
            {
                //преобразовать videoteхture в texture2d
                Texture2D ph = new Texture2D(videoTexture.width, videoTexture.height);

                RenderTexture rend = new RenderTexture(videoTexture.width, videoTexture.height, 24);
                Graphics.Blit(videoTexture, rend);
                ph.ReadPixels(new Rect(0, 0, rend.width, rend.height), 0, 0);
                ph.Apply();
                byte[] bytes = ph.EncodeToPNG();
                zmqInstanceVideo.Send(bytes);
            }

            flazho4ek = !flazho4ek;

        }
    }

    public void OnDisable()
    {
     
        zmqInstanceWebcam.Stop();
        zmqInstanceVideo.Stop();
        print("QUIT");
    }
    // async Task<bool> ToPhoto()
    // {
    //     Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
    //     photo.SetPixels(webCamTexture.GetPixels());
    //     photo.Apply();
    //
    //     //Encode to a PNG АСИНХРОННО БЛЯТЬ
    //     // умри в канаве
    //         
    //         
    //     byte[] bytes = photo.EncodeToPNG();
    //     
    //     //отправить по сокету
    //     zmqInstance.Send(bytes);
    //     return true;
    // }


    void GetPointsWebcam(string points)
    {
        print("PLAYER");
        //разделить строку по разделителю ;
        string[] pointsArray = points.Split(';');


        //разбить полученные строки по пробелам
        for(int i = 0; i < pointsArray.Length; i++)
        {
            string[] pointArray = pointsArray[i].Split(' ');
            if (pointArray.Length < 5) continue;
            // преобразовать в структуру
            if (i < bonesArrayWebcam.Length)
            {
                bonesArrayWebcam[i].id = int.Parse(pointArray[0]);
                bonesArrayWebcam[i].position =
                    new Vector3(toFloat(pointArray[1]), -toFloat(pointArray[2]), 0) * 11;
                bonesArrayWebcam[i].visibility = toFloat(pointArray[4]);
            }
        }
    }
    
    void GetPointsVideo(string points)
    {
        print("VIDEO");
        //разделить строку по разделителю ;
        string[] pointsArray = points.Split(';');

        //разбить полученные строки по пробелам
        for(int i = 0; i < pointsArray.Length; i++)
        {
            string[] pointArray = pointsArray[i].Split(' ');
            if (pointArray.Length < 5) continue;
            // преобразовать в структуру
            if (i < bonesArrayVideo.Length)
            {
                bonesArrayVideo[i].id = int.Parse(pointArray[0]);
                bonesArrayVideo[i].position =
                    new Vector3(toFloat(pointArray[1]), -toFloat(pointArray[2]), 0) * 11;
                bonesArrayVideo[i].visibility = toFloat(pointArray[4]);
            }
        }
    }

    float toFloat(string s)
    {
        float f = 0f;
        // найти индекс элемента "."
        int index = s.IndexOf('.');
        // срез с 0 до индекса

        string s1 = s.Substring(0, index);


        f += int.Parse(s1);
        for (int i = index + 1; i < s.Length; i++)
        {
            f += int.Parse(s[i].ToString()) * Mathf.Pow(10, -i);
        }

        return f;
    }
}
