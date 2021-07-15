
using LWY.FW.Common;
using LWY.FW.Models;
using LWY.FW.Models.Entity;


namespace LWY.FW.BLL
{
    public interface ISysManagerService
    {
        BaseWebApiResponse<int> AddUser(User entity);

        BaseWebApiResponse<int> AddFamily(Family family);

        string GetStr();
    }
}
