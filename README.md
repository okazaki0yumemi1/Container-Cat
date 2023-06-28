# Container-Cat 

This is a sample project made mostly for fun and experience (not only coding, but Docker and Podman too!), so feel free to give me any feedback on my code quality. :)

  The main goal was to create a dashboard for various container host machines (both virtual and bare metal). App tries to get info from URI and, if the response is correct, parses it to 
SystemDataObject (network info, container engine info, status...). Then it gets joined with List<BaseContainer>, which is a basic container info (such as Ports, Mounts, container Id, Status, Image Id), and goes into database. The front-end part is a work in progress, and I have no idea how to code a front-end, so expect horrible results.
  
## What is it?
This is supposed to be a simple monitoring tool for Docker and Podman systems. Learning and implementing real-time monitoring will delay the release though, so right now it can connect to Docker socket via hostname (or IPv4 and port) and gather info on containers: ID, image, mapped ports.

# How to use this app?

## General details
Right now this app can only connect to docker.sock without any security measures. The best (and easiest) way to try it is to create Linux virtual machine, get Docker and install some containers. Then you have to:
1. "Open" Docker socket,
2. Reload Docker service inside your VM,
3. Enter the IP of your guest machine. The default port is 2375, but IIRC you can change it.
Then navigate to /systems and click "Add new". Enter the guest IP or IP:2375 (or any port leading to your guest machine) and click "Add". You will be redirected to /systems page with new info. 
"Update" link, well, updates info on Docker host and its containers. You can also delete host machine and all it's containers by clicking on "Delete" link.

## How to set up hosts?

You can check this instruction (it's good): https://medium.com/@ssmak/how-to-enable-docker-remote-api-on-docker-host-7b73bd3278c6
But basically, you have to edit

`/lib/systemd/system/docker.service`

by adding 

`-H=tcp://0.0.0.0:2375` (listen to any addresses on port 2375, or `-H=tcp://{ip address of your machine}:2375` to access local containers) to the end of the starting with `ExecStart`. Save the file, then reload config files with `systemctl daemon-reload` and docker.service by `systemctl restart docker.service`.

Start the container by running:

`docker run --rm -p 8001:80 okazakiyumemi/containercat -d`

The web page will be available on `localhost:8001`.

# Technical details
The project itself is written on C# (.NET 7).
Things I used in this project:
1. Project template is ASP .NET Core MVC, which was chosen mostly because of simplicity of my project. 
2. Relational DB is SQLite, ORM system is Entity Framework Core. 
3. Simple front-end is written on Razor Pages and it's pretty basic. If you want to write your own front-end, mail me - I'd love to help you on that task.
Code quality of the backend part of my app is acceptable from my point of view, but some parts of it *will* be rewritten anew. 
I will also try to containerize my app with both **Docker** and **Podman**. 
  
  
### Right now paths:
  `/`
  `/systems/`
  `/systems/create`
  `/systems/delete/{id}`
  `/systems/details/{id}`
  works, as well as database CRUD operations.
  
### My main focuses right now are:
  - Code cleanup
  - Unit tests
  - Documentation
  - Release and installation instructions

  (=ↀωↀ=)
