using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;

public class UDPServer : MonoBehaviour
{
    userDB userDb;

    [SerializeField] GameObject connectionManager;

    List<UdpClient> udpClients;

    void Start()
    {
        userDb = connectionManager.GetComponent<userDB>();
        udpClients = new List<UdpClient>();
    }

    public void addClient(string addr, int port)
    {
        udpClients.Add(new UdpClient(addr, port));

        Task.Run(() => SendData(udpClients[udpClients.Count-1], addr));
    }

    private void SendData(UdpClient client, string addr)
    {
        int id = userDb.getId(addr);
        Transform tf = userDb.getData(id);
        var message = Encoding.UTF8.GetBytes(tf.ToString());
        client.Send(message, message.Length) ;
    }

    private void OnDestroy()
    {
        foreach (UdpClient uc in udpClients) uc.Dispose();
        
    }
}
