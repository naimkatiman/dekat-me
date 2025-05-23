This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

# Dekat.me

A comprehensive local business directory platform connecting users with nearby businesses and services.

## Features

- Business listings with detailed information
- User reviews and ratings
- Category-based browsing
- Location-based search
- Interactive map view
- Business submission portal
- Secure authentication system
- Comprehensive API for third-party integration

## Technology Stack

### Frontend
- Next.js
- TypeScript
- Tailwind CSS
- Supabase

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- SQL Server / PostgreSQL
- xUnit for testing

## Project Structure

```
dekat-me/
├── src/               # Frontend source code (Next.js)
├── backend/           # C# Backend components
│   ├── DekatMe.Api/           # ASP.NET Core Web API
│   │   ├── Controllers/       # API endpoints
│   │   ├── Models/            # Data models
│   │   ├── Services/          # Business logic
│   │   ├── Data/              # Database context
│   │   ├── Auth/              # Authentication system
│   │   └── Middleware/        # Request/response middleware
│   ├── DekatMe.Core/          # Core business logic
│   │   ├── Entities/          # Domain entities
│   │   └── Utilities/         # Helper classes
│   ├── DekatMe.Console/       # Administrative console application
│   └── DekatMe.Tests/         # Unit and integration tests
└── public/            # Static assets
```

## Getting Started

### Frontend Development

Run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

### Backend Development

Navigate to the backend API directory:

```bash
cd backend/DekatMe.Api
```

Run the API:

```bash
dotnet run
```

The API will be available at [http://localhost:5000](http://localhost:5000).

## API Documentation

The backend API provides endpoints for:

- Business management (CRUD operations)
- Category management
- User authentication
- Review submission and management
- Geocoding and location services

API documentation is available via Swagger at `/swagger` when running the backend server.

## Testing

Run the backend tests:

```bash
cd backend/DekatMe.Tests
dotnet test
```

Run the frontend tests:

```bash
npm test
# or
yarn test
```

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

To learn more about the technologies used:

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core)

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.

The backend can be deployed to:
- Azure App Service
- AWS Elastic Beanstalk
- Docker containers
