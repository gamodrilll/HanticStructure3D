using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;

public class Server : MonoBehaviour {

    public static string msg = "";

    public static Compound compToVis = null;

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
            Debug.Log(l);
            if (l != 0)
            {
                byte[] buf = new byte[l];
                s.Read(buf, 0, l);
                msg = ByteArrayToString(buf);
            }
            else
            {
                using ( var st = File.OpenRead(@"C:\DIP\Crystal3D\WindowsFormsApplication1\bin\Debug\compound.xml"))
                {
                    Debug.Log("st");
                    XmlSerializer ser = new XmlSerializer(typeof(Compound));
                    compToVis = (Compound)(ser.Deserialize(st));
                    Debug.Log("f");
                    st.Close();
                }
                File.Delete(@"C:\DIP\Crystal3D\WindowsFormsApplication1\bin\Debug\compound.xml");
            }
            s.Close();
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

public sealed class ClassBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
        {
            Type typeToDeserialize = null;
            assemblyName = Assembly.GetExecutingAssembly().FullName;
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
            return typeToDeserialize;
        }
        return null;
    }
}