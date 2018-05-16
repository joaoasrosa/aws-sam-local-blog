# C# example for SAM local

This repo presents one approach to use SAM Local in a real-world scenario. It provides an example of how to local develop an AWS Lambda function, using the same build steps locally and in the CI pipeline.

[![Build status](https://ci.appveyor.com/api/projects/status/tobss8m9jfcjmpo4?svg=true)](https://ci.appveyor.com/project/joaoasrosa/aws-sam-local-blog)
[![Build Status](https://travis-ci.org/joaoasrosa/aws-sam-local-blog.svg?branch=master)](https://travis-ci.org/joaoasrosa/aws-sam-local-blog)

## Scenario

Using AWS Lambda, I want to send notifications to a 3rd party API using SMS. The AWS Lambda is behind an AWS API Gateway, and the requests and responses are being proxy.

The 3rd party API can return the following responses:
* Invalid credentials (`Unauthorized (401)`) - If the client does not set the proper credentials using the Authorization HTTP header, the API returns an `Unauthorized` HTTP Status Code
* Insufficient Credits (`Forbidden (403)`) - If the user account doesn't have credits to use the resources, the API returns a `Forbidden` HTTP Status Code
* Ok (`OK (200)`) - If the API sends the SMS, it returns an `Ok` HTTP Status Code

## Implementation

The AWS Lambda function is implemented using .NET Core 2.0, and is using the `APIGatewayProxyRequest` and `APIGatewayProxyResponse` to be able to communicate with AWS API Gateway.

To be able to local test the AWS Lambda implementation, the project uses [SAM Local](https://github.com/awslabs/aws-sam-cli) to bootstrap all the necessary components to test it. The SAM Local is instrumented using the `template.yml` located at `./build` folder.

Given the 3rd party API doesn't provide a sandbox mode for the acceptance tests, a Testing API was created where it is possible to replicate the behavior of the real API. SAM Local uses Docker containers, therefore the Testing API is implemented using the same technology.

The Acceptance Tests are based on the expected behavior of the API (BDD FTW), where it uses a `Given/When/Then` declarative instructions to specify the steps for the tests.

## Pre-requirements

### Windows

* [.NET Core 2.0](https://www.microsoft.com/net/download/windows/build)
* [.NET Full Framework 4.6.1](https://www.microsoft.com/net/download/windows/build)
* [Docker](https://www.docker.com/docker-windows)
* [SAM Local](https://github.com/awslabs/aws-sam-cli#windows-linux-macos-with-pip-recommended)

You can follow the download instructions or use your preferred package manager.

### macOS 

* [.NET Core 2.0](https://www.microsoft.com/net/download/macos/build)
* [Mono](https://www.microsoft.com/net/download/windows/build)
* [Docker](https://www.docker.com/docker-mac)
* [SAM Local](https://github.com/awslabs/aws-sam-cli#windows-linux-macos-with-pip-recommended)

You can follow the download instructions or use your preferred package manager.

### Linux 

* [.NET Core 2.0](https://www.microsoft.com/net/download/linux/build)
* [Mono](https://www.microsoft.com/net/download/windows/build)
* [Docker](https://docs.docker.com/install/)
* [SAM Local](https://github.com/awslabs/aws-sam-cli#windows-linux-macos-with-pip-recommended)

You can follow the download instructions or use your preferred package manager.

## Run the tests locally

It uses [Cake](https://cakebuild.net) to build and run the tests in a transparent way in all supported platforms.

### Windows

Using a Powershell terminal, on the root folder of the project:

```
./build.ps1 --target=Test-Local
```

### macOS & Linux

Using a Terminal, on the root folder of the project:

```
sh ./build.sh --target=Test-Local
```

## Know issues

At the moment, there are issues with the Docker Network on Windows. We are investigating a solution to it. Please stay tuned!