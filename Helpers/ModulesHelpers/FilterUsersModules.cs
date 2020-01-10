using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PQS.Models.Models;
using Helpers.Interfaces;
using System.Collections.ObjectModel;
using PQS.Models;

namespace Helpers.ModulesHelpers
{
    public class FilterUsersModules
    {
        public ObservableCollection<UsersModules> GetAllModulesWithUsersPopulated(User user, IRetrievor<Module> moduleRetrievor, IAdvancedRetrievor<User, UsersModules> usersModulesRetrievor)
        {
            ObservableCollection<UsersModules> allModules = new ObservableCollection<UsersModules>();

            if (user != null)
            {
                List<Module> modules = new List<Module>(moduleRetrievor.GetAll().Where(x => x.IsActive));
                List<UsersModules> usersModules = usersModulesRetrievor.GetFilteredList(user).ToList();

                foreach (Module module in modules)
                {
                    UsersModules match = usersModules.FirstOrDefault(x => x.Module.PK_Module_Id == module.PK_Module_Id);
                    if (match != null) // found match
                    {
                        allModules.Add(match);
                    }
                    else
                    {
                        allModules.Add(new UsersModules
                        {
                            Module = module,
                            PermissionLevel = null,
                            User = user
                        });
                    }
                }

                allModules.OrderBy(x => x.PermissionLevel != null).ThenBy(x => x.Module.AdminIdentifyingName);
            }

            return allModules;
        }

        public void SetAllPermissionLevelOptions(IList<UsersModules> allUsersModules)
        {
            using (PQSContext db = new PQSContext())
            {
                foreach (UsersModules usersModule in allUsersModules)
                {
                    UserModuleRequests request = db.UserModuleRequests.FirstOrDefault(x => (x.Module.PK_Module_Id == usersModule.Module.PK_Module_Id && x.User.PK_User_Id == usersModule.User.PK_User_Id)
                        && (x.IsComplete == null || x.IsComplete == false) && x.RequestCompletionDate == null);

                    if (request != null)
                    {
                        usersModule.PermissionLevelOptions = new ObservableCollection<PermissionLevels>()
                        {
                            new GrantAccessPermissionLevels()
                            {
                                PK_PermissionLevel_Id = request.PermissionLevel.PK_PermissionLevel_Id,
                                Order = request.PermissionLevel.Order,
                                IsEditorPermission = request.PermissionLevel.IsEditorPermission,
                                AccessDescription = "Accept Request",
                                HasRequested = true,
                                IsAccessRequest = true
                            },
                            new GrantAccessPermissionLevels()
                            {
                                PK_PermissionLevel_Id = request.PermissionLevel.PK_PermissionLevel_Id,
                                Order = request.PermissionLevel.Order,
                                IsEditorPermission = request.PermissionLevel.IsEditorPermission,
                                AccessDescription = "Reject Request",
                                HasRequested = true,
                                IsAccessRequest = false
                            }
                        };


                        usersModule.PermissionLevel = request.PermissionLevel;
                        usersModule.RequestAlreadyMadePermissionLevelOptions = false;
                    }
                    else
                    {
                        // user hasnt made request or already has permission
                        //IList<GrantAccessPermissionLevels> t = (IList<PermissionLevels>)db.PermissionLevels.ToList();
                        usersModule.PermissionLevelOptions = new ObservableCollection<PermissionLevels>(db.PermissionLevels.ToList());
                        usersModule.RequestAlreadyMadePermissionLevelOptions = true;
                    }
                }
            }
        }
    }
}
