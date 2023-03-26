//using CIProjectweb.Models.DataModels;
//using CIProjectweb.Models.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CIProjectweb.Repositories.Repository
//{
//    public class registration
//    {
//        private readonly CIDbContext _context;

//        public registration(CIDbContext context)
//        {
//            _context = context;
//        }

//        public int RegisterUser(Registration model)
//        {
//            var register = new User()
//            {
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                Email = model.Email,
//                PhoneNumber=model.PhoneNumber,
               
//            };

//            _context.Users.Add(register);
//            _context.SaveChangesAsync();
//            return model.PhoneNumber;

//        }
//    }
//}
