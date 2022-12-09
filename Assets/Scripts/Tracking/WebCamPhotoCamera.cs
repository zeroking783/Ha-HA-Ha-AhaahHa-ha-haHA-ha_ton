using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

public class WebCamPhotoCamera : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public Bone[] bonesArray;
    private float timer = 0.5f;
    private ZeroMQInstance zmqInstance;
    
    void Start()
    {
        zmqInstance = new ZeroMQInstance(GetPoints);
        zmqInstance.Start();
        webCamTexture = new WebCamTexture();
        
        //Set resolurion to webCamTexture
        webCamTexture.requestedHeight = 500;
        webCamTexture.requestedWidth = 500;
        
        GetComponent<Renderer>().material.mainTexture =
            webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        webCamTexture.Play();
    }

    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.2f;
            
            
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();
            byte[] bytes = photo.EncodeToPNG();

            //отправить по сокету
            zmqInstance.Send(bytes);

        }
    }

    public void OnDisable()
    {
     
        zmqInstance.Stop();
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


    void GetPoints(string points)
    {
        //разделить строку по разделителю ;
        string[] pointsArray = points.Split(';');

        //разбить полученные строки по пробелам
        for(int i = 0; i < pointsArray.Length; i++)
        {
            string[] pointArray = pointsArray[i].Split(' ');
            if (pointArray.Length < 5) continue;
            // преобразовать в структуру
            if (i < bonesArray.Length)
            {
                bonesArray[i].id = int.Parse(pointArray[0]);
                bonesArray[i].position =
                    new Vector3(toFloat(pointArray[1]), -toFloat(pointArray[2]), 0) * 11;
                bonesArray[i].visibility = toFloat(pointArray[4]);
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
