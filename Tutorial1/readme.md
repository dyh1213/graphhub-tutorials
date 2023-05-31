# GaphHub Tutorial 1

Welcome! GaphHub aims to create an open platform where knowledge graphs can be discovered, built, and collaborated on.

This repository contains the code developed throughout the GraphHub technical tutorial. If you're curious about the creation process, you can check out the tutorial video [here](https://youtu.be/JJBO2r0F1Ug).

For more insights into the development of GraphHub, including the motivations behind it, the development process, and future plans, consider following my vlog, PM DevLog. Below you'll find some noteworthy episodes:

- [EP1: PM's DevLog - I Want to Create an IMDB for EVERYTHING](https://youtu.be/JJBO2r0F1Ug)
- [EP2: PM's DevLog - ChatGPT and Product Development: Crafting and Refining Your Ideas](https://youtu.be/9syv7UbiXDU)

## Tutorial Overview

### 1. Creating a blazor server app
#### Prerequisites 
- Visual Studio 2022 (I use for mac)

#### Steps 
1. Create a new project from “Blazor WebAssembly App” template.
2. Target framework .NET 7.0.
3. Select ASP.Net Core Hosted.
4. Select Use git version control.

### 2. Connecting to GitHub
#### Prerequisite
- GitHub Account

#### Steps
1. Create a new repository.
2. Open the project terminal.
3. Add the repository as a remote using the command `git remote add origin {your repo path}`.
4. Set the main branch with `git branch -M main`.
5. Push to the repository with `git push -u origin main`.

### 3. Connecting to Heroku
#### Prerequisite
- Heroku Account

#### Steps
1. Create a new app.
2. Deploy using GitHub as the deployment method.
3. Add the buildpack `https://github.com/jincod/dotnetcore-buildpack` in Settings.
4. Add a config variable `PROJECT_FILE` with the value of your server csproj file.
5. Connect your GitHub repository and select either automatic deployment with each update or manual deployment.

### 4. Cleaning Template
Follow the instructions in the video and copy the source code to create a beautiful MudBlazor landing page.

### 5. Connecting MudBlazor UI Component library
#### Steps
1. Install MudBlazor NuGet package.
2. Add `@using mudblazor` to the `_imports.razor` file.
3. Register the service in `program.cs`.
4. Add MudBlazor stylesheets and scripts.
5. Update `mainlayout` and `navmenu` files to use MudBlazor components.

### 6. Building the landing page
Follow the instructions in the video and copy the source code.

### 7. Add a subscribe form and send emails to MailChimp 
#### Steps
1. Add a new MailChimp controller that retrieves an email and calls the MailChimp API.
2. Add the API key and list ID to the app settings file.
3. Call the MailChimp API `lists/{listid}/members` endpoint.

Happy coding! 🚀
