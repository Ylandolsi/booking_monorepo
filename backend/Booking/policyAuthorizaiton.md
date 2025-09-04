Multiple handlers for one policy 
policy is alid if 
* at least one of the policy handlers invoke context.Succeed()
and
* none of the policy handler invoke context.fail


order of policy dosent matter 
all policies are being exucted by default for the approrpiate endpoint even inf on of them fails 
but this can be configured 


- [] PolicyAuthorizationHandler vs IauthorizationHandler