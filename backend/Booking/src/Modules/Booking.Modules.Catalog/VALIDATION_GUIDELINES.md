# Catalog Module - Validation Strategy & Guidelines

## Overview

This document defines the validation strategy for the Booking.Modules.Catalog module, establishing clear boundaries between FluentValidation (DTO/Input validation) and domain validation (business rules).

## Validation Layers

### 1. Input Validation (FluentValidation) ✅

**Purpose**: Validate request DTOs, ensuring data format and basic constraints are met before reaching the handler.

**Location**: `*Validator.cs` files alongside commands/queries

**Responsibilities**:

- Data format validation (email, phone, URL patterns)
- Length constraints (min/max)
- Required fields
- Range validation
- Pattern matching (regex)
- Cross-field validation (within same DTO)

**Example**:

```csharp
public class PayoutCommandValidator : AbstractValidator<PayoutCommand>
{
    public PayoutCommandValidator()
    {
        RuleFor(p => p.Amount)
            .GreaterThanOrEqualTo(20)
            .WithMessage("Amount must be at least $20.")
            .LessThanOrEqualTo(1000)
            .WithMessage("Amount cannot exceed $1,000.");
    }
}
```

### 2. Domain Validation (Entity Methods) 🔄

**Purpose**: Enforce business rules and invariants within domain entities.

**Location**: Domain entity methods

**Responsibilities**:

- Business rule enforcement
- State transitions
- Aggregate consistency
- Complex business logic
- Cross-aggregate validation (when necessary)

**Example**:

```csharp
public class Wallet : Entity
{
    public Result UpdateBalance(decimal amount)
    {
        // Business rule: Cannot have negative balance
        if (Balance + amount < 0)
            return Result.Failure(CatalogErrors.Wallet.InsufficientBalance);

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
```

### 3. Handler Validation (Cross-Entity) ✅

**Purpose**: Validate business scenarios that involve multiple entities or external systems.

**Location**: Command/Query handlers

**Responsibilities**:

- Authorization checks
- Entity existence validation
- Cross-entity business rules
- External service constraints

**Example**:

```csharp
public class PayoutCommandHandler : ICommandHandler<PayoutCommand>
{
    public async Task<Result> Handle(PayoutCommand command, ...)
    {
        // Existence validation
        var store = await dbContext.Stores.FirstOrDefaultAsync(...);
        if (store is null)
            return Result.Failure(CatalogErrors.Store.NotFound);

        // Authorization validation
        if (store.UserId != command.UserId)
            return Result.Failure(CatalogErrors.Product.UnauthorizedAccess);

        // External service validation
        if (string.IsNullOrEmpty(konnectWalletId))
            return Result.Failure(CatalogErrors.Payout.KonnectNotIntegrated);

        // ... continue processing
    }
}
```

## Current Validators Audit

### ✅ Well-Structured Validators

#### 1. PayoutCommandValidator

```csharp
public class PayoutCommandValidator : AbstractValidator<PayoutCommand>
{
    public PayoutCommandValidator()
    {
        RuleFor(p => p.Amount)
            .GreaterThanOrEqualTo(20)
            .WithMessage("Amount must be at least $20.")  // ✅ Clear message
            .LessThanOrEqualTo(1000)
            .WithMessage("Amount cannot exceed $1,000.");  // ✅ Clear message
    }
}
```

**Status**: ✅ Good - Clear, concise, appropriate constraints

#### 2. CreateStoreCommandValidator

