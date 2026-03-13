# UiTests Project

## Overview

This project contains UI automated tests using **Playwright**, **xUnit v3**, and **Automation.Framework** services.  

Tests are designed following **best practices**:

- One **browser per test class** (`TestFixture`)  
- One **isolated browser context per test** (`BaseTest`)  
- Automatic **screenshot, log, and HTML report generation** on test failure  
- Parallel execution support configurable via `xunit.runner.json`  

---

## Project Structure

1. **TestFixture**  
   - Runs once per test class  
   - Initializes **PlaywrightManager** (launches browser)  
   - Provides **browser context** and **page** for each test method  

2. **BaseTest**  
   - Runs per test method  
   - Creates **isolated context and page**  
   - Automatically navigates to **BaseUrl** from `appsettings.json`  
   - Wraps the test in a try/catch to capture exceptions  
   - On test failure:  
     - Takes a **screenshot**  
     - Writes **log file**  
     - Generates **HTML report** referencing screenshot  

3. **Page Objects**  
   - Encapsulate page-specific actions  
   - Can access `LoggerManager` from BaseTest to write logs  

4. **Parallel Execution**  
   - Controlled via `xunit.runner.json`:

```json
{
  "parallelizeTestCollections": true,
  "maxParallelThreads": 3
}

Writing New Tests

Create a new test class in Tests folder
Inherit from BaseTest:
