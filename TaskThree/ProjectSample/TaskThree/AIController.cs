namespace TaskThree
{
    public class AIController : IAvatarController
    {
        IAvatar avatar;
        PhantasmBehavior behavior;

        public void UpdateState(PhatasmStateData data)
        {
            // behavior update the data needed
            // change the avatar visual according to the data
            behavior.Process();
            avatar.DoCommand();
        }
    }
}
