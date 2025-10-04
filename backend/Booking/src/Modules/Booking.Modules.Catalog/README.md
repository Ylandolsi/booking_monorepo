# Catalog Module - Refactoring Documentation

## Overview

The Catalog module manages stores, products, sessions, payouts, wallets, and orders. This document outlines the major refactoring efforts to improve code quality, maintainability, and consistency.

---

#### Logging Pattern

```csharp
// Start of operation
logger.LogInformation(
    "Creating store for user {UserId} with slug {Slug} and title {Title}",
    command.UserId, command.Slug, command.Title);

// Warnings for business validation failures
logger.LogWarning(
    "User {UserId} already has a store with ID {StoreId}",
    command.UserId, existingStore.Id);

// Errors for technical failures
logger.LogError(ex,
    "Error creating store for user {UserId} with slug {Slug}",
    command.UserId, command.Slug);

// Success with details
logger.LogInformation(
    "Successfully created store {StoreId} with slug {Slug} for user {UserId}",
    store.Id, store.Slug, command.UserId);
```

**(FluentValidation):**

```csharp
// Separate validator file
public class PatchStoreCommandValidator : AbstractValidator<PatchStoreCommand>
{
    public PatchStoreCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Store title is required.")
            .MaximumLength(100)
            .WithMessage("Store title cannot exceed 100 characters.");
    }
}

// Handler is cleaner
public async Task<Result> Handle(Command command, CancellationToken ct)
{
    // Validation happens automatically before this method
    // ... business logic only
}
```

#### Key Relationships

- **One User → One Store**: A user can only own one store
- **One Store → One Wallet**: Each store has a dedicated wallet
- **One Store → Many Products**: Store can have multiple products
- **One Wallet → Many Payouts**: Track withdrawal history

---

## Code Patterns

### Validation Pattern

```csharp
public class SomeCommandValidator : AbstractValidator<SomeCommand>
{
    public SomeCommandValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title cannot exceed 100 characters.");

        RuleFor(c => c.Email)
            .EmailAddress()
            .When(c => !string.IsNullOrEmpty(c.Email))
            .WithMessage("Invalid email format.");
    }
}
```

### Error Handling Pattern

```csharp
// Use centralized errors from CatalogErrors.cs
using Booking.Modules.Catalog.Domain;

// NOT FOUND (404)
return Result.Failure(CatalogErrors.Store.NotFound);

// CONFLICT (409)
return Result.Failure(CatalogErrors.Store.SlugAlreadyExists);

// PROBLEM (400 - Business Rule)
return Result.Failure(CatalogErrors.Wallet.InsufficientBalance);

// UNAUTHORIZED (401)
return Result.Failure(CatalogErrors.Product.UnauthorizedAccess);

// FAILURE (500 - Technical Error)
return Result.Failure(CatalogErrors.Upload.ImageFailed);
```
