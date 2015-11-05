using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPThread: MonoBehaviour {
	
	public string multicastIP = "239.1.2.3"; // typical for MOVES
	public int multicastPort = 62040;        // typical for MOVES
	public event Action<object, byte[]> eventDataReceived;
	
    Thread receiveThread;
	List<byte[]> dataList;	
	
	static Mutex dataMutex = new Mutex();
	
	#region MonoBehaviour Methods
    // -------------------------------------------------------------------------
    void Start()
    {
       Init(); 
    }
	
	// -------------------------------------------------------------------------
	void Update() {
		
		dataMutex.WaitOne();
		
		foreach(byte[] data in dataList) {
			
			if (eventDataReceived != null) {
				eventDataReceived(this, data);	
			}
		}
		
		dataList.Clear();
		
		dataMutex.ReleaseMutex();
	}
	#endregion
	
	#region Private Methods
    // -------------------------------------------------------------------------
    void Init()
    {		
		dataList = new List<byte[]>();
		
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));

        receiveThread.IsBackground = true;
        receiveThread.Start();

        print("Start"); 
    }

 // -------------------------------------------------------------------------
    void ReceiveData() 
    {			
		IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, multicastPort);
		EndPoint wtf = (EndPoint) anyIP;
		
		try {
			
			Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			udpSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.AddMembership,
    			new MulticastOption(IPAddress.Parse(multicastIP)));
			
			udpSocket.Bind(anyIP);	
		
			while (Thread.CurrentThread.IsAlive) 
			{
				if (udpSocket.Poll(1, SelectMode.SelectRead)) {
					
					dataMutex.WaitOne();
					
					byte[] buffer = new byte[1500];			
					int trueSize = udpSocket.ReceiveFrom(buffer, ref wtf);	
					
					byte[] trimmedBuffer = new byte[trueSize];
					Array.Copy(buffer, trimmedBuffer, trueSize);
					dataList.Add(trimmedBuffer);
					
					dataMutex.ReleaseMutex();
				}				
			 	
				Thread.Sleep(10);
			}
		
		} catch(Exception e) {
			print(e.ToString());
		}        
    } 

    // -------------------------------------------------------------------------
    void OnApplicationQuit()
    {
       // end of application
       if (receiveThread != null)
       { 
          receiveThread.Abort();
		  receiveThread.Join();
       }

       print("Stop"); 
    }
	#endregion
}