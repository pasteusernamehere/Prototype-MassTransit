# Prototype-MassTransit

## Environment

To bring up redis and rabbitmq issue the following command from the root of the repository:

```shell
docker compose up -d
```

To launch the service issue the following command from the service folder:
```shell
dotnet run
```

To launch the webapi issue the following command from the api folder:
```shell
dotnet run
```
## Problem
If you navigate to swagger and post to the endpoint the statemachine does not transition as expected to the AcceptedState.
If you delete the state and reissue the request the state reaches the desired AcceptedState.

