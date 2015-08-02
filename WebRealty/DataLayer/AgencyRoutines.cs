using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRealty.Models;
using RealtyDomainObjects;

namespace WebRealty.DataLayer
{
    public class AgencyRoutines:IRoutines<Agency>
    {
        private RealtyDb _db;
        public AgencyRoutines(RealtyDb _db)
        {
            this._db = _db;
        }

        public IQueryable<Agency> GetAgency(int cityId, int districtId, int page,int pageSize)
        {
            var result = from s in this._db.Agencies.Include("City").Include("District")                         
                         select s;

            if (cityId > 0)
                result = result.Where(m => m.City.Id == cityId);
            if (districtId > 0)
                result = result.Where(m => m.District.Id == districtId);
            result = result.OrderBy(m => m.Name);
            result = result.Skip((page - 1) * pageSize).Take(pageSize);

            return result;



        }


        public void Create(Agency obj)
        {
            this._db.Agencies.Add(obj);
            this._db.Entry(obj.City).State = System.Data.EntityState.Unchanged;
            this._db.Entry(obj.District).State = System.Data.EntityState.Unchanged;
            this._db.SaveChanges();

        }

        public void Update(Agency obj)
        {
            var existingObj = this.GetById(obj.Id);

            existingObj.Name = obj.Name;
            existingObj.City = obj.City;
            existingObj.District = obj.District;
            existingObj.AboutBrief = obj.AboutBrief;
            existingObj.AboutDetailed = obj.AboutDetailed;
            existingObj.Photo = obj.Photo;
            this._db.Entry(existingObj).State = System.Data.EntityState.Modified;
            this._db.SaveChanges();

        }

        public void Delete(Agency obj)
        {
            var existingObj = this.GetById(obj.Id);
            existingObj.IsDeleted = true;
            existingObj.DeletedDate = DateTime.Now;
            this._db.Entry(existingObj).State = System.Data.EntityState.Modified;
            this._db.SaveChanges();

        }

        public void Delete(int id)
        {
            var existingObj = this.GetById(id);
            existingObj.IsDeleted = true;
            existingObj.DeletedDate = DateTime.Now;
            this._db.Entry(existingObj).State = System.Data.EntityState.Modified;
            this._db.SaveChanges();
        }

        public IQueryable<Agency> GetSet()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Agency> GetAllSet()
        {
            throw new NotImplementedException();
        }

        public Agency GetById(int id)
        {
            return this._db.Agencies.Include("City").Include("District")
                .Where(m => m.IsDeleted == false && m.Id == id).SingleOrDefault<Agency>();

        }

        public Agency GetAnyById(int id)
        {
            return this._db.Agencies.Include("City").Include("District")
                .Where(m => m.Id == id).SingleOrDefault<Agency>();
        }
    }
}