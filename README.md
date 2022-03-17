[![.NET](https://github.com/CodeRunRepeat/Loinc.Cli/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/CodeRunRepeat/Loinc.Cli/actions/workflows/dotnet.yml)

# What is Loinc.Cli?

Loinc.Cli is a command line tool to access and use the [LOINC FHIR Terminology Server](https://loinc.org/fhir/) published
and managed by loinc.org. As described [on their site](https://loinc.org/get-started/what-loinc-is/),

> LOINC is a common language (set of identifiers, names, and codes) for identifying health measurements, observations, and documents. The overall scope of LOINC is anything you can test, measure, or observe about a patient.

## Using Loinc.Cli

Loinc.Cli is built on .NET 6 that you can download [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
Clone this repo, go to the src directory, and run

```bash
dotnet build
```

Then you can run the executable in the bin directory.

To connect to the LOINC server, you need credentials for loinc.org; you can sign up for credentials [here](https://loinc.org/join/). The CLI retrieves these credentials from two environment variables:

```text
LOINC_USER_NAME=<username>
LOINC_PASSWORD=<password>
```
