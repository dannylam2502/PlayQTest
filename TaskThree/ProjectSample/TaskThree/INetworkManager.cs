namespace TaskThree
{
    public interface INetworkManager
    {
        void SendRequest(int requestID, DTO data);
        void OnReceivedResponse(int responseID, DTO data);
    }
}
