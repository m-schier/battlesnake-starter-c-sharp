# C# starter snake for BattleSnake

*This starter snake is currently unofficial*

This is a basic [BattleSnake AI](http://battlesnake.io) written in C#.

Visit [https://docs.battlesnake.io](https://docs.battlesnake.io) 
for API documentation and instructions for running your AI.

## Requirements
Install [.NET Core SDK](https://dotnet.microsoft.com/download) to build this project.

## Brief overview
The `StarterSnake.Startup` class starts a new server using the `StarterSnake.StarterController` class to control snake
movement. This movement is random and will lead to rather soon death. You may either implement your own controller
inheriting `StarterSnake.Service.ISnakeServiceController` or improve the existing controller directly.

Generally there should be no need to modify code inside the `StarterSnake.Service` and `StarterSnake.ApiModel` namespaces.

## Running the snake
By default the snake runs a server on port 5050. You may change this through the environment variable `PORT`.

```bash
dotnet run -c Release
```

## Deploying to Heroku

1) Create a new Heroku app:
```
heroku create [APP_NAME]
```

2) Set the correct heroku build pack for .NET Core
```
heroku buildpacks:set jincod/dotnetcore
```

3) Deploy code to Heroku servers:
```
git push heroku master
```

You may have to register your app name as a git remote if git does not recognize 'heroku'
```
heroku git:remote -a [APP_NAME]
```

4) Open Heroku app in browser:
```
heroku open
```
or visit [http://APP_NAME.herokuapp.com](http://APP_NAME.herokuapp.com).

5) View server logs with the `heroku logs` command:
```
heroku logs --tail
```