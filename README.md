# Cloud-Native E-Commerce Microservices Ecosystem

An event-driven e-commerce platform built using **.NET 10** and structured around **Microservices Architecture** principles.

## 🚀 Architectural Blueprint & Patterns

*   **Asynchronous Event-Driven Architecture (EDA):** Leverages **MassTransit** over **RabbitMQ** to achieve loose coupling and high resiliency across microservices.
*   **Distributed Transactions (Saga Pattern):** Orchestrates complex multi-service workflows (such as order checkout and processing) across independent databases with full state-machine error handling and compensating transactions.
*   **Data Reliability (Transactional Outbox Pattern):** Guarantees at-least-once message delivery and atomic consistency, preventing event data loss during network or message broker failures.
*   **High-Performance Inter-Service Communication (gRPC):** Utilizes **gRPC** for synchronous, low-latency, strongly-typed internal data fetching (e.g., pulling real-time pricing discounts from the Basket service).
*   **Containerization:** Fully containerized using **Docker** and multi-stage `Dockerfiles` for optimized image sizes and isolated virtual network security.

---

## 🏢 System Topology (Microservices Overview)

1.  **Catalog Microservice:** Formed around **Clean Architecture / CQRS (MediatR)** principles. Manages product information using a high-performance NoSQL data layer.
2.  **Basket Microservice:** Formed around **Clean Architecture / CQRS (MediatR)** principles. Handles real-time user shopping carts, utilizing gRPC channels to fetch promotional data dynamically.
3.  **Ordering Microservice:** Formed around **Clean Architecture / plain CQRS (without MediatR)** principles to separate read/write workloads and process background outbox queues.
4.  **Payment Microservice:** Simulates transactional payment capture, broadcasting event successes or triggers for Saga rollbacks.
5.  **Identity Microservice 🚧 (In Progress):** Establishes the global token-based security boundary to protect endpoint authentication.

---

## 🛠️ Tech Stack & Tooling

*   **Framework:** .NET 10 (C#)
*   **Communication:** gRPC, RabbitMQ (MassTransit), HTTP Minimal APIs
*   **Data Access:** Entity Framework Core (Fluent API), [Dapper / MongoDB / PostgreSQL / SQL Server]
*   **Infrastructure:** Docker, Docker Compose, Scrutor (Decorator Pattern)
*   **Tooling:** SQL Server Management Studio (SSMS), Swagger

---

## 🚧 Project Milestones & Current Focus

- [x] Architect independent microservice boundaries (Catalog, Basket, Ordering, Payment)
- [x] Configure MassTransit & RabbitMQ event bus infrastructure
- [x] Implement State-Machine Saga workflow for Order Processing
- [x] Implement Background Dispatcher with Transactional Outbox and custom retry guards
- [x] Establish synchronous gRPC protocol channels
- [x] Construct multi-file Docker Compose infrastructure overrides
- [/] **Current Focus:** Completing token-based authentication schemas inside the Identity Microservice
- [ ] Next Horizon: Migrating orchestration to Kubernetes (AKS) and building automated Azure DevOps CI/CD pipelines

---

## ⚙️ Local Development Setup

### Prerequisites
*   [.NET 10 SDK](https://microsoft.com)
*   [Docker Desktop](https://docker.com)

### Spinning Up the Ecosystem
To boot up the database engines, message brokers, and core API services simultaneously, run the following command from the repository root:

```bash
docker-compose up --build
```

To run **only** the underlying infrastructure dependencies (e.g., SQL Server, Postgres, RabbitMQ) to perform migrations or local debugging:

```bash
docker-compose up -d sqldata discountdb rabbitmq
```
