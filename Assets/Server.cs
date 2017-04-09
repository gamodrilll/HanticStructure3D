using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine;
using System.Net;

public class Server : MonoBehaviour {

    public static string msg = "";

    string ByteArrayToString(byte[] val)
    {
        string b = "";
        int len = val.Length;
        for (int i = 0; i < len; i++)
        {
            b += (char)val[i];
        }
        return b;
    }

    public void ReceiveCallback(IAsyncResult AsyncCall)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();


        Socket listener = (Socket)AsyncCall.AsyncState;
        Socket client = listener.EndAccept(AsyncCall);
        using (NetworkStream s = new NetworkStream(client))
        {
            int l = s.ReadByte();

            byte[] buf = new byte[l];
            s.Read(buf, 0, l);
            msg = ByteArrayToString(buf);

        }

        client.Close();

        // После того как завершили соединение, говорим ОС что мы готовы принять новое
        listener.BeginAccept(new AsyncCallback(ReceiveCallback), listener);
    }


    // Use this for initialization
    void Start () {
        IPAddress localAddress = IPAddress.Parse("127.0.0.1");

        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint ipEndpoint = new IPEndPoint(localAddress, 2200);

        listenSocket.Bind(ipEndpoint);

        listenSocket.Listen(1);
        listenSocket.BeginAccept(new AsyncCallback(ReceiveCallback), listenSocket);
        msg = "Server started";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
