using System.Threading.Tasks;

namespace EP.Services.SystemTasks
{
    public interface IBackgroundTask
    {
        void Start();

        void Stop();

        Task Execute();
    }
}
