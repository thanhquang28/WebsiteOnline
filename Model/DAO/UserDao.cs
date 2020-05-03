using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
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

        public int GetUserExisted(string userName)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName);
            if (result ==null) //account have not created
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x => x.Username == userName);
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == userName);
            if (result == null )
            {
                return 0; //Account doesn't exist
            } else
            {
                if (result.Status == false)
                {
                    return -1; //Account has been locked
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        return 1;//true->full access
                    }
                    else
                    {
                        return -2;//Wrong Password
                    }
                }
            }
        }
    }
}
