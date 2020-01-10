using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using PQS.Models;
using Helpers.PermissionLevelsHelper;

namespace Helpers.UserRequests
{
    public class CompleteUserRequest : BaseUserModuleUpdator
    {
        public bool CompleteRequest(UserModuleRequests request)
        {
            using (PQSContext db = new PQSContext())
            {

                User dbUser = db.Users.Find(request.User.PK_User_Id);
                Module dbMod = db.Modules.Find(request.Module.PK_Module_Id);
                PermissionLevels dbPerm = db.PermissionLevels.Find(request.PermissionLevel.PK_PermissionLevel_Id);

                UsersModules userMod = new UsersModules()
                {
                    User = dbUser,
                    Module = dbMod,
                    PermissionLevel = dbPerm,
                    IsFavourite = false
                };

                userMod.Module.Category = GetCategory(db, userMod);
                db.UsersModules.Add(userMod);

                UserModuleRequests dbRequest = db.UserModuleRequests.Find(request.PK_UserModuleRequest_Id);

                if (dbRequest == null) // somethings, happened, the user themselves has deleted the request in the last n secs
                    return false;


                dbRequest.IsComplete = true;
                dbRequest.RequestAccepted = true;
                dbRequest.RequestCompletionDate = DateTime.Now;

                db.SaveChanges();
                return true;
            }
        }

        

        public bool RejectUsersRequest(UserModuleRequests request)
        {
            using (PQSContext db = new PQSContext())
            {
                UserModuleRequests dbRequest = db.UserModuleRequests.Find(request.PK_UserModuleRequest_Id);
                dbRequest.IsComplete = true;
                dbRequest.RequestCompletionDate = DateTime.Now;
                dbRequest.RequestAccepted = false;

                db.SaveChanges();
                return true;
            }
        }

        public void CompleteRequest(UsersModules usersModule)
        {
            AvaliableUserPermissionLevels getPermLevels = new AvaliableUserPermissionLevels();
            CompleteRequest(getPermLevels.GetAvaliablePermissionLevelsForUser(usersModule));
        }

        public void RejectUsersRequest(UsersModules usersModule)
        {
            AvaliableUserPermissionLevels getPermLevels = new AvaliableUserPermissionLevels();
            RejectUsersRequest(getPermLevels.GetAvaliablePermissionLevelsForUser(usersModule));
        }
    }
}
