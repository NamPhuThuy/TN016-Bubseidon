namespace NamPhuThuy
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}