```csharp
internal sealed class CreateStoreCommandValidator : AbstractValidator<PatchStoreCommand>
{
    private static readonly string[] ValidPlatforms = new[]
    {
        "portfolio", "github", "linkedin", "fb", "instagram", "tiktok", "twitter"
    };

    public CreateStoreCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Store title is required.")
            .MaximumLength(100).WithMessage("Store title cannot exceed 100 characters.")
            .MinimumLength(3).WithMessage("Store title must be at least 3 characters.");

        RuleFor(c => c.Slug)
            .NotEmpty().WithMessage("Store slug is required.")
            .MaximumLength(100).WithMessage("Store slug cannot exceed 100 characters.")
            .MinimumLength(3).WithMessage("Store slug must be at least 3 characters.")
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Store slug can only contain lowercase letters, numbers, and hyphens.");

        RuleForEach(c => c.SocialLinks)
            .SetValidator(new SocialLinkValidator())
            .When(c => c.SocialLinks != null && c.SocialLinks.Any());

        RuleFor(c => c.SocialLinks)
            .Must(HaveUniquePlatforms)
            .WithMessage("Social links must have unique platforms.")
            .When(c => c.SocialLinks != null && c.SocialLinks.Any());
    }

    private bool HaveUniquePlatforms(List<SocialLink>? links)
    {
        if (links == null || !links.Any()) return true;
        return links.Select(l => l.Platform.ToLowerInvariant()).Distinct().Count() == links.Count;
    }
}
```

**Status**: ✅ Excellent - Comprehensive with custom validation logic

### Validation Best Practices

#### ✅ DO:

1. **Use clear, user-friendly messages**

   ```csharp
   .WithMessage("Store title must be at least 3 characters.")  // ✅ Clear
   ```

2. **Group related validations**

   ```csharp
   RuleFor(c => c.Title)
       .NotEmpty().WithMessage("...")
       .MinimumLength(3).WithMessage("...")
       .MaximumLength(100).WithMessage("...");
   ```

3. **Use constants for magic numbers**

   ```csharp
   private const int MinTitleLength = 3;
   private const int MaxTitleLength = 100;

   RuleFor(c => c.Title)
       .MinimumLength(MinTitleLength)
       .MaximumLength(MaxTitleLength);
   ```

4. **Conditional validation with When()**

   ```csharp
   RuleFor(c => c.Description)
       .MaximumLength(500)
       .When(c => !string.IsNullOrEmpty(c.Description));
   ```

5. **Custom validators for complex logic**
   ```csharp
   RuleFor(c => c.SocialLinks)
       .Must(HaveUniquePlatforms)
       .WithMessage("Social links must have unique platforms.");
   ```

#### ❌ DON'T:

1. **Don't mix validation concerns**

   ```csharp
   // ❌ Bad: Database query in validator
   RuleFor(c => c.Slug)
       .Must(slug => !dbContext.Stores.Any(s => s.Slug == slug))
       .WithMessage("Slug already exists");

   // ✅ Good: Check in handler
   var existingStore = await dbContext.Stores.FirstOrDefaultAsync(s => s.Slug == command.Slug);
   if (existingStore != null)
       return Result.Failure(CatalogErrors.Store.SlugAlreadyExists);
   ```

2. **Don't validate business rules in FluentValidation**

   ```csharp
   // ❌ Bad: Business rule in validator
   RuleFor(p => p.Amount)
       .Must((command, amount) => CheckWalletBalance(command.UserId, amount))
       .WithMessage("Insufficient balance");

   // ✅ Good: Business rule in handler
   if (wallet.Balance < command.Amount)
       return Result.Failure(CatalogErrors.Wallet.InsufficientBalance);
   ```

3. **Don't use technical jargon in messages**
   ```csharp
   .WithMessage("Slug regex validation failed");  // ❌ Bad
   .WithMessage("Store slug can only contain lowercase letters, numbers, and hyphens.");  // ✅ Good
   ```

## Validation Constants

### Recommended Approach:

Create a `ValidationConstants.cs` file for shared validation values:

