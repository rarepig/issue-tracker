using System.Text.Json;

string filePath = "Issues.json";

IssueTrackerData data;
int nextIssueId;
List<Issue> issues;

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

nextIssueId = data.NextIssueId;
issues = data.Issues;

while (true)
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
                Console.Write("Enter issue title:");
                issueTitle = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(issueTitle))
                {
                    break;
                }
                Console.WriteLine("Title is required.");
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
                        IssueId = nextIssueId,
                        Title = issueTitle,
                        Description = issueDescription,
                        PersonInCharge = personInCharge
                    };

                    nextIssueId++;
                    issues.Add(newIssue);

                    Console.WriteLine("\nIssue has been added.\n");
                    break;
                default:
                    Console.WriteLine("\nIssue creation has been canceled.\n");
                    break;
            }
            break;

        case "2":
            Console.WriteLine("\n----Issues----");
            break;

        case "3":
            Console.WriteLine($"\nSaving\n");
            break;

        case "4":
            Console.WriteLine($"\nBye bye.\n");
            break;

        default:
            Console.WriteLine($"\nYou put the wrong number.\n");
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