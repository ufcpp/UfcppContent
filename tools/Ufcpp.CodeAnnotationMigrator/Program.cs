using Ufcpp.CodeAnnotationMigrator;

return await MigratorCli.RunAsync(
    args,
    Console.OpenStandardOutput(),
    Console.Error,
    Directory.GetCurrentDirectory());
