using Template;

// Features contain vertical-slice use-case handlers. They are allowed to use
// Configuration (options, settings) but must remain ignorant of Infrastructure
// internals to keep the business logic portable and independently testable.
[assembly: RestrictNamespaceReference("Template.Api.Features", "Template.Api.Infrastructure")]

// Configuration holds wiring/options classes that are consumed by the DI root.
// Allowing it to pull in Infrastructure would create a circular setup dependency
// and blur the boundary between "what to configure" and "how to implement it".
[assembly: RestrictNamespaceReference("Template.Api.Configuration", "Template.Api.Infrastructure")]

// Infrastructure must not reach into Features. Infrastructure provides
// cross-cutting services (persistence, messaging, etc.); if it referenced
// Features it would couple the delivery mechanism to business logic, making
// it impossible to swap either side independently.
[assembly: RestrictNamespaceReference("Template.Api.Infrastructure", "Template.Api.Features")]

// Domain represents core business rules and entities. It must be the innermost
// layer with zero outward dependencies so it can be reasoned about and tested
// in complete isolation from technical concerns.
[assembly: RestrictNamespaceReference("Template.Api.Domain", "Template.Api.Infrastructure")]
[assembly: RestrictNamespaceReference("Template.Api.Domain", "Template.Api.Features")]
[assembly: RestrictNamespaceReference("Template.Api.Domain", "Template.Api.Configuration")]
