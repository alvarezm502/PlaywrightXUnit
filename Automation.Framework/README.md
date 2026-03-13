Automation.Framework is a reusable automation foundation built with:
    1. Playwright (.NET)
    2. Dependency Injection
    3. Serilog logging
    4. xUnit-compatible architecture

The purpose of this project is to provide a shared automation engine that can be used by multiple test projects such as:
    1. UI Tests
    2. API Tests
    3. SpecFlow Tests

This framework centralizes browser management, configuration, logging, and common automation utilities so that test projects remain clean, simple, and maintainable.
The framework is designed with the following principles:    
    1. Separation of concerns
        - Framework infrastructure is separated from test implementations.

    2. Reusability
        - Multiple test projects can reference the same framework.

    3. Test isolation
        - Each test receives its own Playwright browser context.

    4. Scalability
        - The architecture supports future additions such as API testing, SpecFlow BDD tests, and CI/CD reporting.

    5. Maintainability
        - Core functionality is centralized to avoid duplicated code across test suites.

Project Structure
Automation.Framework
│
└── Core
    ├── PlaywrightManager.cs
    ├── BrowserContextManager.cs
    ├── ServiceConfigurator.cs
    ├── LoggerManager.cs
    ├── ScreenshotService.cs
    ├── TestSession.cs
    └── TestSettings.cs


CORE COMPONENTS

PlaywrightManager
Manages the lifecycle of Playwright and the browser instance.

Responsibilities:
1. Initialize Playwright
2. Launch the configured browser
3. Provide browser access to the framework
4. Dispose browser resources when execution finishes
The manager does not create pages or contexts.
Those are created per test to ensure proper isolation.

BrowserContextFactory
Creates Playwright browser contexts and pages for tests.

Responsibilities:
1. Retrieve the browser instance from PlaywrightManager
2. Create isolated browser contexts
3. Configure default timeouts
4. Provide new pages for test execution
Each test receives its own BrowserContext to prevent shared state.

ServiceConfigurator
Bootstraps the entire framework.

Responsibilities:
1. Load configuration from appsettings.json
2. Configure logging
3. Register framework services
4. Build and return the Dependency Injection container
Test projects call this once to initialize the framework.
Example:
var services = ServiceConfigurator.Configure();

LoggerManager
Provides access to loggers used throughout the framework.

Responsibilities:
1. Create typed loggers for services and tests
2. Maintain consistent logging across the framework
3. Integrate with the configured logging provider (Serilog)

ScreenshotService
Captures screenshots during test execution.
This is primarily used during test teardown when a test fails.

Features:
1. Saves screenshots to the test-results/screenshots directory
2. Generates unique file names
3. Supports full page screenshots

TestSession
Represents the runtime state of a test.
It bundles the most commonly used objects needed during test execution.

Contents include:
1. Playwright Page
2. BrowserContext
3. Framework settings
4. Test logger
This allows page objects and helpers to receive a single dependency rather than multiple constructor parameters.

Example:
LoginPage(TestSession session)

TestSettings
Represents framework configuration loaded from appsettings.json.
Example configuration:
{
  "TestSettings": {
    "Browser": "chromium",
    "Headless": true,
    "Timeout": 30000
  }
}
Configuration files are stored in test projects, not the framework.

Dependency Injection
The framework uses Microsoft.Extensions.DependencyInjection.
All core services are registered in ServiceConfigurator.

Registered services include:
1. PlaywrightManager
2. BrowserContextFactory
3. LoggerManager
4. ScreenshotService
Test projects obtain services from the ServiceProvider.

Screenshot Output
Screenshots are saved to:
testResults/screenshots/

Example file:
LoginTest_20260309_221530.png

These images can be attached to HTML reports or CI pipelines.