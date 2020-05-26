using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{

    int port = 7777;
    int maxConnections = 10;

    // The id we use to identify our messages and register the handler
    short messageID = 1000;// первый клиент 
    short messageID3 = 1003; //второй клиент
    short messageID2 = 1001;//рассылка клиентам
    short messageID4 = 1004;//рассылка клиентам

    public float[] data0 = new float[9];
    public float[] data2 = new float[5];//данные второго клиента
    public float[] dataAns = new float[14];//данные для рассылки клиентам

    // Use this for initialization
    void Start()
    {
        // Usually the server doesn't need to draw anything on the screen
        Application.runInBackground = true;
        CreateServer();
    }

    void FixedUpdate()
    {

        //NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        //NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);

        // Our message use his own message type.
        NetworkServer.RegisterHandler(messageID, OnMessageReceived);
        NetworkServer.RegisterHandler(messageID3, OnMessageReceived);

        NetworkServer.RegisterHandler(messageID3, OnMessageReceived2);

        


        dataAns[0]= data0[0];//id
        //dataAns[1] = data0[1];
        dataAns[2] = data0[2];
        dataAns[3] = data0[3];
        dataAns[4] = data0[4];
        dataAns[5] = data2[0];
        //dataAns[6] = data2[1];
        dataAns[7] = data2[2];
        dataAns[8] = data2[3];
        dataAns[9] = data2[4];
        dataAns[10] = data0[5];
        dataAns[11] = data0[6];
        dataAns[12] = data0[7];
        dataAns[13] = data0[8];

        NetworkServer.RegisterHandler(messageID2, OnMessageReceived3);
        NetworkServer.RegisterHandler(messageID4, OnMessageReceived4);
    }

    void CreateServer()
    {
        // Register handlers for the types of messages we can receive
        RegisterHandlers();

        var config = new ConnectionConfig();
        // There are different types of channels you can use, check the official documentation
        config.AddChannel(QosType.ReliableFragmented);
        config.AddChannel(QosType.UnreliableFragmented);

        var ht = new HostTopology(config, maxConnections);

        if (!NetworkServer.Configure(ht))
        {
            Debug.Log("No server created, error on the configuration definition");
            return;
        }
        else
        {
            // Start listening on the defined port
            if (NetworkServer.Listen(port))
                Debug.Log("Server created, listening on port: " + port);
            else
                Debug.Log("No server created, could not listen to the port: " + port);
        }
    }

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }

    private void RegisterHandlers()
    {
        // Unity have different Messages types defined in MsgType
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);

        // Our message use his own message type.
        NetworkServer.RegisterHandler(messageID, OnMessageReceived);

        
        
    }

    private void RegisterHandler(short t, NetworkMessageDelegate handler)
    {
        NetworkServer.RegisterHandler(t, handler);
    }

    void OnClientConnected(NetworkMessage netMessage)
    {
        // Do stuff when a client connects to this server

        // Send a thank you message to the client that just connected
        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "Thanks for joining!";

        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID, messageContainer);

        // Send a message to all the clients connected
        messageContainer = new MyNetworkMessage();
        messageContainer.message = "A new player has conencted to the server";

        // Broadcast a message a to everyone connected
        NetworkServer.SendToAll(messageID, messageContainer);


        messageContainer = new MyNetworkMessage();
        messageContainer.message = "От сервера Клинету";
        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID, messageContainer);


    }

    void OnClientDisconnected(NetworkMessage netMessage)
    {
        // Do stuff when a client dissconnects
    }

    void OnMessageReceived(NetworkMessage netMessage)
    {
        // You can send any object that inherence from MessageBase
        // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
        // The first thing we do is deserialize the message to our custom type
        var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();
        data0 = objectMessage.data;
        Debug.Log("Message received: " + objectMessage.data);

        // data2 = objectMessage.data2;

        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "сообщение";
        messageContainer.dataAns = dataAns;
        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID2, messageContainer);

        
        

    }


    void OnMessageReceived2(NetworkMessage netMessage)
    {
        // You can send any object that inherence from MessageBase
        // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
        // The first thing we do is deserialize the message to our custom type
        var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();
        data2 = objectMessage.data;
        Debug.Log("Message received: " + objectMessage.data);


        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "сообщение";
        messageContainer.dataAns = dataAns;
        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID2, messageContainer);
        //MyNetworkMessage messageContainer = new MyNetworkMessage();
        //messageContainer.message = "сообщение";
        //// This sends a message to a specific client, using the connectionId
        //NetworkServer.SendToClient(netMessage.conn.connectionId, messageID2, messageContainer);
    }


    void OnMessageReceived3(NetworkMessage netMessage)//рассылка клиентам
    {
        
        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.dataAns = dataAns;
        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID2, messageContainer);
    }

    void OnMessageReceived4(NetworkMessage netMessage)//рассылка клиентам
    {

        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.dataAns = dataAns;
        // This sends a message to a specific client, using the connectionId
        NetworkServer.SendToClient(netMessage.conn.connectionId, messageID4, messageContainer);
    }


    void OnGUI()
    {


        GUI.Label(new Rect(Screen.width - 90, 60, 50, 50), dataAns[0].ToString());
        GUI.Label(new Rect(Screen.width - 90, 70, 50, 50), dataAns[1].ToString());
        GUI.Label(new Rect(Screen.width - 90, 80, 50, 50), dataAns[2].ToString());
        GUI.Label(new Rect(Screen.width - 90, 90, 50, 50), dataAns[3].ToString());
        GUI.Label(new Rect(Screen.width - 90, 100, 50, 50), dataAns[4].ToString());
        GUI.Label(new Rect(Screen.width - 90, 110, 50, 50), dataAns[5].ToString());
        GUI.Label(new Rect(Screen.width - 90, 120, 50, 50), dataAns[6].ToString());
        GUI.Label(new Rect(Screen.width - 90, 130, 50, 50), dataAns[7].ToString());
        GUI.Label(new Rect(Screen.width - 90, 140, 50, 50), dataAns[8].ToString());
        GUI.Label(new Rect(Screen.width - 90, 150, 50, 50), dataAns[9].ToString());
        GUI.Label(new Rect(Screen.width - 90, 160, 50, 50), dataAns[10].ToString());
        GUI.Label(new Rect(Screen.width - 90, 170, 50, 50), dataAns[11].ToString());




    }

}