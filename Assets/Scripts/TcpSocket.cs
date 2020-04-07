using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;


public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Receive buffer.  
    public byte[] buf;
}

public class TcpSocket : UnityEngine.Object
{

    // ManualResetEvent instances signal completion.  
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private static ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.  
    private static string response = string.Empty;

    Socket client;

    private ByteQueue _incomingData;
    private ByteQueue _outgoingData;

    private TcpSocket() { }
    
    public TcpSocket(ref ByteQueue incomingData,ref ByteQueue outgoingData)
    {
        _incomingData = incomingData;
        _outgoingData = outgoingData;
    }

    public void Connect(string remoteHost, int remotePort)
    {
        if (client != null && client.Connected)
            Disconnect();

        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.   
            IPHostEntry ipHostInfo = Dns.GetHostEntry(remoteHost);
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, remotePort);

            // Create a TCP/IP  socket.  
            client = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);

        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            Debug.Log("Socket connected to " +
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.  
            connectDone.Set();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void Receive()
    {
        try
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.buf = new byte[_incomingData.queueCapacity];
            state.workSocket = client;

            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buf, 0, _incomingData.queueCapacity, 0,
                new AsyncCallback(ReceiveCallback), state);
            receiveDone.WaitOne();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                Debug.Log("Data received: " + bytesRead + " bytes.");

                // There might be more data, so store the data received so far.  
                _incomingData.WriteBlock(state.buf, bytesRead);

                // Signal that all bytes have been received.  
                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void Send()
    {

        // Begin sending the data to the remote device.  
        client.BeginSend(_outgoingData.data, 0, _outgoingData.queueLength, 0,
            new AsyncCallback(SendCallback), client);

        _outgoingData.RemoveData(_outgoingData.queueLength);

    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            Debug.Log("Sent "+ bytesSent + " bytes to server.");

            // Signal that all bytes have been sent.  
            sendDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void Disconnect()
    {
        if (client != null && client.Connected)
        {
            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }

    public bool IsConnected()
    {
        return client != null && client.Connected;
    }

    public bool IsDataAvailableToRead()
    {
        return client != null && client.Poll(1000, SelectMode.SelectRead);
    }
}
