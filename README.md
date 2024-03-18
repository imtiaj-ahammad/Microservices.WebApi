# Microservices.WebApi


A simple microsoft application for demonstrating various concepts including Microsoft Architecture in ASP>NET Core, API Gateways, Ocelot, Ocelot Configuration and Routing etc.



     Monolith Architecture 
     - Traditional and widely used architectural pattern while developing applications
     - It is designed in a way that the entire application components is ultimately a single piece, no matter how much try to de-couple them by using Patterns and Onion/Hexagonal architechture
     - All the services would be tightly coupled with the solution
     - Any small to mid scaled applicatons would do just fine with this architechture, but when you suddenly start to scale up even further, we would have to make a compromises as well as face certain downtimes while deploying new versions/fixes
     Monolith Architecture Components: 
                - USER-INTERFACE 
                - BUSINESS
                - DATA ACCESS
                - DB
---

    Microservice Architecture
    - An architecture wehre the application itself is divided into various components, with each component serving a particular purpose, collectively being called a microservice
    - Components are not dependent on the application itself
    - Each of these components is literally and physically independent
    - WIth this awesome separation, we can have dedicated Databases for each component
    - Each microservice can be deployed into separate hosts/servers
    Microservice Architecture:
                - Client
                - API Gateway
                - (MicroService1+DB)[hosted on server1] + (MicroService2+DB)[hosted on server2] + (MicroService3+DB)[hosted on server3]
---

We would build a ASP.NET CORE API solution for an eCommerce Application. We would consider 2 components products and customers as individual services here. And a gateway application so that it can route the api requests to the necessary API service from one single service.
This API Gateway will be placed right between the client and microservices.

1. Create a blank Solution named "Microservices.WWebApi"
2. On the solution, create a folder named "Microservices", this is where we would add all our microservices, in our case, the Product service and the customer service
3. On the solution, create a new Asp.net core web application with empty template named "Gateway.WebApi"
4. On the Microservices folder, create 2 asp.net core web api projct named "Product.Microservice" and "Customer.Microservice"
5. For both the api services, make a sample controller such as ProductController and CustomerController with all the CRUD APIs
6. Now its the turn to make to web application as an gateway service, Add "Ocelot" for the "GateWay.WebApi" project

    ```
    Install-Package Ocelot
    ```

7. Configure the program file to setum ocelot
    ```
    // Configure Ocelot
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
    builder.Services.AddOcelot(builder.Configuration);
    ```

    ```
    // Use Ocelot middleware
    app.UseOcelot().Wait();
    ```
8. From the above point, you will see there is a json file name as the configuration file for ocelot. Lets add the json file into the Geteway.WebApi project directory and configure the routes into this file

    **DownstreamPathTemplate**: denotes the route of the actual endpoint in the Microservice.

    **DownstreamScheme**: This is the scheme of the Microservice, here it is HTTPS

    **DownstreamHostAndPorts**: It defines the location of the Microservice. We will add the host and port number 
    here.

    **UpstreamPathTemplate**: It is the path at which the client will request the Ocelot API Gateway.

    **UpstreamHttpMethod**: It is the supported HTTP Method to the API Gateway. Based on the Incoming Method, Ocelot sends a similar HTTP method request to the microservice as well.
    ```
    {
    "Routes": [
        {
        "DownstreamPathTemplate": "/api/product",
        "DownstreamScheme": "https",
        "DownstreamHostAndPorts": [
            {
            "Host": "localhost",
            "Port": 7085
            }
        ],
        "UpstreamPathTemplate": "/gateway/product",
        "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
        },
        {
        "DownstreamPathTemplate": "/api/customer",
        "DownstreamScheme": "https",
        "DownstreamHostAndPorts": [
            {
            "Host": "localhost",
            "Port": 7132
            }
        ],
        "UpstreamPathTemplate": "/gateway/customer",
        "UpstreamHttpMethod": [ "POST", "PUT", "GET" ]
        }
    ]
    }
    ```
    Here you can see that, the host and port for **product.microservice** has been set in the first route from its own launchsettings.json for **"/gateway/product"** and for **customer.microservice** it has been set in the second route for **"/gateway/customer"**
9. Now go to properties of the solution and run all the projects at the same time and check if gateway works with the configured routes
    - https://localhost:7273/gateway/product
    - https://localhost:7273/gateway/customer


#### **Reference**: https://codewithmukesh.com/blog/microservice-architecture-in-aspnet-core/