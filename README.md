# UiT INF-3770 Demo App

Simple demo app that logs a user into DIPS and searches for patients using our
FHIR Patient API. Developed as part of a lecture in Eirik Aarsands course
[INF-3770 Computer Science in Health Technology fall 2021](https://uit.no/utdanning/emner/emne?p_document_id=721866).

# Build and run

This is a [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0) project, so
you'll need to download and install it first. Then build and run the app with

```
dotnet run
```

and navigate to [https://localhost:5001](https://localhost:5001).

PS: You'll need to fill out the `dips-subscription-key` and `openid-secret`
configuration values with an API key from [Open DIPS](https://open.dips.no), and
an OpenID Connect client secret respectively. You'll need to add them to the
[appsettings.json](appsettings.json) file.
