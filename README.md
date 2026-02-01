# DreamNumbers

DreamNumbers is an open-source, cross-platform analytics and simulation engine designed to analyze lottery draw history, compute statistical insights, and generate probability-based number combinations using multiple simulation strategies.

The project is built with a clean, modular architecture and delivered through a hybrid ecosystem:
- Blazor WebApp
- .NET MAUI Hybrid App
- Shared Razor UI Library
- Background Scheduler for automatic draw updates

DreamNumbers aims to be a modern, extensible, and community-driven platform for statistical exploration and predictive simulation.

---

## Features

### Statistical Engine
- Computes number absence streaks
- Calculates frequency across 20, 40, and 60-draw intervals
- Generates normalized probability estimations
- Provides ranking and trend indicators

### Simulation Engine
- Weighted random combination generator
- Multiple pluggable strategies:
  - Absence Only
  - Frequency Only
  - Hybrid
  - Exponential Absence
- Extensible architecture for future strategies (Monte Carlo, GA, ML)

### Automatic Draw Updates
- Background scheduled task using Marquitos.Schedulers
- Periodic fetch and parsing of draw results
- Repository auto-sync

### UI Components (Blazor)
- Dashboard with charts and statistics
- Draw history viewer with filtering
- Simulation interface with strategy selection
- Shared Razor Class Library for reuse across Web and MAUI

### Cross-Platform Delivery
- Web (Blazor Server)
- Desktop and Mobile (MAUI Blazor Hybrid)
- Shared codebase across all platforms

---

## Architecture Overview


### Domain Layer
Contains the fundamental models:
- Draw
- NumberStatistics
- SimulationRequest
- SimulationResult

### Application Layer
Implements:
- Statistics service
- Simulation engine
- Simulation strategies
- Draw update service abstractions
- Repository interfaces

### Infrastructure Layer
Provides:
- EF Core database (SQLite)
- Draw repository
- Draw update service
- Scheduler integration (Marquitos.Schedulers)
- HTTP client and parsing logic

### UI Layer
Reusable Razor components:
- Dashboard
- DrawHistory
- Simulation
- Layouts and shared UI elements

---

## Technology Stack

- .NET 10
- Blazor Server
- .NET MAUI Blazor Hybrid
- Entity Framework Core (SQLite)
- Marquitos.Schedulers
- Chart.js
- Dependency Injection
- Clean Architecture principles

---

## Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio 2026 or VS Code
- MAUI workload installed (for mobile/desktop)
- SQLite (included by default)

### Clone the repository

```bash
git clone https://github.com/MarquitosPT/DreamNumbers.git
cd DreamNumbers

```

### Run the WebApp

```bash
cd src/DreamNumbers.Web
dotnet run

```

### Run the MAUI App
Open the solution in Visual Studio and select:

DreamNumbers.Maui -> Run on Windows / Android / iO


---

## Roadmap

### v0.1 - Core Domain and Statistics  
### v0.2 - Simulation Engine and Strategies  
### v0.3 - Dashboard UI  
### v0.4 - Draw History UI  
### v0.5 - Scheduler Integration  
### v0.6 - MAUI Hybrid App  
### v1.0 - Public Release  

Future enhancements:
- Monte Carlo simulation strategy
- Genetic Algorithm strategy
- Machine-learning prediction module
- Cloud sync
- User profiles and saved simulations

---

## Contributing

Contributions are welcome.  
Please open an issue or submit a pull request.

Guidelines:
- Follow clean architecture principles
- Use consistent naming conventions
- Keep UI components reusable
- Write clear commit messages (Conventional Commits recommended)

---

## License

This project will be released under an open-source MIT license.

---

## Author

Marcos Gomes (MarquitosPT)  
Creator of DreamNumbers and the Marquitos.Schedulers library.
