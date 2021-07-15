
using LWY.FW.Common;
using LWY.FW.DAL;
using LWY.FW.Models.Entity;

namespace LWY.FW.BLL
{
    public class SysManagerServiceImpl:ISysManagerService
    {
        #region   注入
        public IRepository<User> UserRepository { get; set; }

        public IRepository<Family> FamilyRepository { get; set; }

        #endregion 

        public BaseWebApiResponse<int> AddUser(User user)
        {
            User entity = UserRepository.Add(user);
            return new BaseWebApiResponse<int>(entity.ID);
        }

        public BaseWebApiResponse<int> AddFamily(Family family)
        {
            Family entity = FamilyRepository.Add(family);
            return new BaseWebApiResponse<int>(0);
        }

        public string GetStr()
        {
            return "123";
        }
    }
}
