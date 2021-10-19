﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShop.Models.EF;
using PagedList.Mvc;
using PagedList;
namespace OnlineShop.Models.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public IEnumerable<User> ListAllPaging(string searchString,int page,int pageSize)
        {
            IQueryable<User> model = db.Users;
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));

            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page,pageSize);
        }
        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public int Login(string userName,string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if(result==null)
            {
                return 0;
            }
            else
            {
                if(result.Status==false)
                {
                    return -1;
                }
                else
                {
                    if(result.Password==passWord)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2; 
                    }
                }
            }
        }
    }
}