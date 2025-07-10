//namespace MovieMateAPI
//{
//    public static class ApiEnpoints
//    {
//         public static void ConfigureAPI(this WebApplication app)
//        {
//            // Add route mapping for GetUsers
//            //app.MapGet("/users", GetUsers);
//            //app.MapGet("/users/{id:int}", GetUser);
//            //app.MapPost("/users", InsertUser);
//            //app.MapPut("/users/{id:int}", UpdateUser);
//            //app.MapDelete("/users/{id:int}", DeleteUser);

//            app.MapGet("/shiftData", GetShiftData);

//        }

//        //IUserData data is comming from the global using ile, so no need to inject seperately

//        private static async Task<IResult> GetShiftData(IShiftData data)
//        {
//            //try
//            //{
//            var users = await data.GetShifts();
//            return Results.Ok(users);
//            //}
//            //catch (Exception ex)
//            //{
//            //    return Results.Problem(ex.Message);
//            //}
//        }




//        private static async Task<IResult> GetUsers(IUserData data)
//        {
//            //try
//            //{
//            var users = await data.GetUsers();
//            return Results.Ok(users);
//            //}
//            //catch (Exception ex)
//            //{
//            //    return Results.Problem(ex.Message);
//            //}
//        }

//        private static async Task<IResult> GetUser(int id, IUserData data)
//        {
//            try
//            {
//                var user = await data.GetUser(id);
//                return user is not null ? Results.Ok(user) : Results.NotFound();
//            }
//            catch (Exception ex)
//            {
//                return Results.Problem(ex.Message);
//            }
//        }
//        private static async Task<IResult> InsertUser(UserModel user, IUserData data)
//        {
//            try
//            {
//                await data.InsertUser(user);
//                return Results.Created($"/users/{user.Id}", user);
//            }
//            catch (Exception ex)
//            {
//                return Results.Problem(ex.Message);
//            }
//        }

//        private static async Task<IResult> UpdateUser(int id, UserModel user, IUserData data)
//        {
//            try
//            {
//                user.Id = id; // Ensure the ID is set for the update
//                await data.UpdateUser(user);
//                return Results.Ok();
//            }
//            catch (Exception ex)
//            {
//                return Results.Problem(ex.Message);
//            }
//        }

//        private static async Task<IResult> DeleteUser(int id, IUserData data)
//        {
//            try
//            {
//                await data.DeleteUser(id);
//                return Results.NoContent();
//            }
//            catch (Exception ex)
//            {
//                return Results.Problem(ex.Message);
//            }
//        }



//    }
//}
