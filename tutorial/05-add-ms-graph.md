<!-- markdownlint-disable MD002 MD041 -->

In this demo you will incorporate Microsoft Graph into the application. For this application, you will use the [Microsoft Graph Client Library for .NET](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to Microsoft Graph.

## Get calendar events from Outlook

Start by creating a new controller for calendar views.

1. Add a new file named **CalendarController.cs** in the **./Controllers** directory and add the following code.

    :::code language="csharp" source="../demo/GraphTutorial/Controllers/CalendarController.cs" id="UsingSnippet,SecondSnippet":::

    Consider what the code in `GetUserWeekCalendar` does.

    - It uses the user's time zone to get UTC start and end date/time values for the week.
    - It queries the user's [calendar view](/graph/api/calendar-list-calendarview?view=graph-rest-1.0) to get all events that fall between the start and end date/times. Using a calendar view instead of [listing events](/graph/api/user-list-events?view=graph-rest-1.0) expands recurring events, returning any occurrences that happen in the specified time window.
    - It uses the `Prefer: outlook.timezone` header to get results back in the user's timezone.
    - It uses `Select` to limit the fields that come back to just those used by the app.
    - It uses `OrderBy` to sort the results chronologically.
    - It uses a `PageIterator` to [page through the events collection](/graph/sdks/paging). This handles the case where the user has more events on their calendar than the requested page size.

1. Add the following function to the `CalendarController` class to implement a temporary view of the returned data.

    ```csharp
    // Minimum permission scope needed for this view
    [AuthorizeForScopes(Scopes = new[] { "Calendars.Read" })]
    public async Task<IActionResult> Index()
    {
        var events = await GetUserWeekCalendar();

        // TEMPORARY
        // Create a Graph client just to access its
        // serializer
        var graphClient = GraphServiceClientFactory
            .GetAuthenticatedGraphClient(async () =>
            {
                return await _tokenAcquisition
                    .GetAccessTokenForUserAsync(GraphConstants.Scopes);
            }
        );

        // Return a JSON dump of events
        return new ContentResult {
            Content = graphClient.HttpProvider.Serializer.SerializeObject(events),
            ContentType = "application/json"
        };
    }
    ```

1. Start the app, sign in, and click the **Calendar** link in the nav bar. If everything works, you should see a JSON dump of events on the user's calendar.

## Display the results

Now you can add a view to display the results in a more user-friendly manner.

1. In Solution Explorer, right-click the **Views/Calendar** folder and select **Add > View...**. Name the view `Index` and select **Add**. Replace the entire contents of the new file with the following code.

    ```html
    @model IEnumerable<Microsoft.Graph.Event>

    @{
        ViewBag.Current = "Calendar";
    }

    <h1>Calendar</h1>
    <table class="table">
        <thead>
            <tr>
                <th scope="col">Organizer</th>
                <th scope="col">Subject</th>
                <th scope="col">Start</th>
                <th scope="col">End</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model)
            {
                <tr>
                    <td>@item.Organizer.EmailAddress.Name</td>
                    <td>@item.Subject</td>
                    <td>@Convert.ToDateTime(item.Start.DateTime).ToString("M/d/yy h:mm tt")</td>
                    <td>@Convert.ToDateTime(item.End.DateTime).ToString("M/d/yy h:mm tt")</td>
                </tr>
            }
        </tbody>
    </table>
    ```

    That will loop through a collection of events and add a table row for each one.

1. Remove the `return Json(events, JsonRequestBehavior.AllowGet);` line from the `Index` function in `Controllers/CalendarController.cs`, and replace it with the following code.

    ```cs
    return View(events);
    ```

1. Start the app, sign in, and click the **Calendar** link. The app should now render a table of events.

    ![A screenshot of the table of events](./images/add-msgraph-01.png)
