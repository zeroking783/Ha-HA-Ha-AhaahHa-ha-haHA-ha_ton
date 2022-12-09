using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class ZeroMQInstance
{
    private Thread _thread;
    private byte[] _data = null;
    private bool _isRunning = false;
    private bool pythonRunning = false;
    
    private Process _process;
    
    public ZeroMQInstance(Action<string> callback)
    {
        //run_cmd();
        
        _thread =  new Thread(() =>
        {
            using (var socket = new RequestSocket())
            {

                while (!pythonRunning)
                {
                    socket.Connect("tcp://localhost:5555");

                    while (_isRunning)
                    {

                        if (_data != null)
                        {
                            socket.SendFrame(_data);
                            _data = null;
                            string message = socket.ReceiveFrameString();
                            callback(message);
                        }
                    }
                }
            }
        });
    }

    public void Start()
    {
        _isRunning = true;
        _thread.Start();
    }
    
    public void Stop()
    {
        _isRunning = false;
       _thread.Interrupt();
    }
    
    public void Send(byte[] data)
    {
        _data = data;
    }

     private void run_cmd()
        {

            string fileName = @"D:\Desktop\AlgoritmsTasks\Tracking\main.py";

            _process = new Process();
            _process.StartInfo = new ProcessStartInfo(@"d:\Desktop\AlgoritmsTasks\Tracking\Scripts\python.exe", fileName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            _process.Start();
            
            pythonRunning = true;
            
            
        }
}
