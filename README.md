# C# example for SAM local

This repo presents one approach to use SAM Local in a real world scenario. It provides an example in how to local develop a AWS Lambda function, using the same build steps locally and in the CI pipeline.

[![Build status](https://ci.appveyor.com/api/projects/status/tobss8m9jfcjmpo4?svg=true)](https://ci.appveyor.com/project/joaoasrosa/aws-sam-local-blog)
[![Build Status](https://travis-ci.org/joaoasrosa/aws-sam-local-blog.svg?branch=master)](https://travis-ci.org/joaoasrosa/aws-sam-local-blog)

## Scenario

Using AWS Lambda, I want to send notifications to a 3rd party API using SMS. The AWS Lambda is behind a API Gatweay, and the requests and responses and being proxy.

The 3rd party API can return the following responses:
* Invalid credentials (Unauthorized (401)) - If the client do not set the proper credentials using the Authorization HTTP header, the API returns an Unauthorized HTTP Status Code
* Insufficient Credits (Forbidden (403)) - If the user account doesn't have credits to use the resources, the API returns an Forbidden HTTP Status Code
* Ok (OK (200)) - If the API sends the SMS, it returns an Ok HTTP Status Code