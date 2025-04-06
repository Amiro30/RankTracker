# RankTracker

This is a full-stack application to track a website's position in Google and Bing search results for a given keyword.

## Overview

- **Backend**: ASP.NET Core Web API (`RankTracker.Api`)
- **Frontend**: React with Vite (`clientapp.client`)
- **Google**: Uses a real saved mock file (`googleLandRegistrySearch.html`) because scraping live Google results is against their ToS.
- **Bing**: Fetches real results directly from Bing.

## Requirements

- .NET 8 SDK
- Node.js (v18+ recommended)
- npm or yarn


## 🔧 How to Run
```
git clone https://github.com/Amiro30/RankTracker
```
### Option 1 — Run via CLI

1. **Restore packages and build the backend:**
```
cd RankTracker
dotnet restore
dotnet build
```
2. Start the backend API: 
```
dotnet run --project RankTracker.Api
```
3. Start the frontend (React app):
```
cd clientapp.client
npm install
npm run dev
```
go to  http://localhost:54211/


Database & Persistence
This app uses Entity Framework Core with SQL Server Express LocalDB (or your default SQL Server instance).

The schema is created using migrations, and the initial migration has already been committed to the repository.

Applying the Migration
If the database doesn't exist, it will be created automatically when the application starts.

Alternatively, to apply the migration manually:
```
cd RankTracker
dotnet ef database update --project RankTracker.DataAccess
```
Make sure the dotnet-ef tool is installed globally:
```
dotnet tool install --global dotnet-ef
```
<img src="https://github.com/user-attachments/assets/edcb98aa-bad8-45f1-8a16-05d50cd843c6" width="400" height="600"/>


### Option 2 — Run via Visual Studio
Open the RankTracker.sln solution in Visual Studio.

In the Solution Explorer, right-click the solution and go to: Set Startup Projects → Multiple startup projects.

Set both:
```
RankTracker.Api — Start

clientapp.client — Start (if you want to run the frontend via VS Code extension or task)

Press F5 or click Start.
```

Notes
Google results are parsed from a static HTML file due to restrictions on scraping Google Search directly.
Bing results are fetched live using HTTP requests.

The app includes a parsing engine that analyzes the HTML and finds target URL positions among the top 100 results.

#### This project is for demo purpose only.
