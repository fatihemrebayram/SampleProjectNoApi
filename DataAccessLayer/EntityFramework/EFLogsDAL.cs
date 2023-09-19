using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;

namespace DataAccessLayer.EntityFramewok;

public class EFLogsDAL : GenericRepositories<Logs>, ILogDAL
{
}