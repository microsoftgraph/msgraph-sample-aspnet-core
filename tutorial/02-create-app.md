<!-- markdownlint-disable MD002 MD041 -->

Start by creating an ASP.NET Core web app.

1. Open your command-line interface (CLI) in a directory where you want to create the project. Run the following command.

    ```Shell
    dotnet new mvc -o GraphTutorial
    ```

1. Once the project is created, verify that it works by changing the current directory to the **GraphTutorial** directory and running the following command in your CLI.

    ```Shell
    dotnet run
    ```

1. Open your browser and browse to `https://localhost:5001`. If everything is working, you should see a default ASP.NET Core page.

> [!TIP]
> You can use any text editor to edit the source files for this tutorial. However, [Visual Studio Code](https://code.visualstudio.com/) provides additional features, such as debugging and Intellisense.

## Add NuGet packages

Before moving on, install some additional NuGet packages that you will use later.

- [Microsoft.Identity.Web](https://www.nuget.org/packages/Microsoft.Identity.Web/) for requesting and managing access tokens.
- [Microsoft.Identity.Web.UI](https://www.nuget.org/packages/Microsoft.Identity.Web.UI/) for sign-in and sign-out UI.
- [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/) for making calls to Microsoft Graph.

1. Run the following commands in your CLI to install the dependencies.

    ```Shell
    dotnet add package Microsoft.Identity.Web --version 0.1.1-preview
    dotnet add package Microsoft.Identity.Web.UI --version 0.1.1-preview
    dotnet add package Microsoft.Graph --version 3.3.0
    ```

## Design the app

In this section you will create the basic structure of the application.

1. Open the **./Views/Shared/_Layout.cshtml** file, and replace its entire contents with the following code to update the global layout of the app.

    :::code language="cshtml" source="../demo/GraphTutorial/Views/Shared/_Layout.cshtml" id="LayoutSnippet":::

1. Open **./wwwroot/css/site.css** and add the following code at the bottom of the file.

    :::code language="css" source="../demo/GraphTutorial/wwwroot/css/site.css" id="CssSnippet":::

1. Open the **./Views/Home/index.cshtml** file and replace its contents with the following.

    :::code language="cshtml" source="../demo/GraphTutorial/Views/Home/Index.cshtml" id="HomeIndexSnippet":::

1. Add a new file named **_LoginPartial.cshtml** in the **./Views/Shared** directory and add the following code.

    :::code language="cshtml" source="../demo/GraphTutorial/Views/Shared/_LoginPartial.cshtml" id="LoginPartialSnippet":::

1. Add a new file named **_AlertPartial.cshtml** in the **./Views/Shared** directory and add the following code.

    :::code language="cshtml" source="../demo/GraphTutorial/Views/Shared/_AlertPartial.cshtml" id="AlertPartialSnippet":::

1. Create a new directory in the **GraphTutorial** directory named **Alerts**.

1. Create a new file named **WithAlertResult.cs** in the **./Alerts** directory and add the following code.

    :::code language="csharp" source="../demo/GraphTutorial/Alerts/WithAlertResult.cs" id="WithAlertResultSnippet":::

1. Create a new file named **AlertExtensions.cs** in the **./Alerts** directory and add the following code.

    :::code language="csharp" source="../demo/GraphTutorial/Alerts/AlertExtensions.cs" id="AlertExtensionsSnippet":::

1. Save all of your changes and restart the server. Now, the app should look very different.

    ![A screenshot of the redesigned home page](./images/create-app-01.png)
