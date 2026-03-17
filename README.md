# Automation Solution

## Overview

This solution contains automation frameworks and test projects for UI and API testing.  
It is designed for **parallel execution**, **isolated browser contexts**, and **easy artifact capture** on test failures.

## Prerequisites

Before running tests, ensure the following (will need terminal open):
Run each command and me sure you have the required dependency versions. Install if not available

1. **.NET 10 SDK** or later installed (checks version only) 
   ```bash
   dotnet --version
2. Node.js (for Playwright)  v22.22.0 or latest
   ```bash
   node --version
3. Dependencies installed  
   ```bash
   dotnet restore
   ```
4. Build Solution
   ```bash
   dotnet build
   ```
5. cd into Automation.UiTests
   ```bash
   cd Automation.UiTests
   ```
6. Install playwright browsers
   ```bash 
   pwsh bin/Debug/net10.0/playwright.ps1 install
   ```
## Test Execution
1. Run tests from Test Explorer: If not open View > Test Explorer. Select a test class or a test method and click play/debu
2. Run tests from CLI 
   ```bash 
   dotnet tests
   ```
1. 
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