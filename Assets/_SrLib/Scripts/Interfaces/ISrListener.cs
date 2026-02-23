namespace SrLib
{
    /// <summary>
    /// エンジンからのコールバック
    /// </summary>
    public interface ISrListener
    {
        public void Restart();
        public void OnApplicationQuit();
    }
}
