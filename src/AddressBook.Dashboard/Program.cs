using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedisContainer("theCache");

var environmentVars = new Dictionary<string, string>();

string[] keys = { "ConnectionStrings:theDb", "FavoriteColor" };

foreach (var key in keys)
{
  var value = builder.Configuration[key];
  if (value is not null)
  {
    environmentVars.Add(key.Replace(":", "__"), value);
  }
}

var api = builder.AddProject<AddressBook_Api>("theApi")
  .WithReference(cache)
  .WithEnvironment(opt =>
  {
    foreach (var pair in environmentVars)
    {
      if (pair.Value is not null)
      { 
        opt.EnvironmentVariables.Add(pair.Key, pair.Value);
      }
    }
  });

builder.AddNpmApp("frontEnd", "../addressbookapp", "dev")
  .WithReference(api)
  .WithServiceBinding(containerPort: 8001, hostPort: 8001, scheme: "http", env: "PORT");

builder.Build().Run();
