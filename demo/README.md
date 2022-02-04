# How to run the completed project

## Prerequisites

To run the completed project in this folder, you need the following:

- The [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your development machine. (**Note:** This tutorial was written with .NET Core SDK version 3.1.201. The steps in this guide may work with other versions, but that has not been tested.)
- Either a personal Microsoft account with a mailbox on Outlook.com, or a Microsoft work or school account.

If you don't have a Microsoft account, there are a couple of options to get a free account:

- You can [sign up for a new personal Microsoft account](https://signup.live.com/signup?wa=wsignin1.0&rpsnv=12&ct=1454618383&rver=6.4.6456.0&wp=MBI_SSL_SHARED&wreply=https://mail.live.com/default.aspx&id=64855&cbcxt=mai&bk=1454618383&uiflavor=web&uaid=b213a65b4fdc484382b6622b3ecaa547&mkt=E-US&lc=1033&lic=1).
- You can [sign up for the Microsoft 365 Developer Program](https://developer.microsoft.com/microsoft-365/dev-program) to get a free Office 365 subscription.

## Register a web application with the Azure Active Directory admin center

1. Open a browser and navigate to the [Azure Active Directory admin center](https://aad.portal.azure.com). Login using a **personal account** (aka: Microsoft Account) or **Work or School Account**.

1. Select **Azure Active Directory** in the left-hand navigation, then select **App registrations** under **Manage**.

    ![A screenshot of the App registrations ](../tutorial/images/aad-portal-app-registrations.png)

1. Select **New registration**. On the **Register an application** page, set the values as follows.

    - Set **Name** to `ASP.NET Core Graph Tutorial`.
    - Set **Supported account types** to **Accounts in any organizational directory and personal Microsoft accounts**.
    - Under **Redirect URI**, set the first drop-down to `Web` and set the value to `https://localhost:5001/`.

    ![A screenshot of the Register an application page](../tutorial/images/aad-register-an-app.png)

1. Select **Register**. On the **ASP.NET Core Graph Tutorial** page, copy the value of the **Application (client) ID** and save it, you will need it in the next step.

    ![A screenshot of the application ID of the new app registration](../tutorial/images/aad-application-id.png)

1. Select **Authentication** under **Manage**. Under **Redirect URIs** add a URI with the value `https://localhost:5001/signin-oidc`.

1. Set the **Logout URL** to `https://localhost:5001/signout-oidc`.

1. Locate the **Implicit grant** section and enable **ID tokens**. Select **Save**.

    ![A screenshot of the Web platform settings in the Azure portal](../tutorial/images/aad-web-platform.png)

1. Select **Certificates & secrets** under **Manage**. Select the **New client secret** button. Enter a value in **Description** and select one of the options for **Expires** and select **Add**.

    ![A screenshot of the Add a client secret dialog](../tutorial/images/aad-new-client-secret.png)

1. Copy the client secret value before you leave this page. You will need it in the next step.

    > [!IMPORTANT]
    > This client secret is never shown again, so make sure you copy it now.

    ![A screenshot of the newly added client secret](../tutorial/images/aad-copy-client-secret.png)

## Configure the sample

1. Open your command line interface (CLI) in the directory where **GraphTutorial.csproj** is located, and run the following commands, substituting `YOUR_APP_ID` with your application ID from the Azure portal, and `YOUR_APP_SECRET` with your application secret.

    ```Shell
    dotnet user-secrets init
    dotnet user-secrets set "AzureAd:ClientId" "YOUR_APP_ID"
    dotnet user-secrets set "AzureAd:ClientSecret" "YOUR_APP_SECRET"
    ```

## Run the sample

In your CLI, run the following command to start the application.

```Shell
dotnet run
```
