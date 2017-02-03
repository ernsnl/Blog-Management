using System.Security.Cryptography.X509Certificates;

namespace BlogApplication.Entity
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}