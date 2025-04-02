using DekatMe.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace DekatMe.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Setup dependency injection
            var serviceProvider = ConfigureServices();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("DekatMe Data Management Console started");

            // Display welcome banner
            DisplayWelcomeBanner();

            bool exit = false;
            while (!exit)
            {
                var choice = ShowMainMenu();
                exit = await ProcessMenuChoice(choice, serviceProvider);
            }

            AnsiConsole.WriteLine("Thank you for using DekatMe Data Management Console!");
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register logging
            services.AddLogging(configure => configure.AddConsole());

            // Register services
            services.AddTransient<DataImportService>();
            services.AddTransient<DataExportService>();
            services.AddTransient<StatisticsService>();

            return services.BuildServiceProvider();
        }

        private static void DisplayWelcomeBanner()
        {
            AnsiConsole.Write(
                new FigletText("DekatMe")
                    .Color(Color.Blue)
                    .Centered());

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Data Management Console[/]").Centered());
            AnsiConsole.WriteLine();
        }

        private static string ShowMainMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Import Data",
                        "Export Data",
                        "View Statistics",
                        "Generate Sample Data",
                        "Exit"
                    }));
        }

        private static async Task<bool> ProcessMenuChoice(string choice, ServiceProvider serviceProvider)
        {
            switch (choice)
            {
                case "Import Data":
                    await ImportData(serviceProvider);
                    return false;
                case "Export Data":
                    await ExportData(serviceProvider);
                    return false;
                case "View Statistics":
                    await ViewStatistics(serviceProvider);
                    return false;
                case "Generate Sample Data":
                    await GenerateSampleData(serviceProvider);
                    return false;
                case "Exit":
                    return true;
                default:
                    return false;
            }
        }

        private static async Task ImportData(ServiceProvider serviceProvider)
        {
            var importService = serviceProvider.GetRequiredService<DataImportService>();

            var filePath = AnsiConsole.Ask<string>("Enter the path to the import file:");
            
            if (!File.Exists(filePath))
            {
                AnsiConsole.Write(new Markup($"[red]File not found: {filePath}[/]"));
                AnsiConsole.WriteLine();
                return;
            }
            
            var importType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What type of data would you like to import?")
                    .AddChoices(new[] { "Businesses", "Categories", "Reviews", "Users", "All" }));
                    
            await AnsiConsole.Status()
                .StartAsync("Importing data...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.Status("Reading file...");
                    
                    // Simulate import process
                    await Task.Delay(2000);
                    
                    ctx.Status("Processing data...");
                    await Task.Delay(1500);
                    
                    ctx.Status("Saving to database...");
                    await Task.Delay(2000);
                });
                
            AnsiConsole.MarkupLine("[green]Data imported successfully![/]");
            AnsiConsole.WriteLine();
            
            // Display summary
            var table = new Table();
            table.AddColumn("Entity Type");
            table.AddColumn("Records Imported");
            
            table.AddRow("Businesses", "42");
            table.AddRow("Categories", "8");
            table.AddRow("Reviews", "156");
            table.AddRow("Users", "37");
            
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
            
            PressAnyKeyToContinue();
        }

        private static async Task ExportData(ServiceProvider serviceProvider)
        {
            var exportService = serviceProvider.GetRequiredService<DataExportService>();
            
            var exportType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What type of data would you like to export?")
                    .AddChoices(new[] { "Businesses", "Categories", "Reviews", "Users", "All" }));
                    
            var exportFormat = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select export format:")
                    .AddChoices(new[] { "JSON", "CSV", "XML" }));
                    
            var filePath = AnsiConsole.Ask<string>("Enter the export file path:");
            
            await AnsiConsole.Status()
                .StartAsync("Exporting data...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.Status("Retrieving data...");
                    
                    // Simulate export process
                    await Task.Delay(2000);
                    
                    ctx.Status("Formatting data...");
                    await Task.Delay(1500);
                    
                    ctx.Status("Writing to file...");
                    await Task.Delay(1000);
                });
                
            AnsiConsole.MarkupLine($"[green]Data exported successfully to {filePath}![/]");
            AnsiConsole.WriteLine();
            
            PressAnyKeyToContinue();
        }

        private static async Task ViewStatistics(ServiceProvider serviceProvider)
        {
            var statsService = serviceProvider.GetRequiredService<StatisticsService>();
            
            await AnsiConsole.Status()
                .StartAsync("Gathering statistics...", async ctx =>
                {
                    // Simulate statistics calculation
                    await Task.Delay(2000);
                });
                
            // Display statistics
            AnsiConsole.Write(new Rule("[yellow]System Statistics[/]").Centered());
            AnsiConsole.WriteLine();
            
            // Entity counts
            var barChart = new BarChart()
                .Width(60)
                .Label("[green bold underline]Entity Counts[/]")
                .CenterLabel()
                .AddItem("Businesses", 127, Color.Blue)
                .AddItem("Categories", 8, Color.Green)
                .AddItem("Reviews", 342, Color.Yellow)
                .AddItem("Users", 215, Color.Red);
                
            AnsiConsole.Write(barChart);
            AnsiConsole.WriteLine();
            
            // Category distribution pie chart
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]Category Distribution[/]").Centered());
            AnsiConsole.WriteLine();
            
            var pieChart = new PieChart()
                .Width(60)
                .AddItem("Food & Beverage", 42, Color.Red)
                .AddItem("Shopping", 23, Color.Green)
                .AddItem("Services", 18, Color.Blue)
                .AddItem("Health & Beauty", 15, Color.Yellow)
                .AddItem("Other", 29, Color.Gray);
                
            AnsiConsole.Write(pieChart);
            AnsiConsole.WriteLine();
            
            PressAnyKeyToContinue();
        }

        private static async Task GenerateSampleData(ServiceProvider serviceProvider)
        {
            var dataCount = AnsiConsole.Prompt(
                new TextPrompt<int>("How many records would you like to generate?")
                    .ValidationErrorMessage("[red]Please enter a valid number[/]")
                    .Validate(count => count switch
                    {
                        <= 0 => ValidationResult.Error("[red]Value must be greater than 0[/]"),
                        > 1000 => ValidationResult.Error("[red]Value must be less than or equal to 1000[/]"),
                        _ => ValidationResult.Success(),
                    }));
                    
            var dataType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What type of sample data would you like to generate?")
                    .AddChoices(new[] { "Businesses", "Categories", "Reviews", "Users", "All" }));
                    
            await AnsiConsole.Status()
                .StartAsync("Generating sample data...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Star);
                    
                    // Simulate data generation
                    for (int i = 0; i < 10; i++)
                    {
                        ctx.Status($"Generating data... {i * 10}%");
                        await Task.Delay(300);
                    }
                });
                
            AnsiConsole.MarkupLine($"[green]Successfully generated {dataCount} {dataType.ToLower()}![/]");
            AnsiConsole.WriteLine();
            
            PressAnyKeyToContinue();
        }

        private static void PressAnyKeyToContinue()
        {
            AnsiConsole.WriteLine("Press any key to continue...");
            System.Console.ReadKey(true);
            AnsiConsole.Clear();
            DisplayWelcomeBanner();
        }
    }

    // Mock services for the console application
    public class DataImportService { }
    public class DataExportService { }
    public class StatisticsService { }
}
