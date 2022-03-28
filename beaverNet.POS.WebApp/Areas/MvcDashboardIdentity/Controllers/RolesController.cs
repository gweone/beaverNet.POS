using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beaverNet.POS.WebApp.Areas.MvcDashboardIdentity.Models;
using beaverNet.POS.WebApp.Areas.MvcDashboardIdentity.Models.Roles;
using System.Security.Claims;

namespace beaverNet.POS.WebApp.Areas.MvcDashboardIdentity.Controllers
{
    public class RolesController : BaseController
    {
        #region Construction

        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(IServiceProvider services, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        #endregion

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index(int p = 1, int ps = 10, string q = null)
        {
            // Retrieve data:
            var query = roleManager.Roles.AsQueryable();
            var fullCount = query.Count();
            if (!String.IsNullOrWhiteSpace(q))
                query = query
                    .Where(d => d.NormalizedName.Contains(q));
            var filteredCount = query.Count();
            query = query
                .OrderBy(d => d.NormalizedName);

            // Build model:
            var model = new IndexModel();
            model.DataPage = new DataPage<IdentityRole>()
            {
                CurrentPage = p,
                PageSize = ps,
                FullCount = fullCount,
                FilteredCount = filteredCount,
                Filter = q,
                Items = query.Page(p, ps)
            };

            // Render view:
            return View(model);
        }

        public IActionResult Download()
        {
            var sb = new StringBuilder();
            sb.AppendLine("RoleId,RoleName");
            var query = roleManager.Roles.AsQueryable();
            foreach (var line in query.OrderBy(r => r.Name).Select(r => $"{r.Id},{r.Name}"))
                sb.AppendLine(line);
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return this.File(bytes, "text/csv", "Roles.csv");
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> UpdateDialog(string id = "0")
        {
            // Retrieve data:
            var role = await roleManager.FindByIdAsync(id);

            // Build model:
            var model = new UpdateModel() { Item = role };

            // Render view:
            return UpdateView(model, null);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDialog(UpdateModel model)
        {
            IdentityResult result = null;
            if (this.ModelState.IsValid)
            {
                result = await this.SaveRoleAsync(model.Item);

                if (result == null || result.Succeeded)
                {
                    return DialogOk();
                }
            }

            return UpdateView(model, result);
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return View("DeleteRequestDialog", new DeleteRequestModel() { Item = role });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateModel model)
        {
            if (model.Item.Id == null)
            {
                return DialogClose();
            }
            else
            {
                IdentityResult result = await roleManager.DeleteAsync(model.Item);

                if (result.Succeeded)
                {
                    return DialogOk();
                }

                return UpdateView(model, result);
            }
        }

        private IActionResult UpdateView(UpdateModel model, IdentityResult identityResult)
        {
            if (identityResult != null && !identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("UpdateDialog", model);
        }

        public async Task<IActionResult> SyncAuthorize()
        {
            var asm = this.GetType().Assembly;
            var methods = asm.ExportedTypes
                .Where(type => typeof(Controller)
                    .IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic
                    && (
                        method.ReturnType == typeof(IActionResult) ||
                        method.ReturnType == typeof(Task<IActionResult>)
                        )
                    )
                .Select(m => new { Action = m.Name, Controller = m.DeclaringType.Name.Replace("Controller", "") });

            var role = await roleManager.FindByNameAsync("Administrator");
            var claims = await roleManager.GetClaimsAsync(role);

            foreach (var item in methods)
            {
                var rolename = string.Format("{0}.{1}", item.Controller, item.Action).ToLowerInvariant();
                if (claims.Any(x => x.Type == ClaimTypes.UserData && x.Value == rolename))
                    continue;
                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(ClaimTypes.UserData, rolename));
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task<IdentityResult> SaveRoleAsync(IdentityRole role)
        {
            if (role.Id == null)
            {
                role.Id = Guid.NewGuid().ToString();
                return await roleManager.CreateAsync(role);
            }
            else
            {
                var storedRole = await roleManager.FindByIdAsync(role.Id);
                if (storedRole.Name == role.Name) return null;

                storedRole.Name = role.Name;
                storedRole.ConcurrencyStamp = role.ConcurrencyStamp;
                return await roleManager.UpdateAsync(storedRole);
            }
        }
    }
}
