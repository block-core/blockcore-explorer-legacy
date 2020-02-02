# Blockcore Explorer | Multi-Chain Block Explorer



## Build and Setup

When you fork this repo, you must ensure you add your own personal secret for DockerHub to your fork.

Do this by navigating to the [/settings/secrets](/settings/secrets) page on your GitHub project.

Create a new secret named "DockerHubSecret" and fill in your secret (password) from DockerHub.

## Docker

Manual build from the source directory:

```sh
docker build -t blockcoreexplorer .
```

```sh
docker run -p 9901:9901 --name cityexplorer blockcoreexplorer:latest
```

