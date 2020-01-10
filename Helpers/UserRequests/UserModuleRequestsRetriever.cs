using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using Helpers.ModulesHelpers;
using PQS.Models;
using System.Diagnostics;

namespace Helpers.UserRequests
{
    public class UserModuleRequestsRetriever : IAdvancedRetrievor<User, UserModuleRequests>
    {
        public IList<UserModuleRequests> GetFilteredList(User item)
        {
            try
            {
                IList<UserModuleRequests> userModulesRequests = new List<UserModuleRequests>();

                IList<UsersModules> usersModules = new UsersModulesProcessor().GetFilteredList(item);
                IList<Module> modules = new ModulesRetrievor().GetAll();

                using (PQSContext db = new PQSContext())
                {
                    foreach (Module module in modules)
                    {
                        if (usersModules.Any((x => x.Module.PK_Module_Id == module.PK_Module_Id))) // user already has access then skip
                            continue;

                        UserModuleRequests requestMadeForModule = db.UserModuleRequests.FirstOrDefault(x =>
                            x.Module.PK_Module_Id == module.PK_Module_Id
                            && x.User.PK_User_Id == item.PK_User_Id
                            && (x.IsComplete == false || x.IsComplete == null)
                            && (x.RequestAccepted == false || x.RequestAccepted == null));

                        bool requestAccess = requestMadeForModule != null;

                        userModulesRequests.Add(new UserModuleRequests()
                        {
                            PK_UserModuleRequest_Id = requestMadeForModule != null ? requestMadeForModule.PK_UserModuleRequest_Id : 0,
                            Module = module,
                            User = item,
                            RequestAlreadyMade = requestAccess
                        });
                    }

                    return userModulesRequests;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
