# Refugee.Core
This is an API to help support refugees everywhere around the world.



# Setup for contribution

## Requirements
Make sure you have the latest Visual Studio and [.net 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).

Use the [issues](https://github.com/hassanhabib/Refugee.Core/issues) tracker and [Wiki](https://github.com/hassanhabib/Refugee.Core/wiki) for more information about the project and how you can contribute.

The current setup is using .net 7 Maui and we'll be building a mobile and web application.

## Steps

### Fork and clone 

You can fork the repo using [this link](https://github.com/hassanhabib/Refugee.Core/fork) or run the following with [Github CLI](https://cli.github.com/).

```shell
gh repo fork hassanhabib/Refugee.Core
```

Now that you have a fork on your project you can clone it locally using git. We'd recommend you read

Use this if you want to use git
```shell
git clone https://github.com/<your-username>/Refugee.Core
```

...or use this if you're using [Github CLI](https://cli.github.com/) in a directory of your choice.

```shell
gh repo clone hassanhabib/Refugee.Core
```


### Domain-Driven Design

If you're not used to Domain-driven design then I would suggest looking at the following resources before you get started contributing to this repo.

Books:

- Domain-Driven Design by Eric Evans - [Amazon](https://amzn.to/3yrjXYi)
- Learning Domain-Driven Design by Vladik Khonov - [Amazon](https://amzn.to/3ytQAVw)

Videos:

- [Introduction to Systems Design & Architecture](https://www.youtube.com/watch?v=eHnjdR9DvGk)
- [Domain Driven Design: The Good Parts - Jimmy Bogard](https://www.youtube.com/watch?v=U6CeaA-Phqo&t=934s)
- [What is DDD - Eric Evans - DDD Europe 2019](https://www.youtube.com/watch?v=pMuiVlnGqjk)
- [Clean Architecture with ASP.NET Core 3.0 • Jason Taylor • GOTO 2019](https://youtu.be/dK4Yb6-LxAk)
- [Clean Architecture with ASP.NET Core 3.0 - Jason Taylor - NDC Sydney 2019](https://youtu.be/5OtUm1BLmG0)

The structure of the page includes the following:

Api - This is the main application of the project.
Infrastructure - Contains the persistence of the application.
Domain - TBC will hold the core business entities of the project
WebUI - TBC will hold the web UI
MobileUI - TBC will hold the mobile interface built in Maui