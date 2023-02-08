using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListAppTests.Helpers;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace TodoListAppTests
{
   public class TodoListTestCases
    {

        [Fact]
        public async Task TestGetEndpointIsNull()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var Client = application.CreateClient();

            var response = await Client.GetAsync("/TodoList");
            var data = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

                     

        }
        [Fact]
        public async Task TestPostEndPointGetValues()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var Client = application.CreateClient();
            
            using (var scope = application.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using(var todoListDb = provider.GetRequiredService<TodoListDbContext>())
                {
                    await todoListDb.Database.EnsureCreatedAsync();

                    await todoListDb.todoLists.AddAsync(new TodoList
                    {
                        Id = 1,
                        TaskName = "Wash Vessels",
                        Duedate = new DateTime(2023 - 02 - 01)

                    });

                    await todoListDb.todoLists.AddAsync(new TodoList
                    {

                        Id = 2,
                        TaskName = "Clean Gutter",
                        Duedate = new DateTime(2023 - 02 - 22)
                    });

                    await todoListDb.todoLists.AddAsync(new TodoList
                    {

                        Id = 3,
                        TaskName = "Walk the dog",
                        Duedate = new DateTime(2023 - 02 - 08)
                    });

                    await todoListDb.SaveChangesAsync();
                  
                 }
            }

           var data = await Client.GetFromJsonAsync<List<TodoList>>("/TodoList/FetchTasks");
         
           var foundData = Assert.IsAssignableFrom<List<TodoList>>(data);

             Assert.NotNull(foundData);
             Assert.Collection(foundData, data1 =>
             {
                 Assert.Equal("Wash Vessels", data1.TaskName);
                 Assert.Equal(1, data1.Id);
                 Assert.False(data1.IsCompleted);
                 
             }, data2 =>
             {
                 Assert.Equal("Clean Gutter", data2.TaskName);
                 Assert.Equal(2, data2.Id);
                 Assert.False(data2.IsCompleted);
                 
             }, data3 =>
             {
                 Assert.Equal("Walk the dog", data3.TaskName);
                 Assert.Equal(3, data3.Id);
                 Assert.False(data3.IsCompleted);
                 
             }


             );
        }

               

    }
}
