namespace PlayWrightTimeout;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunTestAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected issue");
            }
        }
    }

    private async Task RunTestAsync()
    {
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();

        page.Response += (_, response) =>
        {
            if (response.Request.IsNavigationRequest)
            {
                _logger.LogInformation(
                    "{method} {status} {response}",
                    response.Request.Method,
                    response.Status,
                    response.Request.Url
                );
            }
        };

        await page.GotoAsync("https://demo.duendesoftware.com/grants");

        //Random timeouts happen here sometimes
        await page.WaitForURLAsync("https://demo.duendesoftware.com/Account/Login**");

        await page.FillAsync("#Input_Username", "bob");
        await page.FillAsync("#Input_Password", "bob");

        await page.ClickAsync("button[name='Input.Button']");

        //Random timeouts happen here sometimes
        await page.WaitForURLAsync("https://demo.duendesoftware.com/grants");

        //Timeouts disappear when doing:
        // await Task.WhenAll(
        //     page.WaitForURLAsync("https://demo.duendesoftware.com/grants"),
        //     page.ClickAsync("button[name='Input.Button']")
        // );
    }
}
