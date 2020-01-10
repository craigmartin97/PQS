using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers.Interfaces;
using PQS.Models.Models;
using PQS.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Helpers.UserRequests;

namespace Helpers.ModulesHelpers
{
    public class UsersModulesUpdator : BaseUserModuleUpdator//, IUpdator<ObservableCollection<UsersModules>>
    {
        public bool UpdateUsersModules(ObservableCollection<UsersModules> item, User currentUser)
        {
            try
            {
                using (PQSContext db = new PQSContext())
                {
                    if (item != null)
                    {
                        foreach (UsersModules usersMod in item)
                        {
                            Debug.WriteLine("adding / editing " + usersMod.Module.AdminIdentifyingName);

                            PermissionLevels perm = GetPermissionLevel(db, usersMod);
                            Categories cat = GetCategory(db, usersMod);
                            User dbAdminUser = db.Users.Find(currentUser.PK_User_Id);


                            UsersModules dbUsersMod = db.UsersModules.FirstOrDefault(x => x.PK_UsersModules_Id == usersMod.PK_UsersModules_Id);

                            if (dbUsersMod != null) // in the database the user has permission
                            {
                                if (perm != null) // the user still has permission
                                    dbUsersMod.PermissionLevel = perm;
                                else // the user has had there permission removed
                                    db.UsersModules.Remove(dbUsersMod);
                            }
                            else // the user never had permission before
                            {
                                if (usersMod.PermissionLevel != null && usersMod.Module != null)
                                {
                                    Module mod = db.Modules.Find(usersMod.Module.PK_Module_Id);
                                    User user = db.Users.Find(usersMod.User.PK_User_Id);

                                    //CheckIfUserHasMadeRequest(ref db, usersMod);

                                    if (perm != null && mod != null && cat != null && user != null)
                                    {
                                        usersMod.PermissionLevel = perm;
                                        usersMod.Module = mod;
                                        usersMod.Module.Category = cat;
                                        usersMod.User = user;
                                        usersMod.AdminAccessor = dbAdminUser;

                                        db.UsersModules.Add(usersMod);
                                    }
                                }
                            }
                        }

                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message + " " + ex.InnerException);
                return false;
            }
        }

        //private void CheckIfUserHasMadeRequest(ref PQSContext db, UsersModules item)
        //{
        //    // find if the user has made a request for the module that hasnt been completed yet
        //    PQS.Models.Models.UserModuleRequests incompleteRequest = db.UserModuleRequests.FirstOrDefault(x => (x.User.PK_User_Id == item.User.PK_User_Id && x.Module.PK_Module_Id == item.Module.PK_Module_Id)
        //        && x.RequestCompletionDate == null && x.IsComplete == false || x.IsComplete == null);

        //    if (incompleteRequest != null) // user has made a request, that is incomplete for this module
        //    {
        //        incompleteRequest.IsComplete = true;
        //        incompleteRequest.RequestCompletionDate = DateTime.Now;
        //        incompleteRequest.RequestAccepted = true; // ??need to do a check to see if the user gave them the same access??

        //        db.SaveChanges(); // temp added this to do check
        //    }

        //}
    }
}
