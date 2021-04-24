namespace WuQu.Http
{
    using System.Numerics;
    using System.Threading.Tasks;

    public interface IHttpService
    {
        Task Get(string address);
        
        Task Post(string address, object payload);
    }
}