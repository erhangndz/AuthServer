using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.API.Requirements
{
    public class BirthdayRequirement:IAuthorizationRequirement
    {
        public int Age { get; set; }

        public BirthdayRequirement(int age)
        {
            Age = age;
        }



        
    }


    public class BirthdayRequirementHandler : AuthorizationHandler<BirthdayRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthdayRequirement requirement)
        {
            var birthDate = context.User.FindFirst("BirthDate");
            if(birthDate == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

                var today = DateTime.Now;

                var age = today.Year - DateTime.Parse(birthDate.Value).Year;
            
            if(age>= requirement.Age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;





        }
    }
}
