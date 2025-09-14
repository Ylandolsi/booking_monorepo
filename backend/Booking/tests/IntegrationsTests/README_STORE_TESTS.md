# Store Endpoints Test Suite

## Overview

I've created comprehensive integration tests for the Store endpoints following the existing patterns in your test suite. The tests cover all CRUD operations for both Stores and Session Products.

## Test Files Created

### 1. StoresTests.cs

**Location**: `/tests/IntegrationsTests/Tests/Catalog/Stores/StoresTests.cs`

**Test Categories**:

- ✅ **Create Store Tests**

  - `CreateStore_ShouldSucceed_WhenValidDataProvided`
  - `CreateStore_ShouldFail_WhenUnauthenticated`
  - `CreateStore_ShouldFail_WhenDuplicateSlug`
  - `CreateStore_ShouldFail_WhenUserAlreadyHasStore`
  - `CreateStore_ShouldFail_WhenRequiredFieldsAreMissing` (Theory with multiple test cases)
  - `CreateStore_ShouldSucceed_WithSocialLinks`

- ✅ **Get My Store Tests**

  - `GetMyStore_ShouldSucceed_WhenStoreExists`
  - `GetMyStore_ShouldFail_WhenStoreDoesNotExist`
  - `GetMyStore_ShouldFail_WhenUnauthenticated`

- ✅ **Update Store Tests**

  - `UpdateStore_ShouldSucceed_WhenValidDataProvided`
  - `UpdateStore_ShouldFail_WhenStoreNotFound`
  - `UpdateStore_ShouldFail_WhenUnauthenticated`

- ✅ **Check Slug Availability Tests**
  - `CheckSlugAvailability_ShouldReturnTrue_WhenSlugIsAvailable`
  - `CheckSlugAvailability_ShouldReturnFalse_WhenSlugIsNotAvailable`

### 2. SessionProductsTests.cs

**Location**: `/tests/IntegrationsTests/Tests/Catalog/Products/SessionProductsTests.cs`

**Test Categories**:

- ✅ **Create Session Product Tests**

  - `CreateSessionProduct_ShouldSucceed_WhenValidDataProvided`
  - `CreateSessionProduct_ShouldFail_WhenUserHasNoStore`
  - `CreateSessionProduct_ShouldFail_WhenUnauthenticated`
  - `CreateSessionProduct_ShouldFail_WhenInvalidDataProvided` (Theory with validation scenarios)

- ✅ **Get Session Product Tests**

  - `GetSessionProduct_ShouldSucceed_WhenProductExists`
  - `GetSessionProduct_ShouldFail_WhenProductNotFound`
  - `GetSessionProduct_ShouldFail_WhenUnauthenticated`

- ✅ **Update Session Product Tests**
  - `UpdateSessionProduct_ShouldSucceed_WhenValidDataProvided`
  - `UpdateSessionProduct_ShouldFail_WhenProductNotFound`
  - `UpdateSessionProduct_ShouldFail_WhenUserDoesNotOwnProduct`

### 3. CatalogTestUtilities.cs

**Location**: `/tests/IntegrationsTests/Abstractions/CatalogTestUtilities.cs`

**Utility Classes**:

- ✅ **StoreTestData**: Helper methods for creating store test data
- ✅ **SessionProductTestData**: Helper methods for creating session product test data
- ✅ **ApiEndpoints**: Constants for all API endpoints
- ✅ **Response Verification**: Methods to verify API responses
- ✅ **Helper Methods**: For creating test entities and validation

## Key Features of the Test Suite

### 🔐 Authentication & Authorization Testing

- Tests for unauthenticated requests (401 responses)
- Tests for authorization (users can only access their own resources)
- Tests for ownership validation (users can't update other users' products)

### 📊 Data Validation Testing

- Theory tests with multiple parameter combinations
- Validation of required fields
- Validation of data ranges (price, buffer time, etc.)
- Validation of business rules (unique slugs, one store per user)

### 🗃️ Database State Testing

- Tests that create dependencies (store before session product)
- Tests for resource not found scenarios
- Tests for duplicate prevention
- Tests for data integrity

### 📸 Snapshot Testing

- Uses Snapshooter.Xunit for response verification
- Ignores dynamic fields (IDs, timestamps)
- Maintains test reliability across runs

### 🛠️ Test Organization

- Follows existing patterns from MentorshipTests
- Uses regions to organize test categories
- Descriptive test method names
- Helper methods to reduce code duplication

## Test Patterns Used

### 1. Arrange-Act-Assert Pattern

```csharp
// Arrange
var (userArrange, userAct) = GetClientsForUser("unique_user_name");
await CreateUserAndLogin(null, null, userArrange);

// Act
var response = await userAct.PostAsync("/api/catalog/stores", storeData);

// Assert
Assert.Equal(HttpStatusCode.OK, response.StatusCode);
```

### 2. Theory Tests for Multiple Scenarios

```csharp
[Theory]
[InlineData("", "valid-slug", "Title cannot be empty")]
[InlineData("Valid Title", "", "Slug cannot be empty")]
public async Task CreateStore_ShouldFail_WhenRequiredFieldsAreMissing(...)
```

### 3. Snapshot Testing

```csharp
responseContent.MatchSnapshot(matchOptions => matchOptions
    .IgnoreField("createdAt")
    .IgnoreField("picture.url"));
```

### 4. Helper Methods

```csharp
private static MultipartFormDataContent CreateValidStoreFormData(
    string title,
    string slug,
    string description = "")
```

## Endpoint Coverage

### Store Endpoints (✅ Complete)

- `POST /api/catalog/stores` - Create Store
- `GET /api/catalog/stores/my-store` - Get My Store
- `PUT /api/catalog/stores/{storeId}` - Update Store
- `GET /api/catalog/stores/slug-availability/{slug}` - Check Slug Availability

### Session Product Endpoints (✅ Complete)

- `POST /api/catalog/products/sessions` - Create Session Product
- `GET /api/catalog/products/sessions/{productId}` - Get Session Product
- `PUT /api/catalog/products/sessions/{productId}` - Update Session Product

## Running the Tests

To run the Store tests specifically:

```bash
cd /path/to/backend/Booking
dotnet test tests/IntegrationsTests/IntegrationsTests.csproj --filter "FullyQualifiedName~StoresTests"
```

To run all Catalog tests:

```bash
dotnet test tests/IntegrationsTests/IntegrationsTests.csproj --filter "FullyQualifiedName~Catalog"
```

## Test Data Management

### Unique Test Users

Each test uses unique user identifiers to prevent test interference:

```csharp
var (userArrange, userAct) = GetClientsForUser($"user_validation_{Guid.NewGuid():N}");
```

### Realistic Test Data

- Valid slugs and titles
- Realistic prices and time ranges
- Proper availability schedules
- Social media links
- File uploads with proper headers

### Error Scenarios

- Invalid input validation
- Resource not found
- Unauthorized access
- Business rule violations
- Duplicate prevention

This test suite provides comprehensive coverage of your Store and Session Product endpoints, following the existing patterns and ensuring reliability through proper isolation, validation, and response verification.
