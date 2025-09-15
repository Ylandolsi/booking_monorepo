### review prompt

```text

Could you perform a deep review of these files, covering all aspects including:

Logging (clarity, consistency, log levels, structured logs)

Error handling (exceptions, validation errors, edge cases, error response format)

Business logic (correctness, maintainability, testability, separation of concerns)

API/endpoint conventions (URL naming, HTTP verbs, status codes, request/response shape, REST consistency)

Security and validation (auth checks, data validation, potential vulnerabilities)

Code style and structure (readability, adherence to conventions, duplication, potential refactors)

```

### review json format

```json

{
  "title": "Deep review — <Feature or Flow Name> (<Module/Context>)",
  "repo_path": "<repository path or file scope>",
  "module_status": "<new | migrated | legacy | experimental>",
  "scope": "<High-level description of what to review end-to-end>",
  "deliverables": {
    "<topic_area>": {
      "description": "<Short summary of what this area covers>",
      "requirements": [
        "List of concrete items to check",
        "Each should be action-oriented and specific",
        "Avoid vague words like 'optimize' — prefer 'ensure X follows Y convention'"
      ]
    }
    // repeat topic areas as needed: api_and_contracts, domain_model, logging, etc.
  },
  "constraints": "<Rules for the review: e.g., no implementation code, keep architecture, etc.>",
  "priority_and_formatting": {
    "priority": "How to prioritize findings (e.g., High/Medium/Low).",
    "output_format": "Expected structure of the output (e.g., JSON with 'issues', 'risks', 'recommendations', 'checklist')."
  }
}

```