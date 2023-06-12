# Container-Cat 

This is a sample project made mostly for fun and experience (not only coding, but Docker and Podman too!), so feel free to give me any feedback on my code quality. :)

  The main goal was to create a dashboard for various container host machines (both virtual and bare metal). App tries to get info from URI and, if the response is correct, parses it to 
SystemDataObject (network info, container engine info, status...). Then it gets joined with List<BaseContainer>, which is a basic container info (such as Ports, Mounts, container Id, Status, Image Id), and goes into database. The front-end part is a work in progress, and I have no idea how to code a front-end, so expect horrible results.
  
  Right now paths:
  `/systems/`
  `/systems/create`
  `/systems/delete/{id}`
  `/systems/details/{id}`
  works, as well as database CRUD operations.
  
  My main focuses are:
  - Ddd an option to return JSON obj to a front-end 
  - Code cleanup
  - Unit tests
  - Documentation
  - Release and installation instructions

  (=ↀωↀ=)
