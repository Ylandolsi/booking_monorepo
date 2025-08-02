# Users Domain Layer

This document describes the domain layer structure and design patterns used in the Users module.

## Domain Structure

### Entities

#### User (Aggregate Root)

- **Primary Key**: `int Id`
- **Unique Identifier**: `string Slug`
- **Responsibilities**: Central user aggregate that manages user profile, authentication status, and relationships
- **Key Methods**:
  - `UpdateBio(string bio)`: Updates user bio with validation
  - `UpdateGender(string gender)`: Updates gender with validation
  - `UpdateName(string firstName, string lastName)`: Updates user name
  - `AddExpertise(int expertiseId)`: Adds expertise with limit validation
  - `RemoveExpertise(int expertiseId)`: Removes expertise
  - `AddLanguage(int languageId)`: Adds language with limit validation
  - `RemoveLanguage(int languageId)`: Removes language

#### Experience

- **Responsibilities**: Manages user work experience
- **Factory Method**: `Create(...)` with validation
- **Key Methods**: `Update(...)`, `Complete(DateTime endDate)`

#### Education

- **Responsibilities**: Manages user education history
- **Factory Method**: `Create(...)` with validation
- **Key Methods**: `Update(...)`, `Complete(DateTime endDate)`

#### Expertise

- **Responsibilities**: Defines available skills/expertise
- **Factory Method**: `Create(...)` with validation

#### Language

- **Responsibilities**: Defines available languages
- **Factory Method**: `Create(...)` with validation

#### RefreshToken

- **Responsibilities**: Manages JWT refresh tokens
- **Factory Method**: `Create(...)` with validation
- **Key Methods**: `Revoke()`

### Value Objects

#### Name

- Encapsulates first and last name with validation
- Provides `FullName` computed property

#### Email

- Validates email format and provides normalization
- Tracks verification status

#### Status

- Manages mentor/mentee status and activation state
- Methods: `BecomeMentor()`, `ToggleActivation()`, `Activate()`, `Deactivate()`

#### ProfilePicture

- Manages profile and thumbnail image URLs
- Validation for URL format and file constraints

#### SocialLinks

- Encapsulates social media links (LinkedIn, Twitter, GitHub, etc.)

#### ProfileCompletionStatus

- Tracks profile completion percentage
- Automatically updates based on user data

### Domain Services

#### UserDomainService

- `ValidateExpertiseLimit()`: Enforces expertise limits
- `ValidateLanguageLimit()`: Enforces language limits
- `UpdateProfileCompletionStatus()`: Updates completion and raises events
- `CanBecomeMentor()`: Validates mentor eligibility

### Domain Events

#### User Events

- `UserRegisteredDomainEvent`: Raised when user registers
- `UserProfileUpdatedDomainEvent`: Raised when profile is updated
- `UserExpertiseAddedDomainEvent` / `UserExpertiseRemovedDomainEvent`
- `UserLanguageAddedDomainEvent` / `UserLanguageRemovedDomainEvent`

#### Profile Events

- `ProfileCompletedDomainEvent`: Raised when profile reaches 100% completion
- `ProfileHalfCompletedDomainEvent`: Raised when profile reaches 50% completion
- `UserBecameMentorDomainEvent`: Raised when user becomes a mentor

### Business Rules

#### User Constraints

- Maximum 4 expertises per user (`UserConstraints.MaxExpertises`)
- Maximum 4 languages per user (`UserConstraints.MaxLanguages`)
- Bio maximum length: 1000 characters (`UserConstraints.MaxBioLength`)

#### Mentor Eligibility

- Profile must be at least 80% complete
- Must have at least one experience
- Must have at least one expertise

#### Email Verification

- Verification tokens expire after 5 minutes
- Users cannot access certain features until email is verified

### Error Handling

All domain operations return `Result<T>` or `Result` types for explicit error handling:

- **UserErrors**: User-specific validation errors
- **ExperienceErrors**: Experience validation errors
- **EducationErrors**: Education validation errors
- **StatusErrors**: Status transition errors
- **RefreshTokenErrors**: Token validation errors

### Design Patterns Used

1. **Aggregate Pattern**: User is the aggregate root
2. **Factory Pattern**: Static factory methods for entity creation
3. **Value Object Pattern**: Immutable objects for complex types
4. **Domain Events**: Loose coupling between domain operations
5. **Result Pattern**: Explicit error handling without exceptions
6. **Domain Services**: Complex business logic that doesn't belong to entities

### Integration Points

- **Entity Framework**: All entities configured via `IEntityTypeConfiguration`
- **Identity Framework**: User inherits from `IdentityUser<int>`
- **Outbox Pattern**: Domain events converted to outbox messages
- **MediatR**: Domain events dispatched via pipeline

## Usage Examples

```csharp
// Creating a new user
var user = User.Create("john-doe", "John", "Doe", "john@example.com", "profile.jpg");

// Adding expertise with validation
var result = user.AddExpertise(expertiseId);
if (result.IsFailure)
{
    // Handle validation error
}

// Updating bio
var bioResult = user.UpdateBio("This is my bio");

// Becoming a mentor
var domainService = new UserDomainService();
var canBecomeMentor = domainService.CanBecomeMentor(user);
if (canBecomeMentor.IsSuccess)
{
    user.Status.BecomeMentor();
}
```
