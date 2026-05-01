# Azure DevOps Test Plans Pipeline

This repository includes `azure-pipelines.yml` for running the Playwright/xUnit tests through Azure DevOps Test Plans.

## What it supports

- **Run all associated automated tests on a schedule**: the YAML schedule runs nightly from `master`.
- **Rerun only failed tests from Azure Test Plans**: when you use Azure Test Plans to rerun failed automated tests, Azure DevOps supplies `$(test.RunId)`. The pipeline detects that variable and uses `VSTest@3` with `testSelector: testRun`, so only the tests selected from that Test Run are executed.
- **Publish results back to the Test Plan**: `VSTest@3` uses Azure Test Plans selection (`testPlan` or `testRun`), so associated test case results are written back to the Test Plan/Test Suite/Test Case history.

## Required Azure DevOps configuration

Create these pipeline variables, or map them from a variable group:

| Variable | Purpose |
| --- | --- |
| `ADO_TEST_PLAN_ID` | Azure Test Plan id for scheduled/manual all-test runs |
| `ADO_TEST_SUITE_ID` | Azure Test Suite id that contains the automated tests |
| `ADO_TEST_CONFIGURATION_ID` | Azure Test Configuration id used by the suite |
| `TestUsers__Admin__Username` | Admin username used by the UI tests |
| `TestUsers__Admin__Password` | Admin password used by the UI tests; mark secret |
| `TestUsers__TestUser1__Username` | Invalid-login username used by the UI tests |
| `TestUsers__TestUser1__Password` | Invalid-login password used by the UI tests; mark secret |

## Test case association checklist

Each Azure DevOps test case should be associated to the matching automated test method. For xUnit, use the fully qualified test name, for example:

```text
Automation.UiTests.Tests.LoginTests.Login001_WithValidCredentials_ShouldSucceed
```

The test storage is the compiled test assembly:

```text
Automation.UiTests.dll
```

Once the associations are in place:

1. Queue the pipeline manually or let the nightly schedule run to execute the whole suite.
2. If test cases fail in the Test Plan, select the failed tests in Azure Test Plans and choose the run/rerun option that uses this automated pipeline.
3. Azure DevOps passes `$(test.RunId)` to the pipeline, and the `Run selected failed tests from Azure Test Plans test run` step executes only those tests.

## Notes

- The pipeline uses `windows-latest` because `VSTest@3` is the Azure DevOps task that integrates directly with Test Plans and test case history.
- The solution targets `.NET 10`, so the pipeline installs `10.0.x` with `UseDotNet@2`.
- The pipeline installs the Chromium Playwright browser after the build using the generated `playwright.ps1` script.
