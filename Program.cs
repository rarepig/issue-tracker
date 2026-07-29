using System.Text.Json;

string filePath = "Issues.json";
bool exit = false;

IssueTrackerData data;

if (File.Exists(filePath))
{
    string existingJson = File.ReadAllText(filePath);
    data = JsonSerializer.Deserialize<IssueTrackerData>(existingJson)
        ?? new IssueTrackerData();
}
else
{
    data = new IssueTrackerData();
}

while (!exit)
{
    Console.WriteLine("=====Issue Tracker=====");
    Console.WriteLine("1. Create Issue");
    Console.WriteLine("2. View All Issues");
    Console.WriteLine("3. Save Issues");
    Console.WriteLine("4. Exit\n");
    Console.Write("Select an option: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine("\n----Create Issue----");

            string issueTitle;
            while (true)
            {
                Console.Write("Enter issue title: ");
                issueTitle = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(issueTitle))
                {
                    break;
                }
                Console.WriteLine("Title is required.\n");
            }
            Console.Write("Enter issue description: ");
            string issueDescription = Console.ReadLine() ?? "";
            Console.Write("Enter person in charge: ");
            string personInCharge = Console.ReadLine() ?? "";
            Console.Write("\nAdd exactly as entered (Y/N):");

            switch (Console.ReadLine())
            {
                case "Y" or "y":
                    Issue newIssue = new Issue
                    {
                        IssueId = data.NextIssueId,
                        Title = issueTitle,
                        Description = issueDescription,
                        PersonInCharge = personInCharge
                    };

                    data.NextIssueId++;
                    data.Issues.Add(newIssue);

                    Console.WriteLine("\nIssue has been added.\n");
                    break;
                default:
                    Console.WriteLine("\nIssue creation has been canceled.\n");
                    break;
            }
            break;

        case "2":
            Console.WriteLine("\n----Issues----");
            foreach (Issue issue in data.Issues)
            {
                Console.WriteLine($"# {issue.IssueId}: {issue.Title}");
            }
            Console.WriteLine(
                data.Issues.Count == 1
                    ? "\n1 issue found.\n"
                    : $"\n{data.Issues.Count} issues found.\n");
            while (true)
            {
                Console.Write("Select ID for detail('Q' to go back): ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int selectedIssueId) && selectedIssueId > 0)
                {
                    bool issueFound = false;
                    foreach (Issue issue in data.Issues)
                    {
                        if (issue.IssueId == selectedIssueId)
                        {
                            Console.WriteLine("\nAbout Issue # " + issue.IssueId);
                            Console.WriteLine("------------------------------");
                            Console.WriteLine("Title: " + issue.Title);
                            Console.WriteLine("Assignee: " + issue.PersonInCharge);
                            Console.WriteLine("Description: \n" + issue.Description + "\n");
                            issueFound = true;
                            break;
                        }
                    }
                    if (!issueFound)
                    {
                        Console.WriteLine($"\nIssue # {selectedIssueId} not found.\n");
                    }
                    break;
                }
                else
                {
                    if (input == "Q" || input == "q")
                    {
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number.\n");
                    }
                }
            }
            break;

        case "3":
            string updatedJson = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine("\nIssues saved successfully.\n");
            break;

        case "4":
            exit = true;
            break;

        default:
            Console.WriteLine($"\nPlease enter a valid number.\n");
            break;
    }
}

class IssueTrackerData
{
    public int NextIssueId { get; set; } = 1;
    public List<Issue> Issues { get; set; } = new List<Issue>();
}

class Issue
{
    public int IssueId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string PersonInCharge { get; set; } = "";
}