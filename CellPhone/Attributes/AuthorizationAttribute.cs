using CellPhone.Datas;
using CellPhone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CellPhone.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly CellPhoneContext _context = new CellPhoneContext();
        public string Role { set; get; }

        public AuthorizationAttribute() { }
        public AuthorizationAttribute(string permission) {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext != null)
            {
                var httpContext = filterContext.HttpContext;
                var session = httpContext.Session;
                var userClaims = session["user"] as List<Claim>;
                if (userClaims == null) { httpContext.Response.Redirect("/user/login"); return; }
                var isAuthorize = false;
                if(_permission != null)
                {
                    var roleClaims = userClaims.Where(u => u.Type == ClaimTypes.Role).ToList();
                    foreach (var roleClaim in roleClaims)
                    {
                        if( _context.RolePermissions
                            .Include(rp => rp.Role)
                            .Include(rp => rp.Permission)
                            .Any(rp => rp.Role.Name == roleClaim.Value && rp.Permission.PermissionName == _permission)
                          || roleClaim.Value == RoleData.Admin)
                        {
                            isAuthorize = true; break;
                        }
                    }

                    if(!isAuthorize) { httpContext.Response.Redirect("/"); return; }
                }
            }
        }
    }
}