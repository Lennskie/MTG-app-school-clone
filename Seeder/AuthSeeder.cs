using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Seeders
{
    class AuthSeeder
    {
        private UserManager<IdentityUser> userManager = null;
        private RoleManager<IdentityRole> roleManager = null;

        public AuthSeeder(IServiceProvider serviceProvider){
            this.userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); 
            this.roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        public void Run(){
            string[] roleNames = { "Admin", "Manager", "User" };
            CreateRoles(roleNames).Wait();
            CreateUser("milan.lagae@student.howest.be", "!IamTheBest", "Admin").Wait();
            CreateUser("lenn.crochart@student.howest.be", "!IamTheBest!", "Admin").Wait();
            CreateUser("sieger.verdonck@student.howest.be", "!IamTheBest!", "Admin").Wait();
        }


        private async Task CreateUser(string email, string password, string role) {
            IdentityUser user = await this.userManager.FindByEmailAsync(email);
            IdentityResult userResult;
            if(user == null){
                user = new IdentityUser()
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                userResult = await this.userManager.CreateAsync(user, password);
            }

            var roleResult = await this.userManager.AddToRoleAsync(user, role);

        }

        private async Task CreateRoles(string[] roleNames) {
            IdentityResult roleResult;
            foreach (var roleName in roleNames) {
                var roleExist = await this.roleManager.RoleExistsAsync(roleName); 
                if (!roleExist)
                {
                    roleResult = await this.roleManager.CreateAsync(new IdentityRole(roleName)); 
                }
            }
        }
    }
}

