# Automation Solution

## Overview

This solution contains automation frameworks and test projects for UI and API testing.  
It is designed for **parallel execution**, **isolated browser contexts**, and **easy artifact capture** on test failures.

## Prerequisites

Before running tests, ensure the following:

1. **.NET 10 SDK** or later installed  
   ```bash
   dotnet --version
2. Node.js (for Playwright)  
   ```bash
   node --version
3. Dependencies installed  
   ```bash
   dotnet restore
   dotnet build
   dotnet playwright install
   ```

## Configuration
1. UiTets/appsettings.json contains configuration settings for the tests, such as:
```json
{
  "BaseUrl": "https://example.com",
  "DriverType": "chromium",
  "Headless": false,
  "Timeout": 5000
}
```

2. Parallel Execution
	- In UiTests/xunit.runner.json, you can configure parallel execution settings

3. Test Artifacts - only for failed tests
 - TestResults
	- Screenshots
	- Logs
	- Reports

Run tests from CLI
```bash
dotnet test
```