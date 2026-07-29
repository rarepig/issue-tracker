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
    Console.WriteLine("4. Exit");
    Console.WriteLine();
    Console.Write("Select an option: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine();
            Console.WriteLine("----Create Issue----");

            string issueTitle;
            while (true)
            {
                Console.Write("Enter issue title: ");
                issueTitle = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(issueTitle))
                {
                    break;
                }
                Console.WriteLine();
                Console.WriteLine("Title is required.");
            }
            Console.Write("Enter issue description: ");
            string issueDescription = Console.ReadLine() ?? "";
            Console.Write("Enter person in charge: ");
            string personInCharge = Console.ReadLine() ?? "";
            Console.WriteLine();
            Console.Write("Add exactly as entered (Y/N): ");

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

                    Console.WriteLine();
                    Console.WriteLine("Issue has been added.");
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Issue creation has been canceled.");
                    Console.WriteLine();
                    break;
            }
            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
            break;

        case "2":
            Console.WriteLine();
            Console.WriteLine("----Issues----");
            foreach (Issue issue in data.Issues)
            {
                Console.WriteLine($"# {issue.IssueId}: {issue.Title}");
            }
            Console.WriteLine();
            Console.WriteLine(
                data.Issues.Count == 1
                    ? "1 issue found."
                    : $"{data.Issues.Count} issues found.");
            Console.WriteLine();
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
                            Console.WriteLine();
                            Console.WriteLine("About Issue # " + issue.IssueId);
                            Console.WriteLine("------------------------------");
                            Console.WriteLine("Title: " + issue.Title);
                            Console.WriteLine("Assignee: " + issue.PersonInCharge);
                            Console.WriteLine("Description:");
                            Console.WriteLine(issue.Description);
                            Console.WriteLine();
                            issueFound = true;
                            break;
                        }
                    }
                    if (!issueFound)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Issue # {selectedIssueId} not found.");
                        Console.WriteLine();
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
                        Console.WriteLine();
                        Console.WriteLine("Please enter a valid number.");
                    }
                }
            }
            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
            break;

        case "3":
            string updatedJson = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine();
            Console.WriteLine("Issues saved successfully.");
            Console.WriteLine();
            Console.WriteLine("Press Enter to return to the main menu.");
            Console.ReadLine();
            break;

        case "4":
            exit = true;
            break;

        default:
            Console.WriteLine();
            Console.WriteLine("Please enter a valid number.");
            Console.WriteLine();
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
