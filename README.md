# Movie API

This is a .NET Core API project for a movie application with MongoDB and JWT Authentication.

## Overview

This project serves as the backend for a movie application, providing functionality for user authentication, movie management, user comments, and ratings.

## Features

- User authentication with JWT (JSON Web Token).
- Access and refresh tokens for secure authentication.
- CRUD operations for movies.
- User comments on movies.
- Movie rating system.

## Technologies Used

- .NET Core
- MongoDB
- JWT (JSON Web Token)

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-username/your-movie-api.git

2. **Configure MongoDB connection in appsettings.json:**
    ```sh
    {
      "ConnectionStrings": {
        "MongoDB": "mongodb://localhost:27017/yourdatabase"
      }
    }


