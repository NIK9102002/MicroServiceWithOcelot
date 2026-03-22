# 🛒 E-Commerce Microservices Application

![.NET](https://img.shields.io/badge/.NET-8-blue)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue)
![Architecture](https://img.shields.io/badge/Microservices-Ready-green)

------------------------------------------------------------------------

## 📌 Overview

A **production-style Microservices E-Commerce system** built using .NET
8.\
It demonstrates real-world backend architecture including API Gateway,
Docker, resilience patterns, and logging.

------------------------------------------------------------------------

## 🖼️ Architecture Diagram

            Client (Browser / Postman)
                        │
                        ▼
             API Gateway (Ocelot)
                        │
         ┌──────────────┼──────────────┐
         ▼              ▼              ▼
    Product Service  Order Service  Auth Service
         │              │              │
         └──────────────┴──────────────┘
                        │
                  SQL Server (Docker)

------------------------------------------------------------------------

## 🚀 Features

-   Microservices Architecture
-   API Gateway Routing
-   Retry & Resilience (Polly)
-   Logging with Serilog
-   Dockerized Services
-   Service-to-Service Communication

------------------------------------------------------------------------

## 🧰 Tech Stack

-   .NET 8 Web API
-   Ocelot API Gateway
-   Entity Framework Core
-   SQL Server (Docker)
-   Polly
-   Serilog
-   Docker & Docker Compose

------------------------------------------------------------------------

## ▶️ Run the Project

### 1. Clone

    git clone <your-repo-url>
    cd <project-folder>

### 2. Run

    docker-compose up --build

------------------------------------------------------------------------

## 🌐 Access URLs

  Service       URL
  ------------- -----------------------
  API Gateway   http://localhost:5003
  Product API   http://localhost:5000
  Order API     http://localhost:5001
  Auth API      http://localhost:5002

------------------------------------------------------------------------

## 🧪 API Testing (Postman)

### Sample Requests

**Get Products**

    GET http://localhost:5003/api/products

**Get Order Details**

    GET http://localhost:5003/api/orders/1

**Login**

    POST http://localhost:5003/api/authentication/login

------------------------------------------------------------------------

## 🐳 Docker Understanding

  Context          URL Format
  ---------------- --------------------------
  Inside Docker    http://service-name:8080
  Outside Docker   http://localhost:port

------------------------------------------------------------------------

## 🔁 Resilience Strategy

-   Retry using Polly
-   Handles:
    -   Timeouts
    -   Network failures
    -   Transient errors

------------------------------------------------------------------------

## 📊 Logging

View logs:

    docker logs orderservice

------------------------------------------------------------------------

## ⚠️ Common Issues

  Issue                   Solution
  ----------------------- ---------------------------------------
  Service not reachable   Use service-name instead of localhost
  Timeout error           Check dependent services
  Gateway not routing     Fix Ocelot config

------------------------------------------------------------------------

## 🚀 Future Improvements

-   Circuit Breaker
-   Distributed Tracing
-   Kubernetes Deployment
-   Centralized Logging (ELK / Seq)

------------------------------------------------------------------------

## 👨‍💻 Author

**Nikhil Badhe**

------------------------------------------------------------------------

## ⭐ Support

If you like this project, give it a ⭐ on GitHub!
