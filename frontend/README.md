# Patterns

## Alert

The Alert component is used to display notifications, such as success or error messages after form submissions.

### Icon Mapping

Define an icon map for different alert variants:

```javascript
const alertIconMap = {
    default: AlertCircle,
    destructive: XCircle,
    success: CheckCircle,
    warning: AlertTriangle,
    info: Info,
} as const;
```

### Error Alert Example

Use this pattern to show an error alert when a mutation fails:

```jsx
{
  becomeMentorMutation.isError && (
    <Alert variant={'destructive'}>
      {React.createElement(alertIconMap['destructive'])}
      <AlertTitle>Error</AlertTitle>
      <AlertDescription>Failed to register as a mentor. Please try again.</AlertDescription>
    </Alert>
  );
}
```

### Success Alert Example

Use this pattern to show a success alert when a mutation succeeds:

```jsx
{
  becomeMentorMutation.isSuccess && (
    <Alert variant={'success'}>
      {React.createElement(alertIconMap['success'])}
      <AlertTitle>Success</AlertTitle>
      <AlertDescription>Successfully registered as a mentor.</AlertDescription>
    </Alert>
  );
}
```

This approach provides a consistent way to handle success and error states after form submissions.
