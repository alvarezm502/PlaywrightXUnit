# Automation Solution

## Overview

This solution contains automation frameworks and test projects for UI testing using Playwright, .NET 10 and xUnit.V3 
It is designed for **parallel execution**, **isolated browser contexts**, and **easy artifact capture** on test failures.

## Prerequisites
Make sure the following are installed:
1. Visual Studio 2026
2. Powershell 7
3. .NET
4. Node JS

## Powershell 7 setup in Visual Studio
Step 1: Get pwsh path
Open Terminal: View > Terminal
Run the following command to find location of Powershell 7. 
```bash
Get-Command pwsh | Select-Object Source
```
Will output something like
```
C:\Users\YOURNAME\AppData\Local\Microsoft\WindowsApps\pwsh.exe
```

Copy output (Highlight all + right click)

Step 2: Add it to Visual Studio powershell options
Navigate to Tools > Options > Environment > Terminal
Click Add and fill in:
Name: Developer Powershell 7
Shell location: (Paste path from above)

Save and Set as Default
The newly created option should now be available in the dropdown found in the termnal view top left corner

## Environment Setup
Run the following in the terminal to verify everything is ready:

1. .NET SDK
Requires .NET 10 or later. You can check your installed version with:
   ```bash
   dotnet --version
   ```

2. Node.js (for Playwright)
Recommended: v22.22.0 or latest
   ```bash
   node --version
   ```

3. Restore Dependencies  
   ```bash
   dotnet restore
   ```

4. Build Solution
   ```bash
   dotnet build
   ```

5. Navigate to UI Test Project
   ```bash
   cd Automation.UiTests
   ```

6. Install playwright browsers
   ```bash 
   pwsh bin/Debug/net10.0/playwright.ps1 install
   ```

## Configuration
1. User Secrets Setup
This is a sample configuration for this sample project.  
```bash
dotnet user-secrets set "TestUsers:Admin:Username" "tomsmith"
dotnet user-secrets set "TestUsers:Admin:Password" "SuperSecretPassword!"
dotnet user-secrets set "TestUsers:TestUser1:Username" "TestUser1"
dotnet user-secrets set "TestUsers:TestUser1:Password" "badpassword"
```

For CI or local shell runs, the same values can be supplied with environment variables:
```bash
export TestUsers__Admin__Username="tomsmith"
export TestUsers__Admin__Password="SuperSecretPassword!"
export TestUsers__TestUser1__Username="TestUser1"
export TestUsers__TestUser1__Password="badpassword"
```

2. Application Settings (Sample project is already configured)
File: Automation.UiTests/appsettings.json
```json
{
  "BaseUrl": "https://example.com",
  "DriverType": "chromium",
  "Headless": false,
  "Timeout": 5000
}
```

3. Parallel Execution (Sample project is configured to run 3 in parallel)
Configure parallel tests settings in:
- Automation.UiTests/xunit.runner.json

3. Test Artifacts - only for failed tests
 - TestResults
	- Screenshots
	- Logs
	- Reports

## Test Execution
Option 1: Visual Studio (Test Explorer)
1. Go to View > Test Explorer
2. Select a test class or a method
3. Click Run or Debug. 

Option 2: Command Line 
   ```bash 
   dotnet test
   ```
