using System.Collections.Generic;
using System;
using WebApi.Entities;
using WebApi.Models.Users;

namespace WebApi.Interfaces
{
    public interface IUserService
    {
        UserModel Authenticate(string LoginName, string password);
        bool Logout(int userID, string sessionID);
        IEnumerable<User> GetAll();
        IEnumerable<User> GetAllWebUsers();
        User GetById(int id);
        void UpdateLastLogin(int userID, bool isSuccess, DateTime now);
        Guid CreateUserSession(int userID, bool isLDAPLogin, string loginSource, string deviceIDOrIPAddress, DateTime now);
        bool CheckSessionValidity(string sessionID);
        IEnumerable<ToggleColsModel> GetUserSavedToggleColumns(int userID);
        IEnumerable<string> GetDefaultToggleColumns(string webPage);
        void UpdateUserSavedToggleColumns(UpdateUserToggleColsModel model);
        IEnumerable<FiltersModel> GetUserSavedFilters(int userID);
        List<int> GetUserRoles(int userID);
        List<UserRole> GetUserRolesObj(int userID);
        List<int> GetUserRights(int userID);
        UserModel GetUserRolesAndRights(UserModel userModel);
        bool CheckIsAdmin(int userID);
        dynamic GetWeb_AllUsers();
        dynamic GetUserModel_ByUserId(int userID);
        dynamic GetUserDetails_ByUserId(int userID);
    }
}
