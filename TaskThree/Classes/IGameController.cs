using System;

namespace TaskThree
{
    public interface IGameController
    {
        void RequestUpdateState();
        void HandleStateResponse(DTO data);
        void OnReceiveResponse(int responseID, DTO data);
    }
}
