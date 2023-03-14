using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskThree
{
    public class GameController : IGameController
    {
        INetworkManager _networkManager;
        IAvatarController[] _aiControllers;

        public GameController()
        {
            _networkManager = new NetworkManager(OnReceiveResponse);
        }
        public void HandleStateResponse(DTO data)
        {
            // update all the AI state data
            foreach (var aiController in _aiControllers)
            {
                aiController.UpdateState(someData);
            }
        }

        public void OnReceiveResponse(int responseID, DTO data)
        {
            // check if the responseID is for PhantasmState
            if (responseID == UpdatePhatasmStateID)
            {
                HandleStateResponse(data);
            }
        }

        public void RequestUpdateState()
        {
            // request ID for this particular job
            // DTO: maybe phantasmID, or additional data needed
            _networkManager.SendRequest(requestID, new DTO());
        }

        public void Update(float deltaTime)
        {
            if (IsItTimeToUpdatePhatasm())
            {
                RequestUpdateState();
            }
        }
    }
}
