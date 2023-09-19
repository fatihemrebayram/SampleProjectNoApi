using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class LogManager : ILogService
    {
        private ILogDAL _logService;

        public LogManager(ILogDAL logService)
        {
            _logService = logService;
        }

        public void AddBL(Logs t)
        {
            throw new NotImplementedException();
        }

        public void AddRangeBL(List<Logs> p)
        {
            var context = new Context();
            context.AddRange(p);
            context.SaveChanges();
        }

        public List<Logs> GetListBL()
        {
            return _logService.ListDAL();
        }

        public List<Logs> GetListFilteredBL(string filter)
        {
            return _logService.ListDAL(x => x.MessageTemplate.Contains(filter) ||
              x.Level.Contains(filter) ||
              x.Message.Contains(filter) ||
              x.TimeStamp.ToString().Contains(filter) ||
              x.UserDomainNamePC.Contains(filter) ||
              x.Username.Contains(filter) ||
              x.UserNamePC.Contains(filter));
        }

        public void RemoveBL(Logs t)
        {
            throw new NotImplementedException();
        }

        public Logs TGetByID(int id)
        {
            return _logService.GetByIdDAL(id);
        }

        public void UpdateBL(Logs t)
        {
            throw new NotImplementedException();
        }
    }
}