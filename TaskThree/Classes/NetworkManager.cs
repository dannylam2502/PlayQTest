using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    public class NetworkManager : INetworkManager
    {
        public delegate void Callback(int responseID, DTO data);

        // have a callback to notify the GameController about the response
        Callback callbackHandler;
        public NetworkManager(Callback callbackHandler)
        {
            this.callbackHandler = callbackHandler;
        }

        public void OnReceivedResponse(int responseID, DTO data)
        {
            // invoke the callback when we received the response
            callbackHandler?.Invoke(responseID, data);
        }

        public void SendRequest(int requestID, DTO data)
        {
            // request RESTFUL API
            // parse the response, get the responseID & DTO data
            if (handler.IsSucceed)
            {
                OnReceivedResponse(responseID, responseData);
            }
            else
            { 
               // HandleException or Time -out
            }
        }
    }
}
