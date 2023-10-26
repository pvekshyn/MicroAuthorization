Feature: Authorization

@Cleanup
Scenario: Authorization success flow
	Given I am logged in as admin

	Given user John created
	And permission Read Test created
	And role Test Admin created
	And permission Read Test added to role Test Admin

	When user John assigned to role Test Admin
	Then user John has Read Test permission

	When user John deassigned from role Test Admin
	Then user John doesn't have Read Test permission

Scenario: Not authenticated
	Given I am not logged in
	When create permission Read Test
	Then result Unauthorized

Scenario: No permission
	Given I am logged in as user without permissions
	When create permission Read Test
	Then result Forbidden
	