```csharp
namespace Booking.Modules.Catalog.Features;

public static class ValidationConstants
{
    public static class Store
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 100;
        public const int DescriptionMaxLength = 500;
        public const string SlugPattern = "^[a-z0-9-]+$";
    }

    public static class Product
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 255;
        public const int DescriptionMaxLength = 2000;
        public const decimal MinPrice = 0.01m;
        public const decimal MaxPrice = 999999.99m;
    }

    public static class Payout
    {
        public const decimal MinAmount = 20m;
        public const decimal MaxAmount = 1000m;
    }

    public static class Upload
    {
        public const int MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        public static readonly string[] AllowedImageTypes = { "image/jpeg", "image/png", "image/webp" };
    }
}
```

## Validation Error Messages Guidelines

### Format:

- **Start with what's wrong**: "Amount must be..."
- **Be specific**: "at least $20" not "too small"
- **Be helpful**: Suggest the correct format when possible

### Examples:

#### ✅ Good Messages:

```csharp
"Store title must be between 3 and 100 characters."
"Store slug can only contain lowercase letters, numbers, and hyphens."
"Amount must be between $20 and $1,000."
"Please upload an image file (JPEG, PNG, or WebP)."
```

#### ❌ Bad Messages:

```csharp
"Invalid title"  // Not specific enough
"Slug validation failed"  // Technical jargon
"Wrong format"  // Not helpful
"Error"  // Completely useless
```

## Integration with Result Pattern

### Validation Flow:

```
1. FluentValidation (automatic via middleware)
   ↓ (if valid)
2. Handler validation (existence, authorization)
   ↓ (if valid)
3. Domain validation (business rules)
   ↓ (if valid)
4. Execute business logic
```

### Error Mapping:

```csharp
// FluentValidation errors → ValidationError
if (validationResult.Errors.Any())
{
    var errors = validationResult.Errors
        .Select(e => new Error(e.PropertyName, e.ErrorMessage, ErrorType.Validation))
        .ToArray();
    return Result.Failure(new ValidationError(errors));
}

// Handler/Domain errors → Specific Error Types
return Result.Failure(CatalogErrors.Wallet.InsufficientBalance);  // Problem (400)
return Result.Failure(CatalogErrors.Store.NotFound);  // NotFound (404)
```

## Recommended Improvements

### 1. Create ValidationConstants.cs

Centralize all validation constants to avoid magic numbers.

### 2. Review Domain Entities

Add validation methods to entities where business rules are enforced:

```csharp
public class Wallet : Entity
{
    public Result Withdraw(decimal amount)
    {
        if (amount <= 0)
            return Result.Failure(Error.Problem("Amount.Invalid", "Amount must be positive."));

        if (Balance < amount)
            return Result.Failure(CatalogErrors.Wallet.InsufficientBalance);

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
```

### 3. Standardize Error Messages

Review all validators and ensure messages follow the guidelines above.

### 4. Add Validator Tests

```csharp
public class PayoutCommandValidatorTests
{
    private readonly PayoutCommandValidator _validator = new();

    [Theory]
    [InlineData(19.99)]  // Below minimum
    [InlineData(1000.01)]  // Above maximum
    public void Validate_InvalidAmount_ReturnsError(decimal amount)
    {
        // Arrange
        var command = new PayoutCommand(userId: 1, amount: amount);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(PayoutCommand.Amount));
    }

    [Theory]
    [InlineData(20)]
    [InlineData(500)]
    [InlineData(1000)]
    public void Validate_ValidAmount_ReturnsSuccess(decimal amount)
    {
        // Arrange
        var command = new PayoutCommand(userId: 1, amount: amount);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}
```

## Summary

| Aspect                 | Status     | Notes                                 |
| ---------------------- | ---------- | ------------------------------------- |
| FluentValidation Usage | ✅ Good    | Well-structured validators            |
| Error Messages         | ✅ Good    | Clear and user-friendly               |
| Validation Constants   | 🔄 Improve | Should be centralized                 |
| Domain Validation      | 🔄 Review  | Some entities need validation methods |
| Validation Tests       | ❌ Missing | Should add comprehensive tests        |

---

**Document Status**: Phase 1 Complete
**Last Updated**: October 4, 2025
**Module**: Booking.Modules.Catalog
