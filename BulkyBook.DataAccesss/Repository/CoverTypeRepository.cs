using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository( ApplicationDbContext db) :base(db)
        {
            _db = db;

        }
        public void Update(CoverType coverType)
        {
            var objCoverType = _db.CoverTypes.FirstOrDefault(c => c.Id == coverType.Id);

            if (objCoverType != null)
            {
                objCoverType.Name = coverType.Name;

            }
        }
    }
}
